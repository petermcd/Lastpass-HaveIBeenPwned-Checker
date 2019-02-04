using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;

namespace Lastpass_HaveIBeenPwned_Checker
{
    class Checker
    {

        const string API_URL = "https://api.pwnedpasswords.com/range/";
        public void CheckSite(Site site)
        {
            string convertedPassword = this.ConvertToSha1(site.Password);
            if (site.Password == "") {
                Console.WriteLine(site.Name + " not checked");
                return;
            }
            int[] result = this.CallApi(convertedPassword);
            site.Matched = result[0] == 1;
            site.Count = result[1];
        }
        private int[] CallApi(string password)
        {
            string substringPassword = password.Substring(0, Math.Min(password.Length, 5));
            WebClient client = new WebClient();
            byte[] content = client.DownloadData(Checker.API_URL + substringPassword);
            client.Dispose();
            return this.CheckResponse(Encoding.UTF8.GetString(content), substringPassword, password);
        }
        private int[] CheckResponse(string requestResponse, string passwordSuffix, string password)
        {
            int[] response = { 0, 0 };
            string[] lines = requestResponse.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Count(); i++)
            {
                string[] lineDetails = lines[i].Split(':');
                if (passwordSuffix + lineDetails[0].ToUpper() == password)
                {
                    int.TryParse(lineDetails[1], out int count);
                    response[0] = 1;
                    response[1] = count;
                    return response;
                }
            }
            return response;
        }
        private string ConvertToSha1(string password)
        {
            using(SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    builder.Append(b.ToString("X2"));
                }
                return builder.ToString().ToUpper();
            }
        }
    }
}
