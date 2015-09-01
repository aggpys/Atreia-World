using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace Rift.Data
{
    /// <summary>
    /// Represents an in-game user account.
    /// This class cannot be inherited.
    /// </summary>
    [Serializable]
    public sealed class GameAccount : IEquatable<GameAccount>, ISerializable
    {
        /// <summary>
        /// Gets an empty <see cref="Rift.Data.GameAccount"/> without a name and password.
        /// </summary>
        public static readonly GameAccount Empty = default (GameAccount);

        /// <summary>
        /// Gets an account name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets an account password.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rift.Data.GameAccount"/> structure
        /// from the specified account name and password or it's hash.
        /// </summary>
        /// <param name="name">An account name.</param>
        /// <param name="password">An account password or it's hash.</param>
        public GameAccount(string name, string password)
        {
            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(password))
            {
                Name = null;
                Password = null;

                return;
            }

            Name = name;
            Password = password;
        }

        private GameAccount(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("name");
            Password = info.GetString("password");
        }

        /// <summary>
        /// Returns the string representation for this <see cref="Rift.Data.GameAccount"/>.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Returns the hash code for this <see cref="Rift.Data.GameAccount"/>.
        /// </summary>
        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode();
        }

        /// <summary>
        /// Returns the hash string for the account password.
        /// </summary>
        public string GetPasswordHashString()
        {
            string hash;

            using (var hashGen = new MD5CryptoServiceProvider())
            {
                var buffer = Encoding.UTF8.GetBytes(Password);
                var temp = hashGen.ComputeHash(buffer);

                hash = Encoding.UTF8.GetString(temp).Replace("-", string.Empty);
            }

            return hash;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj"> Another object to compare to.</param>
        public override bool Equals(object obj)
        {
            var account = obj as GameAccount;

            return account != null && Equals(account);
        }

        /// <summary>
        /// Indicates whether this instance is equal to another <see cref="Rift.Data.GameAccount"/>.
        /// </summary>
        /// <param name="data">
        /// Another <see cref="Rift.Data.GameAccount"/> to compare with this instance.
        /// </param>
        public bool Equals(GameAccount data)
        {
            return data != null && string.Equals(data.Name, Name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Indicates whether this instance is equal to another <see cref="Rift.Data.GameAccount"/>.
        /// </summary>
        /// <param name="data">
        /// Another <see cref="Rift.Data.GameAccount"/> to compare with this instance.
        /// </param>
        bool IEquatable<GameAccount>.Equals(GameAccount data)
        {
            return Equals(data);
        }
        
        /// <summary>
        /// Populates a <see cref="System.Runtime.Serialization.SerializationInfo"/> with the data
        /// needed to serialize this <see cref="Rift.Data.GameAccount"/>.
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
            info.AddValue("name", Name);
            info.AddValue("password", Password);
        }
    }
}