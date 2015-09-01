using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Rift.Utils
{
    /// <summary>
    /// Represents a callback method that is called when
    /// the image was downloaded asynchronously.
    /// </summary>
    /// <param name="image">A downloaded image that is received.</param>
    public delegate void AsyncImageCallback(Image image);

    /// <summary>
    /// Represents an image files cache.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class ImageCache : IDisposable
    {
        private readonly string path;
        
        private readonly Dictionary<int, List<AsyncImageCallback>> callbacks;
        private readonly Dictionary<string, Image> memoryCache;
        private readonly Queue<WebClient> clients;
        
        /// <summary>
        /// Gets the cached image files location.
        /// </summary>
        public string Location
        {
            get { return path; }
        }

        /// <summary>
        /// Gets a total amount of cached files
        /// that is places in memory.
        /// </summary>
        public int TotalCached
        {
            get { return memoryCache.Count; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Utils.ImageCache"/> class
        /// from the specified cached files location.
        /// </summary>
        /// <param name="path">A string path to the cached files.</param>
        public ImageCache(string path)
        {
            if (string.IsNullOrEmpty(path))
                path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            if (!path.Contains(GetType().Name))
                path = Path.Combine(path, GetType().Name);

            this.path = path;

            try
            {
                if (!Directory.Exists(this.path))
                    Directory.CreateDirectory(this.path);
            }
            catch (UnauthorizedAccessException) { }
            catch (IOException) { }
            
            memoryCache = new Dictionary<string, Image>();
            callbacks = new Dictionary<int, List<AsyncImageCallback>>();
            clients = new Queue<WebClient>();

            DiscoverCacheFolder();
        }

        private void DiscoverCacheFolder()
        {
            if (!Directory.Exists(path)) return;

            foreach (var file in Directory.GetFiles(path))
                CacheImageFile(file);
        }

        private Image CacheImageFile(string file)
        {
            if (string.IsNullOrEmpty(file)) return null;
            if (!File.Exists(file)) return null;

            Image temp = null;
            var fileName = Path.GetFileName(file);

            if (memoryCache.ContainsKey(fileName))
            {
                return memoryCache[fileName];
            }

            try
            {
                using (var stream = WaitForFile(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (stream != null)
                        temp = Image.FromStream(stream);
                }
            }
            catch (OutOfMemoryException) { }

            if (temp != null)
                memoryCache.Add(fileName, temp);
            
            return temp;
        }

        private static FileStream WaitForFile(string fullPath, FileMode mode, FileAccess access, FileShare share)
        {
            for (var i = 0; i < 100; i++)
            {
                try
                {
                    var fs = File.Open(fullPath, mode, access, share);

                    fs.ReadByte();
                    fs.Seek(0, SeekOrigin.Begin);

                    return fs;
                }
                catch (IOException)
                {
                    Thread.Sleep(50);
                }
            }

#if DEBUG
            Debug.WriteLine("STREAM TIMEOUT");
#endif

            return null;
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var token = e.UserState.GetHashCode();
            if (!callbacks.ContainsKey(token)) return;
            
            if (e.Cancelled ||
                e.Error != null)
                foreach (var call in callbacks[token])
                    call(null);

            var fileFullName = (string) e.UserState;
            var fileName = Path.GetFileName(fileFullName);

            if (memoryCache.ContainsKey(fileName))
                foreach (var call in callbacks[token])
                    call(memoryCache[fileName].Clone() as Image);
            else
            {
                var image = CacheImageFile(fileFullName);

                if (image != null)
                    foreach (var call in callbacks[token])
                        call(image.Clone() as Image);
            }

            callbacks.Remove(token);
        }

        /// <summary>
        /// Gets an image by the specified uniform resource identifier (URI) asynchronously.
        /// </summary>
        /// <param name="uri">A uniform resource identifier (URI) to the image.</param>
        /// <param name="callback">
        /// This method will be called when the downloading operation completed.
        /// </param>
        /// <exception cref="System.UriFormatException"/>
        public void GetImageAsync(string uri, AsyncImageCallback callback)
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                throw new UriFormatException();

            Uri imageUri;
            var temp = Uri.TryCreate(uri, UriKind.Absolute, out imageUri);

            if (!temp) throw new UriFormatException();

            var fileName = imageUri.Segments.Last();
            var fileFullName = Path.Combine(path, fileName);

            if (memoryCache.ContainsKey(fileName))
            {
                callback(memoryCache[fileName].Clone() as Image);
                return;
            }
            
            var token = fileFullName.GetHashCode();

            if (callbacks.ContainsKey(token))
                callbacks[token].Add(callback);
            else
                callbacks.Add(token, new List<AsyncImageCallback> { callback });
            
            try
            {
                foreach (var client in clients.Where(c => !c.IsBusy))
                {
                    client.DownloadFileAsync(imageUri, fileFullName, fileFullName);
                    return;
                }
                
                var newClient = new WebClient();
                newClient.DownloadFileCompleted += client_DownloadFileCompleted;

                clients.Enqueue(newClient);

                newClient.DownloadFileAsync(imageUri, fileFullName, fileFullName);
            }
            catch (WebException)
            {
                callbacks.Remove(token);
            }
        }

        /// <summary>
        /// Releases all resources used by this <see cref="Rift.Utils.ImageCache"/>.
        /// </summary>
        public void Dispose()
        {
            foreach (var client in clients)
            {
                if (client.IsBusy)
                    client.CancelAsync();

                client.DownloadDataCompleted -= client_DownloadFileCompleted;
                client.Dispose();
            }

            if (memoryCache != null)
            {
                foreach (var image in memoryCache.Values)
                    image.Dispose();

                memoryCache.Clear();
            }
            
            if (callbacks != null)
                callbacks.Clear();
        }
    }
}