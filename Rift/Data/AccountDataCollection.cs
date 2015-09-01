using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Rift.Data
{
    /// <summary>
    /// Represents the collection of the user accounts.
    /// This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class AccountDataCollection : ICollection<GameAccount>, ISerializable
    {
        private readonly List<GameAccount> accounts;

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Data.AccountDataCollection"/> class.
        /// </summary>
        public AccountDataCollection()
        {
            accounts = new List<GameAccount>();
        }

        private AccountDataCollection(SerializationInfo info, StreamingContext context)
        {
            accounts = info.GetValue("accounts", typeof (List<GameAccount>)) as List<GameAccount>;
            accounts = accounts ?? new List<GameAccount>();
        }

        /// <summary>
        /// Gets or sets an account data at the specified index
        /// in this <see cref="Rift.Data.AccountDataCollection"/>.
        /// </summary>
        /// <param name="index">
        /// A zero-based index to locate an account data in this
        /// <see cref="Rift.Data.AccountDataCollection"/>.
        /// </param>
        public GameAccount this[int index]
        {
            get { return accounts[index]; }
            set { accounts[index] = value; }
        }

        /// <summary>
        /// Adds an account data to this <see cref="Rift.Data.AccountDataCollection"/>.
        /// </summary>
        /// <param name="data">
        /// An account data to add to the <see cref="Rift.Data.AccountDataCollection"/>.
        /// </param>
        public void Add(GameAccount data)
        {
            if (accounts.Contains(data)) return;

            accounts.Add(data);
        }
        
        /// <summary>
        /// Removes all accounts data from this <see cref="Rift.Data.AccountDataCollection"/>.
        /// </summary>
        public void Clear()
        {
            accounts.Clear();
        }
        
        /// <summary>
        /// Determines whether this <see cref="Rift.Data.AccountDataCollection"/> contains a specific account data.
        /// </summary>
        /// <param name="data">
        /// An account data to locate in this <see cref="Rift.Data.AccountDataCollection"/>.
        /// </param>
        public bool Contains(GameAccount data)
        {
            return accounts.Contains(data);
        }
        
        /// <summary>
        /// Determines whether this <see cref="Rift.Data.AccountDataCollection"/> contains an account data
        /// with the specified name.
        /// </summary>
        /// <param name="accountName">
        /// An account name to locate in this <see cref="Rift.Data.AccountDataCollection"/>.
        /// </param>
        public bool Contains(string accountName)
        {
            return accounts.Any(
                o => string.Equals(
                    o.Name, 
                    accountName, 
                    StringComparison.Ordinal));
        }
        
        /// <summary>
        /// Copies the accounts data of this <see cref="Rift.Data.AccountDataCollection"/>
        /// to an <see cref="System.Array"/>, starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array"/> that is the destination of the accounts data
        /// copied from this <see cref="Rift.Data.AccountDataCollection"/>. The <see cref="System.Array"/>
        /// must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        /// <exception cref="System.ArgumentException"/>
        public void CopyTo(GameAccount[] array, int arrayIndex)
        {
            accounts.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of accounts data contained in this <see cref="Rift.Data.AccountDataCollection"/>.
        /// </summary>
        public int Count
        {
            get { return accounts.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Rift.Data.AccountDataCollection"/> is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specified account data from this <see cref="Rift.Data.AccountDataCollection"/>.
        /// </summary>
        /// <param name="data">
        /// An account data to remove from this <see cref="Rift.Data.AccountDataCollection"/>.
        /// </param>
        public bool Remove(GameAccount data)
        {
            return accounts.Remove(data);
        }

        /// <summary>
        /// Removes the first occurrence of an account data with the specified name
        /// from this <see cref="Rift.Data.AccountDataCollection"/>.
        /// </summary>
        /// <param name="accountName"> 
        /// An account name to remove from this <see cref="Rift.Data.AccountDataCollection"/>.
        /// </param>
        public bool Remove(string accountName)
        {
            var temp = accounts.FirstOrDefault(
                o => string.Equals(
                    o.Name, 
                    accountName, 
                    StringComparison.Ordinal));

            return temp != GameAccount.Empty && accounts.Remove(temp);
        }

        /// <summary>
        /// Removes an account data at the specified index.
        /// </summary>
        /// <param name="index">
        /// A zero-based index of an account to remove from this <see cref="Rift.Data.AccountDataCollection"/>.
        /// </param>
        public void RemoveAt(int index)
        {
            if (accounts.Count > 0 &&
                index < accounts.Count)
                accounts.RemoveAt(index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<GameAccount> GetEnumerator()
        {
            return accounts.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/> with the data
        /// needed to serialize this <see cref="Rift.Data.AccountDataCollection"/>.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> to populate with data.
        /// </param>
        /// <param name="context">
        /// The destination (see <see cref="System.Runtime.Serialization.StreamingContext"/>) for this serialization.
        /// </param>
        /// <exception cref="System.Security.SecurityException"/>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("accounts", accounts, typeof(List<GameAccount>));
        }
    }
}