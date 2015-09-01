using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace Shugo
{
    /// <summary>
    /// Represents a single input item to discover.
    /// </summary>
    public class InputItem
    {
        /// <summary>
        /// Gets or sets an item identifier.
        /// </summary>
        public uint Identifier { get; set; }
        /// <summary>
        /// Gets or sets an item package count.
        /// </summary>
        public uint Count { get; set; }
        /// <summary>
        /// Gets or sets an item package price.
        /// </summary>
        public uint Price { get; set; }
    }

    public class GrabData
    {
        public string Icon { get; set; }

        public string Title { get; set; }

        public string Quality { get; set; }

        public string CategoryName { get; set; }

        public int Race { get; set; }
    }

    /// <summary>
    /// Represents a 'Shugo' application.
    /// </summary>
    public static class App
    {
        private const string DataBaseFormat = @"http://aiondatabase.net/tip.php?id=item--{0}&l=ru&nf=on";
        
        private static readonly Regex splitRegex = new Regex(@"\s+");
        private static readonly ICollection<InputItem> input = new List<InputItem>();
        
        private static readonly Dictionary<string, ShopItemCollection> categories = new Dictionary<string, ShopItemCollection>();

        private static volatile int itemCount = 0;

        // The main entry point.
        private static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.WriteLine("Shugo Trader (http://aiondatabase.net)");
            Console.WriteLine("Copyright (C) 2015 AtreiaWorld.com\n");

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Shugo usage: Shugo.exe file1 [file2 ... fileN]");
                Console.WriteLine("This tool is not working without special input data.");
                return;
            }

            Parse(args);
            Console.WriteLine("Shugo is ready to grab the data.");
            Console.WriteLine("Press ENTER to continue, or press ESC to exit...");

            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);
            } while (keyInfo.Key != ConsoleKey.Enter &&
                     keyInfo.Key != ConsoleKey.Escape);

            if (keyInfo.Key == ConsoleKey.Escape) return;
            
            var defaultCategory = new ShopItemCollection(@"Разное");
            categories.Add(defaultCategory.Name, defaultCategory);

            itemCount = 0;

            Console.WriteLine("\nDownloading data:");
            Parallel.ForEach(input, Run);

            var uid = new List<uint>();
            
            var ic = 0;
            var cc = 0;

            foreach (var category in categories.Values)
            {
                cc += category.Count == 0 ? 0 : 1;
                ic += category.Count;
            }

            Console.WriteLine("DONE!");
            Console.WriteLine("Categories: {0}.", cc);
            Console.WriteLine("Items: {0}.", ic);

            Console.WriteLine("Press any key to save XML file and exit...");
            Console.ReadKey(true);

            if (uid.Count > 0)
            {
                using (var file = new StreamWriter("error.txt"))
                {
                    foreach (var id in uid)
                        file.WriteLine("{0}", id);
                }
            }

            SaveResult();
        }

        private static void Run(InputItem item)
        {
            var id = item.Identifier;
            var count = item.Count;
            var price = item.Price;

            var data = GrabById(id);

            if (data == null)
            {
                return;
            }

            var title = data.Title.Trim();
            var quality = (ItemQuality) Enum.Parse(typeof(ItemQuality), data.Quality, true);
            var icon = data.Icon.Replace(@"/items/", string.Empty).Replace(@".png", string.Empty).Trim();
            var race = (ItemRaceRestriction) data.Race;

            var itemData = new ShopItem(id, title, quality, race, icon, count, price);

            if (string.IsNullOrEmpty(data.CategoryName))
            {
                categories[@"Разное"].Add(itemData);
                return;
            }

            if (categories.ContainsKey(data.CategoryName))
            {
                categories[data.CategoryName].Add(itemData);
                return;
            }

            var newCategory = new ShopItemCollection(data.CategoryName) { itemData };
            categories.Add(newCategory.Name, newCategory);
        }

        private static GrabData GrabById(uint id)
        {
            var client = new ShugoWebClient() {Encoding = Encoding.UTF8};
            var doc = new HtmlDocument();

            var uri = string.Format(DataBaseFormat, id);
            try
            {
                var tipHtml = client.DownloadString(uri);
                doc.LoadHtml(tipHtml);
            }
            catch (WebException e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                client.Dispose();
                return null;
            }

            client.Dispose();
            itemCount++;

            Console.WriteLine("Processing: {0}", itemCount);

            var titleNode = doc.DocumentNode.SelectSingleNode("//span[@class='item_title']");
            if (titleNode == null) return null;

            var categoryNode = doc.DocumentNode.SelectSingleNode("//table[@class='item_stats_table']//tr//td");
            
            var category = string.Empty;
            var race = 0;
            
            if (categoryNode != null)
            {
                var innerHtml = categoryNode.InnerHtml;

#if DEBUG
                Debug.WriteLine(innerHtml);
#endif

                if (!string.IsNullOrEmpty(innerHtml))
                {
                    var splitted = innerHtml.Split('<');

                    category = splitted[0].Trim();

#if DEBUG
                    Debug.WriteLine(category);
#endif

                    if (innerHtml.Contains("Только для эли"))
                        race = 2;
                    else if (innerHtml.Contains("Только для асмо"))
                        race = 1;
                }
            }

            var iconBaseNode = doc.DocumentNode.SelectSingleNode("//td[@class='item-icon']");
            if (iconBaseNode == null) return null;

            var icon = iconBaseNode.FirstChild.GetAttributeValue("src", string.Empty);
            if (string.IsNullOrEmpty(icon)) return null;

            var qualityRaw = titleNode.ParentNode.GetAttributeValue("class", string.Empty);
            if (string.IsNullOrEmpty(qualityRaw)) return null;

            qualityRaw = qualityRaw.Trim().Replace("quality-", string.Empty);

            return new GrabData
            {
                CategoryName = category,
                Icon = icon,
                Quality = qualityRaw,
                Title = titleNode.FirstChild.InnerText,
                Race = race
            };
        }

        private static void Parse(IEnumerable<string> args)
        {
            foreach (var file in args)
            {
                var fileInfo = new FileInfo(file);

                if (!fileInfo.Exists)
                {
                    Console.WriteLine("Shugo cannot find '{0}'", file);
                    continue; // Skip this shit.
                }

                var totalItems = 0;

                using (var reader = new StreamReader(fileInfo.OpenRead()))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrEmpty(line)) continue;

                        var tokens = splitRegex.Split(line);
                        if (tokens.Length != 3) continue;

                        uint id, count, price;

                        var testId = uint.TryParse(tokens[0], out id);
                        var testCount = uint.TryParse(tokens[1], out count);
                        var testPrice = uint.TryParse(tokens[2], out price);

                        if (!testId |
                            !testPrice |
                            !testCount) continue;

                        input.Add(new InputItem {Identifier = id, Count = count, Price = price});
                        totalItems++;
                    }
                }

                Console.WriteLine("Shugo parsed {0} items from '{1}'", totalItems, file);
            }
        }

        private static void SaveResult()
        {
            var root = new XElement(XName.Get("shop", @"http://atreiaworld.com/ings-1.0.xsd"));

            foreach (var category in categories.Values)
            {
                if (category.Count == 0) continue;

                var categoryRoot = new XElement(XName.Get("category", @"http://atreiaworld.com/ings-1.0.xsd"), new XAttribute("name", category.Name));

                foreach (var item in category)
                {
                    var itemXml = new XElement(XName.Get("item", @"http://atreiaworld.com/ings-1.0.xsd"),
                        new XAttribute("id", item.Identifier.ToString("D")),
                        new XAttribute("price", item.Price.ToString("D")),
                        new XAttribute("title", item.Title),
                        new XAttribute("icon", item.IconUri),
                        new XAttribute("quality", (int) item.Quality));

                    if (item.Count > 1)
                        itemXml.Add(new XAttribute("count", item.Count.ToString("D")));

                    if (item.Restriction != ItemRaceRestriction.Universal)
                        itemXml.Add(new XAttribute("race", (int) item.Restriction));

                    categoryRoot.Add(itemXml);
                }

                root.Add(categoryRoot);
            }

            root.Save("out.xml");
        }
    }
}