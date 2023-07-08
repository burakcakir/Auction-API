using System;
using System.Security.Cryptography;
using System.Text;

namespace Auction_API.Infrastructure
{
	public class Utility
	{
        public static string ComputeSHA512(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha512.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // Şifrelenmiş metni hex formatına dönüştürme
                }

                return builder.ToString();
            }
        }
    }
}

