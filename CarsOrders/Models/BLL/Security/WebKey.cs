using System;
using System.Security.Cryptography;
using System.Text;

namespace CarsOrders.Models.BLL
{
    public class WebKey : Interfaces.IWebKey
    {
        #region Properties

        public string KeyHash { get; set; }

        public string Salt { get; set; }

        #endregion

        #region Public Functions

        public bool IsKeyValid(string key)
        {
            return GetHash(key ?? string.Empty, Salt) == KeyHash;
        }

        public static string GetHash(string key, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        #endregion
    }
}
