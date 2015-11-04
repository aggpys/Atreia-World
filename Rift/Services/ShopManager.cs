using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Rift.Data;
using System.ComponentModel;
using Rift.Properties;

namespace Rift.Services
{
    /// <summary>
    /// Specifies the item bought results.
    /// </summary>
    [Flags]
    public enum ItemBoughtResult
    {
        /// <summary>
        /// Success.
        /// </summary>
        Success = 0,
        /// <summary>
        /// Failure.
        /// </summary>
        Failure = 1,
        /// <summary>
        /// Item ID is not found.
        /// </summary>
        IdMismatch = 2,
        /// <summary>
        /// Character name is not found.
        /// </summary>
        CharacterMismatch = 4,
        /// <summary>
        /// The service is unavailable.
        /// </summary>
        ServiceUnavailable = 8,
        /// <summary>
        /// Not enough points.
        /// </summary>
        NotEnoughPoints = 16
    }

    /// <summary>
    /// Provides data for the <see cref="Rift.Services.ShopManager.PriceListReady"/> event.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class PriceListReadyEventArgs : EventArgs
    {
        public GameShop PriceList { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.PriceListReadyEventArgs"/> class
        /// from the specified price list.
        /// </summary>
        /// <param name="priceList">A collection of named <see cref="Rift.Data.ShopItemCollection"/>.</param>
        public PriceListReadyEventArgs(GameShop priceList)
        {
            PriceList = priceList;
        }
    }

    /// <summary>
    /// Provides data for the <see cref="Rift.Services.ShopManager.PriceListLoading"/> event.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class PriceListLoadingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a total amount of bytes to load.
        /// </summary>
        public long TotalBytes { get; private set; }

        /// <summary>
        /// Gets a count of received bytes.
        /// </summary>
        public long BytesReceived { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.PriceListLoadingEventArgs"/> class.
        /// </summary>
        /// <param name="received">A count of received bytes.</param>
        /// <param name="total">A total amount of bytes to load.</param>
        public PriceListLoadingEventArgs(long received, long total)
        {
            BytesReceived = received;
            TotalBytes = total;
        }
    }

    /// <summary>
    /// Provides data for the <see cref="Rift.Services.ShopManager.ItemBought"/> event.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class ItemBoughtEventArgs : EventArgs
    {
        /// <summary>
        /// Gets an item bought result.
        /// </summary>
        public ItemBoughtResult Result { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.ItemBoughtEventArgs"/> class.
        /// </summary>
        /// <param name="result">An item bought result.</param>
        public ItemBoughtEventArgs(ItemBoughtResult result)
        {
            Result = result;
        }
    }

    /// <summary>
    /// Provides data for the <see cref="Rift.Services.ShopManager.PointsReceived"/> event.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class PointsReceivedEventArgs : EventArgs
    {
        public int Points { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.PointsReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="points">A points count that received.</param>
        public PointsReceivedEventArgs(int points)
        {
            Points = points;
        }
    }

    /// <summary>
    /// Represents an in-game shop manager.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class ShopManager : IDisposable
    {
        private const string DefaultNamespace = "http://atreiaworld.com/ings-1.0.xsd";
        
        /// <summary>
        /// Occurs when the in-game shop price list is ready to use.
        /// </summary>
        public event EventHandler<PriceListReadyEventArgs> PriceListReady;

        /// <summary>
        /// Occurs during the in-game shop price list loading.
        /// </summary>
        public event EventHandler<PriceListLoadingEventArgs> PriceListLoading;

        /// <summary>
        /// Occurs when the shop item was bought.
        /// </summary>
        public event EventHandler<ItemBoughtEventArgs> ItemBought;

        /// <summary>
        /// Occurs when the in-game points was received.
        /// </summary>
        public event EventHandler<PointsReceivedEventArgs> PointsReceived;

        private readonly WebClient client;
        private readonly WebClient updateClient;
        private readonly string path;

        private static GameShop ReadPriceList(string path)
        {
            XDocument xdoc = null;

            try
            {
                xdoc = XDocument.Load(path);
            }
            catch (XmlException) { }

            if (xdoc == null || xdoc.Root == null) return null;

            var xNameCategories = XName.Get("categories", DefaultNamespace);
            var xNameCategory = XName.Get("category", DefaultNamespace);
            var xNameItem = XName.Get("item", DefaultNamespace);

            var result = new GameShop();
            var categoriesRoot = xdoc.Root.Element(xNameCategories);

            if (categoriesRoot == null)
                return null;

            foreach (var xcat in categoriesRoot.Elements(xNameCategory))
            {
                var catAttr = xcat.Attribute("name");
                if (catAttr == null || string.IsNullOrEmpty(catAttr.Value)) return null;

                var category = new ShopItemCollection(catAttr.Value.Trim());

                foreach (var xitem in xcat.Elements(xNameItem))
                {
                    var idAttr = xitem.Attribute("id");
                    var titleAttr = xitem.Attribute("title");
                    var qualityAttr = xitem.Attribute("quality");
                    var raceAttr = xitem.Attribute("race");
                    var iconAttr = xitem.Attribute("icon");
                    var countAttr = xitem.Attribute("count");
                    var priceAttr = xitem.Attribute("price");

                    if (idAttr == null || string.IsNullOrEmpty(idAttr.Value)) continue;
                    if (titleAttr == null || string.IsNullOrEmpty(titleAttr.Value)) continue;
                    if (priceAttr == null || string.IsNullOrEmpty(priceAttr.Value)) continue;

                    uint id;
                    uint price;
                    uint count;

                    var idTest = uint.TryParse(idAttr.Value, out id);
                    var priceTest = uint.TryParse(priceAttr.Value, out price);
                    
                    if (!idTest || !priceTest) continue;

                    if (countAttr != null)
                    {
                        var countTest = uint.TryParse(countAttr.Value, out count);

                        if (!countTest)
                            count = 1;
                    }
                    else
                        count = 1;

                    var quality = ItemQuality.Common;

                    if (qualityAttr != null &&
                        !string.IsNullOrEmpty(qualityAttr.Value))
                    {
                        ItemQuality temp;

                        var qualityTest = Enum.TryParse(qualityAttr.Value, true, out temp);

                        if (qualityTest)
                            quality = temp;
                    }

                    var race = ItemRaceRestriction.Universal;

                    if (raceAttr != null &&
                        !string.IsNullOrEmpty(raceAttr.Value))
                    {
                        ItemRaceRestriction temp;

                        var raceTest = Enum.TryParse(raceAttr.Value, true, out temp);

                        if (raceTest)
                            race = temp;
                    }

                    string icon = null;

                    if (iconAttr != null &&
                        !string.IsNullOrEmpty(iconAttr.Value))
                        icon = iconAttr.Value.Trim();

                    category.Add(new ShopItem(id, titleAttr.Value.Trim(), quality, race, icon, count, price));
                }

                if (category.Count > 0)
                    result.Add(category);
            }

            return result.Count == 0 ? null : result;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.ShopManager"/> class.
        /// </summary>
        public ShopManager()
        {
            client = new WebClient();
            updateClient = new WebClient();
            path = Path.GetTempFileName();

            client.DownloadFileCompleted += client_DownloadFileCompleted;
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            client.DownloadStringCompleted += client_DownloadStringCompleted;

            updateClient.DownloadStringCompleted += updateClient_DownloadStringCompleted;
        }

        private void updateClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                OnPointsReceived(-1);
                return;
            }

            int points;
            var temp = int.TryParse(e.Result, out points);

            if (!temp)
            {
                OnPointsReceived(-1);
                return;
            }

            OnPointsReceived(points);
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                OnItemBought(ItemBoughtResult.Failure | ItemBoughtResult.ServiceUnavailable);
                return;
            }

            int code;
            var temp = int.TryParse(e.Result, out code);

            if (!temp)
            {
                OnItemBought(ItemBoughtResult.Failure | ItemBoughtResult.ServiceUnavailable);
                return;
            }

            OnItemBought((ItemBoughtResult) code);
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (PriceListLoading != null)
                PriceListLoading(this, new PriceListLoadingEventArgs(e.BytesReceived, e.TotalBytesToReceive));
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                OnPriceListReady(null);
                return;
            }

            var priceList = ReadPriceList(path);
            OnPriceListReady(priceList);
        }
        
        private void OnPriceListReady(GameShop priceList)
        {
            if (PriceListReady != null)
                PriceListReady(this, new PriceListReadyEventArgs(priceList));
        }

        private void OnItemBought(ItemBoughtResult result)
        {
            if (ItemBought != null)
                ItemBought(this, new ItemBoughtEventArgs(result));
        }

        private void OnPointsReceived(int points)
        {
            if (PointsReceived != null)
                PointsReceived(this, new PointsReceivedEventArgs(points));
        }

        /// <summary>
        /// Gets a shop price list asynchronously.
        /// </summary>
        public void GetPriceListAsync()
        {
            try
            {
                client.DownloadFileAsync(new Uri(Resources.NavigationServerPriceUri), path);
            }
            catch (WebException)
            {
                OnPriceListReady(null);
            }
        }

        /// <summary>
        /// Buys a shop item from the user account in the specified count.
        /// </summary>
        /// <param name="account">An account data to use.</param>
        /// <param name="item">A shop item to buy.</param>
        /// <param name="character">A character name to receive a shop item in-game.</param>
        /// <param name="count">An item count to buy.</param>
        /// <exception cref="System.ArgumentNullException"/>
        public void BuyItemAsync(GameAccount account, ShopItem item, string character, int count)
        {
            if (string.IsNullOrEmpty(character))
                throw new ArgumentNullException("character");

            var request = string.Format(Resources.NavigationServerBuyUriFormat,
                account.Name,
                account.GetPasswordHashString(), 
                item.Identifier,
                count,
                character);

            try
            {
                client.DownloadStringAsync(new Uri(request));
            }
            catch (WebException)
            {
                OnItemBought(ItemBoughtResult.Failure | ItemBoughtResult.ServiceUnavailable);
            }
        }

        /// <summary>
        /// Retrieves a count of points for the specified account.
        /// </summary>
        /// <param name="account">An account data.</param>
        public void UpdatePointsAsync(GameAccount account)
        {
            var request = string.Format(Resources.NavigationServerPointsFormat,
                account.Name,
                account.GetPasswordHashString());

            try
            {
                updateClient.DownloadStringAsync(new Uri(request));
            }
            catch (WebException)
            {
                OnPointsReceived(-1);
            }
        }

        /// <summary>
        /// Releases all resources used by this <see cref="Rift.Services.ShopManager"/>.
        /// </summary>
        public void Dispose()
        {
            if (client != null)
            {
                client.DownloadFileCompleted -= client_DownloadFileCompleted;
                client.DownloadProgressChanged -= client_DownloadProgressChanged;
                client.DownloadStringCompleted -= client_DownloadStringCompleted;

                if (client.IsBusy)
                    client.CancelAsync();

                client.Dispose();
            }

            if (updateClient != null)
            {
                updateClient.DownloadStringCompleted -= updateClient_DownloadStringCompleted;

                if (updateClient.IsBusy)
                    updateClient.CancelAsync();

                updateClient.Dispose();
            }
        }
    }
}