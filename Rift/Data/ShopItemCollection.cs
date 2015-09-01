using System.Collections.Generic;

namespace Rift.Data
{
    /// <summary>
    /// Represents a collection of <see cref="Rift.Data.ShopItem"/> objects.
    /// </summary>
    public sealed class ShopItemCollection : ICollection<ShopItem>
    {
        private readonly List<ShopItem> items;
        private readonly string name;

        /// <summary>
        /// Gets a collection name.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Data.ShopItemCollection"/> class
        /// with the specified name.
        /// </summary>
        public ShopItemCollection(string name)
        {
            items = new List<ShopItem>();
            this.name = name;
        }

        /// <summary>
        /// Determines the index of a specific item in this <see cref="Rift.Data.ShopItemCollection"/>.
        /// </summary>
        /// <param name="item">The <see cref="Rift.Data.ShopItem"/> to locate.</param>
        public int IndexOf(ShopItem item)
        {
            return items.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to this <see cref="Rift.Data.ShopItemCollection"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The <see cref="Rift.Data.ShopItem"/> to insert into this <see cref="Rift.Data.ShopItemCollection"/>.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        public void Insert(int index, ShopItem item)
        {
            items.Insert(index, item);
        }

        /// <summary>
        /// Removes the <see cref="Rift.Data.ShopItem"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }
        
        /// <summary>
        /// Adds an item to this <see cref="Rift.Data.ShopItemCollection"/>.
        /// </summary>
        /// <param name="item">The <see cref="Rift.Data.ShopItem"/> to add.</param>
        public void Add(ShopItem item)
        {
            if (items.Contains(item)) // Item ID.
                return;

            items.Add(item);
        }

        /// <summary>
        /// Removes all items from this <see cref="Rift.Data.ShopItemCollection"/>.
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Determines whether this <see cref="Rift.Data.ShopItemCollection"/> contains a specific
        /// <see cref="Rift.Data.ShopItem"/>.
        /// </summary>
        /// <param name="item">The <see cref="Rift.Data.ShopItem"/> to locate.</param>
        public bool Contains(ShopItem item)
        {
            return items.Contains(item);
        }
        
        /// <summary>
        /// Copies the elements of this <see cref="Rift.Data.ShopItemCollection"/> to an <see cref="System.Array"/>,
        /// starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied
        /// from <see cref="Rift.Data.ShopItemCollection"/>. The <see cref="System.Array"/>
        /// must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        /// <exception cref="System.ArgumentException"/>
        public void CopyTo(ShopItem[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of items contained in this <see cref="Rift.Data.ShopItemCollection"/>.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Rift.Data.ShopItemCollection"/> is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from this <see cref="Rift.Data.ShopItemCollection"/>.
        /// </summary>
        /// <param name="item">a <see cref="Rift.Data.ShopItem"/> to remove.</param>
        public bool Remove(ShopItem item)
        {
            return items.Remove(item);
        }

        /// <summary>
        /// Sorts the collection by the specified <see cref="Rift.Data.ShopItem"/> field.
        /// </summary>
        /// <param name="field">A <see cref="Rift.Data.ShopItem"/> field to sort.</param>
        public void Sort(ShopItemField field)
        {
            items.Sort(new ShopItemComparer(field));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<ShopItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}