using System.Collections.Generic;
using System.Text;
using Rift.Data;

namespace Rift.Services
{

    /// <summary>
    /// Represents a game process start parameters.
    /// </summary>
    public struct GameProcessParameters
    {
        private readonly string ip;
        private readonly int port;
        private readonly int countryCode;
        private readonly GameAccount account;

        private readonly List<string> flags;

        /// <summary>
        /// Gets an IP-address of the server to connect.
        /// </summary>
        public string IPAddress { get { return ip; } }

        /// <summary>
        /// Gets a server port number to use.
        /// </summary>
        public int Port { get { return port; } }

        /// <summary>
        /// Gets a game client country code.
        /// </summary>
        public int CountryCode { get { return countryCode; } }

        /// <summary>
        /// Gets an account data to login.
        /// </summary>
        public GameAccount Account { get { return account; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rift.Services.GameProcessParameters"/> structure
        /// with the specified server and the user account information.
        /// </summary>
        /// <param name="ip">An IP-address of the server to connect.</param>
        /// <param name="port">A server port number to use.</param>
        /// <param name="countryCode">A game client country code.</param>
        /// <param name="account">An account data to login.</param>
        public GameProcessParameters(
            string ip,
            int port,
            int countryCode,
            GameAccount account = default(GameAccount))
        {
            this.ip = ip;
            this.port = port;
            this.countryCode = countryCode;
            this.account = account;

            flags = new List<string>();
        }

        /// <summary>
        /// Inserts the specified flags into the game process parameters.
        /// </summary>
        /// <param name="args">An array of flags to add.</param>
        public void InsertFlags(params string[] args)
        {
            flags.AddRange(args);
        }

        /// <summary>
        /// Returns the string representation for this <see cref="Rift.Services.GameProcessParameters"/>.
        /// </summary>
        public override string ToString()
        {
            var temp = new StringBuilder();

            temp.AppendFormat("-ip:{0} -port:{1} -cc:{2}", ip, port, countryCode);

            if (account != null && !account.Equals(GameAccount.Empty))
                temp.AppendFormat(" -account:{0} -password:{1}", account.Name, account.Password);

            foreach (var flag in flags)
                temp.Append(' ').Append(flag);

            return temp.ToString();
        }
    }

}