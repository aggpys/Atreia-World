using System;
using System.Collections;
using System.Collections.Generic;

namespace Rift.Data
{
    /// <summary>
    /// Specifies an item quality.
    /// </summary>
    public enum ItemQuality
    {
        /// <summary>
        /// Junk quality (grey).
        /// </summary>
        Junk,
        /// <summary>
        /// Common quality (white).
        /// </summary>
        Common,
        /// <summary>
        /// Rare quality (green).
        /// </summary>
        Rare,
        /// <summary>
        /// Legend quality (blue).
        /// </summary>
        Legend,
        /// <summary>
        /// Unique quality (yellow).
        /// </summary>
        Unique,
        /// <summary>
        /// Epic quality (orange).
        /// </summary>
        Epic,
        /// <summary>
        /// Mythic quality (purple).
        /// </summary>
        Mythic
    }

    /// <summary>
    /// Specifies an item race restrictions.
    /// </summary>
    public enum ItemRaceRestriction
    {
        /// <summary>
        /// Not specified (universal item).
        /// </summary>
        Universal = 0,
        /// <summary>
        /// Asmodians only.
        /// </summary>
        Asmodians = 1,
        /// <summary>
        /// Elyos only.
        /// </summary>
        Elyos = 2
    }

    /// <summary>
    /// Specifies a <see cref="Rift.Data.ShopItem"/> object fields.
    /// </summary>
    public enum ShopItemField
    {
        /// <summary>
        /// Identifier field.
        /// </summary>
        Identifier,
        /// <summary>
        /// Title field.
        /// </summary>
        Title,
        /// <summary>
        /// Quality field.
        /// </summary>
        Quality,
        /// <summary>
        /// Prices field.
        /// </summary>
        Price
    }

    /// <summary>
    /// Represents a single shop item.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class ShopItem : IEquatable<ShopItem>, IComparable<ShopItem>, IComparable
    {
        private readonly uint id;
        private readonly string title;
        private readonly ItemQuality quality;
        private readonly ItemRaceRestriction restriction;
        private readonly string iconUri;
        private readonly uint count;
        private readonly uint price;

        /// <summary>
        /// Gets an unique item in-game identifier.
        /// </summary>
        public uint Identifier { get { return id; } }

        /// <summary>
        /// Gets an item title (name).
        /// </summary>
        public string Title { get { return title; } }

        /// <summary>
        /// Gets an item quality.
        /// </summary>
        public ItemQuality Quality { get { return quality; } }

        /// <summary>
        /// Gets an item race restriction type.
        /// </summary>
        public ItemRaceRestriction Restriction { get { return restriction; } }

        /// <summary>
        /// Gets an icon URI associated with this shop item.
        /// </summary>
        public string IconUri { get { return iconUri; } }

        /// <summary>
        /// Gets a base item count.
        /// </summary>
        public uint Count { get { return count; } }

        /// <summary>
        /// Gets an item price.
        /// </summary>
        public uint Price { get { return price; } }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Data.ShopItem"/> class
        /// from the specified information about a shop item.
        /// </summary>
        /// <param name="id">An unique item in-game identifier.</param>
        /// <param name="title">An item title (name).</param>
        /// <param name="quality">An item quality.</param>
        /// <param name="restriction">An item race restriction.</param>
        /// <param name="iconUri">An icon URI associated with this shop item.</param>
        /// <param name="count">An items base count.</param>
        /// <param name="price">An item price.</param>
        public ShopItem(uint id, string title, ItemQuality quality, ItemRaceRestriction restriction, string iconUri, uint count, uint price)
        {
            this.id = id;
            this.title = title;
            this.quality = quality;
            this.restriction = restriction;
            this.iconUri = iconUri;
            this.count = count;
            this.price = price;
        }

        /// <summary>
        /// Returns the hash code for this <see cref="Rift.Data.ShopItem"/>.
        /// </summary>
        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        /// <summary>
        /// Returns the string representation for this <see cref="Rift.Data.ShopItem"/>.
        /// </summary>
        public override string ToString()
        {
            var name = GetType().Name;

            if (string.IsNullOrEmpty(title))
                return string.Format("[{0}]", name);

            return string.Format("[{0} '{1}']", name, title);
        }

        /// <summary>
        /// Indicates whether this <see cref="Rift.Data.ShopItem"/> is equal to another object.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        public override bool Equals(object obj)
        {
            var temp = obj as ShopItem;

            return temp != null && Equals(temp);
        }

        /// <summary>
        /// Indicates whether this <see cref="Rift.Data.ShopItem"/> is equal to another object of the same type.
        /// </summary>
        /// <param name="other">Another <see cref="Rift.Data.ShopItem"/> to compare to.</param>
        public bool Equals(ShopItem other)
        {
            return id == other.id;
        }

        /// <summary>
        /// Compares this <see cref="Rift.Data.ShopItem"/> with another object.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        public int CompareTo(object obj)
        {
            var temp = obj as ShopItem;

            if (temp != null)
                return CompareTo(temp);

            return -1;
        }

        /// <summary>
        /// Compares this <see cref="Rift.Data.ShopItem"/> with another object of the same type.
        /// </summary>
        /// <param name="other">Another <see cref="Rift.Data.ShopItem"/> to compare to.</param>
        public int CompareTo(ShopItem other)
        {
            return id.CompareTo(other.id);
        }
    }

    /// <summary>
    /// Represents the <see cref="Rift.Data.ShopItem"/> comparer.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class ShopItemComparer : IComparer<ShopItem>, IComparer
    {
        private readonly ShopItemField field;

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Data.ShopItemComparer"/> class
        /// from the specified <see cref="Rift.Data.ShopItem"/> object field.
        /// </summary>
        /// <param name="field">
        /// A <see cref="Rift.Data.ShopItem"/> object field that will be used
        /// in the comparsion.
        /// </param>
        public ShopItemComparer(ShopItemField field)
        {
            this.field = field;
        }

        /// <summary>
        /// Compares two <see cref="Rift.Data.ShopItem"/> and returns a value indicating
        /// whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first <see cref="Rift.Data.ShopItem"/> to compare.</param>
        /// <param name="y">The second <see cref="Rift.Data.ShopItem"/> to compare.</param>
        public int Compare(ShopItem x, ShopItem y)
        {
            switch (field)
            {
                case ShopItemField.Title:
                    return string.Compare(x.Title, y.Title, StringComparison.OrdinalIgnoreCase);
                case ShopItemField.Quality:
                    return x.Quality.CompareTo(y.Quality);
                case ShopItemField.Price:
                    return x.Price.CompareTo(y.Price);
                default:
                    return x.CompareTo(y); // Default comparsion (by ID).
            }
        }

        /// <summary>
        /// Compares two objects and returns a value indicating
        /// whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        public int Compare(object x, object y)
        {
            var tempX = x as ShopItem;
            var tempY = y as ShopItem;

            if (tempX != null && tempY != null)
                return Compare(tempX, tempY);

            if (tempX != null)
                return -1;

            return tempY != null ? 1 : 0;
        }
    }
}