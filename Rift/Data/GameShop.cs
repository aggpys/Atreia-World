using System.Collections.Generic;

namespace Rift.Data
{
    /// <summary>
    /// Represents an in-game shop data.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class GameShop : ICollection<ShopItemCollection>
    {
        private readonly Dictionary<string, ShopItemCollection> categories;

        /// <summary>
        /// Gets a set of category names.
        /// </summary>
        public IEnumerable<string> CategoryNames
        {
            get
            {
                foreach (var category in categories.Values)
                    yield return category.Name;
            }
        }

        /// <summary>
        /// Gets a collection of items.
        /// </summary>
        public IEnumerable<ShopItem> Items
        {
            get
            {
                foreach (var category in categories.Values)
                    foreach (var item in category)
                        yield return item;
            }
        } 

        /// <summary>
        /// Gets a shop items category by the specified category name.
        /// </summary>
        /// <param name="name">A name of the category to select.</param>
        public ShopItemCollection this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name) ||
                    !categories.ContainsKey(name))
                    return null;

                return categories[name];
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Data.GameShop"/> class.
        /// </summary>
        public GameShop()
        {
            categories = new Dictionary<string, ShopItemCollection>();
        }

        /// <summary>
        /// Adds the specified <see cref="Rift.Data.ShopItemCollection"/> to the game shop.
        /// </summary>
        /// <param name="category">A <see cref="Rift.Data.ShopItemCollection"/> to add.</param>
        public void Add(ShopItemCollection category)
        {
            if (category == null || category.Count == 0) return;

            if (categories.ContainsKey(category.Name))
            {
                Expand(category);
                return;
            }

            categories.Add(category.Name, category);
        }

        /// <summary>
        /// Expands the existing shop category (if it presented) with the specified
        /// in-game shop items.
        /// </summary>
        /// <param name="category">>A <see cref="Rift.Data.ShopItemCollection"/> to expand.</param>
        public void Expand(ShopItemCollection category)
        {
            if (category == null || category.Count == 0) return;

            if (!categories.ContainsKey(category.Name))
                categories.Add(category.Name, category);

            foreach (var item in category)
                categories[category.Name].Add(item);
        }

        /// <summary>
        /// Removes all shop items and categories from the in-game shop.
        /// </summary>
        public void Clear()
        {
            foreach (var category in categories.Values)
                category.Clear();

            categories.Clear();
        }

        /// <summary>
        /// Determines whether this game shop contains the specified
        /// <see cref="Rift.Data.ShopItemCollection"/>.
        /// </summary>
        /// <param name="category">A <see cref="Rift.Data.ShopItemCollection"/> to locate.</param>
        public bool Contains(ShopItemCollection category)
        {
            return category != null && Contains(category.Name);
        }

        /// <summary>
        /// Determines whether this game shop contains the category
        /// with the specified name.
        /// </summary>
        /// <param name="categoryName">A category name to locate.</param>
        public bool Contains(string categoryName)
        {
            return categories.ContainsKey(categoryName);
        }

        /// <summary>
        /// Copies the elements of this <see cref="Rift.Data.ShopItemCollection"/> to an <see cref="System.Array"/>, 
        /// starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from
        /// <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must
        /// have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentException"/>
        public void CopyTo(ShopItemCollection[] array, int arrayIndex)
        {
            categories.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets a total amount of categories
        /// </summary>
        public int Count
        {
            get { return categories.Count; }
        }

        /// <summary>
        /// Gets a total amount of shop items.
        /// </summary>
        public int ItemsCount
        {
            get
            {
                var temp = 0;

                foreach (var category in categories)
                    temp += category.Value.Count;

                return temp;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified <see cref="Rift.Data.ShopItemCollection"/>.
        /// </summary>
        /// <param name="category">A <see cref="Rift.Data.ShopItemCollection"/> to remove.</param>
        /// <returns></returns>
        public bool Remove(ShopItemCollection category)
        {
            return category != null && categories.Remove(category.Name);
        }

        /// <summary>
        /// Removes the contained <see cref="Rift.Data.ShopItemCollection"/>
        /// with the specified name.
        /// </summary>
        /// <param name="categoryName">A category name to remove.</param>
        public bool Remove(string categoryName)
        {
            return !string.IsNullOrEmpty(categoryName) && categories.Remove(categoryName);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<ShopItemCollection> GetEnumerator()
        {
            return categories.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return categories.Values.GetEnumerator();
        }
    }
}