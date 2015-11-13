using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Rift.Data;
using Rift.Properties;

namespace Rift.Services
{
    /// <summary>
    /// Represents an in-game account manager.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class AccountManager
    {
        private readonly AccountDataCollection accounts;
        private int current;

        /// <summary>
        /// Enumerates a collection of <see cref="Rift.Data.GameAccount"/>
        /// that was previously stored.
        /// </summary>
        public IEnumerable<GameAccount> Accounts
        {
            get { return accounts; }
        }

        /// <summary>
        /// Determines whether an application has a stored accounts data.
        /// </summary>
        public bool HasAccounts
        {
            get { return accounts != null && accounts.Count > 0; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.AccountManager"/> class.
        /// </summary>
        public AccountManager()
        {
            current = Settings.Default.SelectedAccount;
            var data = Settings.Default.Accounts;

            if (string.IsNullOrEmpty(data))
            {
                accounts = new AccountDataCollection();
                current = -1;
            }
            else
            {
                var binaryData = Convert.FromBase64String(data);

                using (var stream = new MemoryStream(binaryData))
                {
                    var formatter = new BinaryFormatter();
                    
                    accounts = formatter.Deserialize(stream) as AccountDataCollection ?? new AccountDataCollection();
                }

                if (accounts.Count <= current)
                    current = accounts.Count - 1;
            }
        }

        /// <summary>
        /// Selects an account data by the specified index.
        /// </summary>
        /// <param name="index">
        /// A zero-based index to select an account data.
        /// </param>
        public GameAccount Select(int index)
        {
            if (index < 0)
                index = 0;

            if (index >= accounts.Count)
                index = accounts.Count - 1;

            current = index;

            return accounts[current];
        }

        /// <summary>
        /// Gets a selected account data.
        /// </summary>
        public GameAccount SelectedAccount
        {
            get { return current < accounts.Count && current >= 0 ? accounts[current] : GameAccount.Empty; }
        }

        /// <summary>
        /// Gets an accounts total count.
        /// </summary>
        public int Count
        {
            get { return accounts.Count; }
        }

        /// <summary>
        /// Adds a new account data.
        /// </summary>
        /// <param name="data">
        /// A <see cref="Rift.Data.GameAccount"/> data to add.
        /// </param>
        public void AddAccount(GameAccount data)
        {
            if (data.Equals(GameAccount.Empty))
                return;

            accounts.Add(data);
            current = accounts.Count - 1;
        }

        /// <summary>
        /// Removes a selected account data.
        /// </summary>
        public void RemoveSelected()
        {
            var temp = current;

            if (current >= accounts.Count - 1)
                current--;

            accounts.RemoveAt(temp);
        }

        /// <summary>
        /// Writes the application settings section.
        /// </summary>
        public void WriteSettings()
        {
            Settings.Default.SelectedAccount = current;

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, accounts);

                var binaryData = stream.GetBuffer();
                var data = Convert.ToBase64String(binaryData);

                Settings.Default.Accounts = data;
            }
        }
    }
}