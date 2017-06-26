using System.Security.Cryptography;

namespace PaladinsApiPCL.Core.Domain.Utilities
{
    public static class SecurityUtilities
    {
        public static string GetMD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(input);
                bytes = md5.ComputeHash(bytes);
                var sb = new System.Text.StringBuilder();
                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2").ToLower());
                }
                return sb.ToString();
            }
        }
    }
}
