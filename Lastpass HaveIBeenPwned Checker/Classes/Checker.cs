using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Globalization;

namespace Lastpass_HaveIBeenPwned_Checker
{
    class Checker
    {

        public const string API_URL = "https://api.pwnedpasswords.com/range/";
        public void CheckSite(Site site)
        {
            site.Processed = true;
            if (!site.HasPassword())
            {
                return;
            }
            int[] result = this.CallApi(site);
            site.Matched = result[0] == 1;
            site.Count = result[1];
        }
        private int[] CallApi(Site site)
        {
            WebClient client = new WebClient();
            byte[] content = client.DownloadData(Checker.API_URL + site.Sha1PasswordShortened);
            client.Dispose();
            return Checker.CheckResponse(Encoding.UTF8.GetString(content), site);
        }
        private static int[] CheckResponse(string requestResponse, Site site)
        {
            int[] response = { 0, 0 };
            site.Responses = requestResponse.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            for (int i = 0; i < site.Responses.Count(); i++)
            {
                string[] lineDetails = site.Responses[i].Split(':');
                if (site.Sha1PasswordShortened + lineDetails[0].ToUpper(CultureInfo.InvariantCulture) == site.Sha1Password)
                {
                    int.TryParse(lineDetails[1], out int count);
                    response[0] = 1;
                    response[1] = count;
                    return response;
                }
            }
            return response;
        }
    }
}
