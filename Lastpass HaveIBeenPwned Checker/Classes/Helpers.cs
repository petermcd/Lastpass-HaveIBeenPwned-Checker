using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Lastpass_HaveIBeenPwned_Checker
{
    class Helpers
    {
        protected Helpers(){}
        public static string ConvertToSha1(string password)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    builder.Append(b.ToString("X2"));
                }
                return builder.ToString().ToUpper(CultureInfo.InvariantCulture);
            }
        }
    }
}
