//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PasswordHasher.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Identity
{
    using System.Security.Cryptography;
    using System.Text;

    using Microsoft.AspNet.Identity;

    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return GetMd5Hash(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return hashedPassword == this.HashPassword(providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }

        public static string GetMd5Hash(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(input);
            byte[] result = md5.ComputeHash(textToHash);

            return System.BitConverter.ToString(result).Replace("-", string.Empty);
        }

        //public string CalculateMD5Hash(string input)
        //{
        //    // step 1, calculate MD5 hash from input
        //    MD5 md5 = System.Security.Cryptography.MD5.Create();
        //    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        //    byte[] hash = md5.ComputeHash(inputBytes);

        //    // step 2, convert byte array to hex string
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < hash.Length; i++)
        //    {
        //        sb.Append(hash[i].ToString("X2"));
        //    }
        //    return sb.ToString();
        //}
    }
}