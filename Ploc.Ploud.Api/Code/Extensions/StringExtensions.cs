using System;
using System.Security.Cryptography;
using System.Text;

namespace Ploc.Ploud.Api
{
    public static class StringExtensions
    {
        public static string HMac(this string value, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(value);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
