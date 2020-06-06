using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Education.Helper
{
    public class Helper
    {
       

        public static string ToGravatarHash(string email)
        {
            var encoder = new UTF8Encoding();
            var md5 = MD5.Create();
            var hashedBytes = md5.ComputeHash(encoder.GetBytes(email.ToLower()));
            var sb = new StringBuilder(hashedBytes.Length * 2);

            for (var i = 0; i < hashedBytes.Length; i++)
                sb.Append(hashedBytes[i].ToString("X2"));

            return sb.ToString().ToLower();
        }


        public static string GetGravatarUrl(string email)
        {
         
            string hash = ToGravatarHash("vincentius.william.setiono@gmail.com");
            string gravatarUrl = string.Format(
              "https://www.gravatar.com/avatar/{0}.jpg&d=identicon$s=40",
              hash);

            return gravatarUrl;
        }
    }
}
