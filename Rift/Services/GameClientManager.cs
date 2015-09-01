using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using Rift.Properties;

namespace Rift.Services
{
    /// <summary>
    /// Specifies a game client special folders.
    /// </summary>
    public enum ClientSpecialFolder
    {
        /// <summary>
        /// Client root directory.
        /// </summary>
        Root,
        /// <summary>
        /// Client 'bin32' folder.
        /// </summary>
        Bin32,
        /// <summary>
        /// Client 'bin64' folder.
        /// </summary>
        Bin64,
        /// <summary>
        /// Client 'Data' folder.
        /// </summary>
        Data,
        /// <summary>
        /// Client 'L10N' folder.
        /// </summary>
        L10N,
        /// <summary>
        /// Client 'Screenshot' folder.
        /// </summary>
        Screenshot
    }

    /// <summary>
    /// Represents the game client manager.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class GameClientManager
    {
        private const string ExecutableName = "aion.bin";
        private const string GameModuleName = "Game.dll";
        private const int ExecutableMajorVersion = 4500;

        private readonly DirectoryInfo pathInfo;
        private readonly Dictionary<ClientSpecialFolder, string> structure; 
        
        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.GameClientManager"/> class.
        /// </summary>
        /// <param name="path">A string path to the game files.</param>
        public GameClientManager(string path)
        {
            pathInfo = new DirectoryInfo(path);

            structure = new Dictionary<ClientSpecialFolder, string>
            {
                { ClientSpecialFolder.Root, path },
                { ClientSpecialFolder.Bin32, Path.Combine(path, "bin32") },
                { ClientSpecialFolder.Bin64, Path.Combine(path, "bin64") },
                { ClientSpecialFolder.Data, Path.Combine(path, "Data") },
                { ClientSpecialFolder.L10N, Path.Combine(path, "L10N") },
                { ClientSpecialFolder.Screenshot, Path.Combine(path, "Screenshot") }
            };
        }

        /// <summary>
        /// Returns a string path to the game client special folder.
        /// </summary>
        /// <param name="specialFolder">
        /// An enumerated constant that identifies a game client special folder. 
        /// </param>
        public string GetFolderPath(ClientSpecialFolder specialFolder = ClientSpecialFolder.Root)
        {
            return structure[specialFolder];
        }

        /// <summary>
        /// Returns a string path to the game client executable.
        /// </summary>
        /// <param name="is64BitClient">
        /// A value that indicates whether the 64 bit client has a priority.
        /// </param>
        public string GetExecutablePath(bool is64BitClient = false)
        {
            return Path.Combine(
                is64BitClient ? structure[ClientSpecialFolder.Bin64] : structure[ClientSpecialFolder.Bin32],
                ExecutableName);
        }

        /// <summary>
        /// Enumerates all files that contains in the game client root directory.
        /// </summary>
        public IEnumerable<FileInfo> EnumerateFiles()
        {
            return pathInfo.EnumerateFiles("*.*", SearchOption.AllDirectories);
        }

        /// <summary>
        /// Validates the game client structure.
        /// </summary>
        /// <param name="checkExecutable">
        /// A value that indicates whether this method should check the executable files too.
        /// </param>
        /// <exception cref="Rift.Services.GameClientException"/>
        public bool IsClientStructureValid(bool checkExecutable = true)
        {
            if (!pathInfo.Exists) return false;

            if (checkExecutable)
            {
                var pathExec32 = Path.Combine(structure[ClientSpecialFolder.Bin32], ExecutableName);
                var pathExec64 = Path.Combine(structure[ClientSpecialFolder.Bin64], ExecutableName);
                var pathGame32 = Path.Combine(structure[ClientSpecialFolder.Bin32], GameModuleName);
                var pathGame64 = Path.Combine(structure[ClientSpecialFolder.Bin64], GameModuleName);

                if (!File.Exists(pathExec32) ||
                    !File.Exists(pathExec64))
                    throw new GameClientException(Resources.ExceptionMissingExecutable);

                var executable32Info = FileVersionInfo.GetVersionInfo(pathExec32);
                var executable64Info = FileVersionInfo.GetVersionInfo(pathExec64);
                var gameModule32Info = FileVersionInfo.GetVersionInfo(pathGame32);
                var gameModule64Info = FileVersionInfo.GetVersionInfo(pathGame64);
                var exeVersion32 = Version.Parse(executable32Info.ProductVersion);
                var exeVersion64 = Version.Parse(executable64Info.ProductVersion);
                var dllVersion32 = Version.Parse(gameModule32Info.ProductVersion);
                var dllVersion64 = Version.Parse(gameModule64Info.ProductVersion);

                if (exeVersion32.Major < ExecutableMajorVersion ||
                    exeVersion64.Major < ExecutableMajorVersion ||
                    dllVersion32.Major < ExecutableMajorVersion ||
                    dllVersion64.Major < ExecutableMajorVersion) 
                    throw new GameClientException(
                        string.Format(Resources.ExceptionClientVersionFormat, dllVersion32), 
                        dllVersion32);
            }

            foreach (var folder in structure.Values)
                if (!Directory.Exists(folder))
                    return false;

            return true;
        }
    }

    /// <summary>
    /// Represents errors that occur in game client manager.
    /// This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class GameClientException : Exception
    {
        /// <summary>
        /// Gets a game client version.
        /// </summary>
        public Version ClientVersion { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.GameClientException"/> class
        /// with a specified game client version.
        /// </summary>
        /// <param name="clientVersion">A game client executable version.</param>
        public GameClientException(Version clientVersion = default (Version))
        {
            ClientVersion = clientVersion;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.GameClientException"/> class
        /// with a specified game client version and error message.
        /// </summary>
        /// <param name="message">A error text message that describes the error.</param>
        /// <param name="clientVersion">A game client executable version.</param>
        public GameClientException(string message, Version clientVersion = default (Version))
            : base(message)
        {
            ClientVersion = clientVersion;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.GameClientException"/> class
        /// with a specified game client version, error message and a reference to the inner exception
        /// that is cause of this exception.
        /// </summary>
        /// <param name="message">A error text message that describes the error.</param>
        /// <param name="innerException">The exception that is cause of the current exception.</param>
        /// <param name="clientVersion">A game client executable version.</param>
        public GameClientException(string message, Exception innerException, Version clientVersion = default (Version))
            : base(message, innerException)
        {
            ClientVersion = clientVersion;
        }

        private GameClientException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            var temp = info.GetValue("version", typeof (Version)) as Version;

            ClientVersion = temp ?? new Version();
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="System.Runtime.Serialization.SerializationInfo"/>
        /// with information about the exception.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data
        /// about the exception being thrown.
        /// </param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains 
        /// contextual information about the source or destination.
        /// </param>
        /// <exception cref="System.ArgumentNullException"/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("version", ClientVersion);
        }
    }
}