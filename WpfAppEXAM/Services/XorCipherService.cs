using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppEXAM.Services
{
    public class XorChipperService
    {
        private readonly static string DefaultKey = "KEY";

        private string GetRepeatKey(string s, int n)
        {
            var r = s;
            while (r.Length < n)
            {
                r += r;
            }

            return r.Substring(0, n);
        }

        private string Cipher(string text, string secretKey)
        {
            var currentKey = GetRepeatKey(secretKey, text.Length);
            var res = string.Empty;
            for (var i = 0; i < text.Length; i++)
            {
                res += ((char)(text[i] ^ currentKey[i])).ToString();
            }

            return res;
        }

        public string Encrypt(string plainText, string password = null)
        {

            var key = string.IsNullOrEmpty(password) ? DefaultKey : password;
            return Cipher(plainText, key);
        }

        public string Decrypt(string encryptedText, string password = null)
        {

            var key = string.IsNullOrEmpty(password) ? DefaultKey : password;
            return Cipher(encryptedText, key);
        }
    }
}
