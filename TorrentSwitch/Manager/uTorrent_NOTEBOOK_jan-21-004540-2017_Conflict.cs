using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using TorrentSwitch.torrent_clients;

namespace TorrentSwitch.managers
{
    /// <summary>
    /// Here is uTorrent client managed, sending a torrent magnet link to the client. And checking the status
    /// </summary>
    class UTorrent
    {

        private static string URL { get; set; }
        private CookieAwareWebClient client { get; set; }

        private CookieAwareWebClient initializeWebClient(Settings currentClient)
        {
            CookieAwareWebClient client = new CookieAwareWebClient();
            client.Credentials = new NetworkCredential(currentClient.username, currentClient.password);

            return client;
        }

        private string getToken(Settings currentClient)
        {
            string tokenURL = URL + "token.html";

            StreamReader reader = new StreamReader(client.OpenRead(tokenURL));
            string token = reader.ReadToEnd();
            // Remove HTML tags
            token = Regex.Replace(token, "<.*?>", String.Empty);
            return token;
        }

        private static string getURL(Settings currentClient)
        {
            string URL = "http://" + currentClient.hostname + ":" + currentClient.port + "/gui/";
            return URL;
        }

        

        public static bool SendMagnetURI(Settings currentClient, string magnet)
        {
             a = initializeWebClient();
            ///READ TOKEN
            string tokenUrlAddress = URL + "token.html";

            try
            {
                
                StreamReader reader = new StreamReader(client.OpenRead(tokenUrlAddress));
                string token = reader.ReadToEnd();
                // Remove HTML tags
                token = Regex.Replace(token, "<.*?>", String.Empty);
                // Send magnet URI
                string addUrl = URL + "?action=add-url&s=" + System.Uri.EscapeDataString(magnet) + "&token=" + token;
                client.OpenRead(addUrl);
            }
            catch (Exception)
            {
                return false;
            }
            return true;


        }

        public static bool check_status(Settings currentClient)
        {
            

            string baseUrl = "http://" + currentClient.hostname + ":" + currentClient.port + "/gui/";
            
            ///READ TOKEN
            string tokenUrlAddress = baseUrl + "token.html";

            CookieAwareWebClient client = new CookieAwareWebClient();
            client.Credentials = new NetworkCredential(currentClient.username, currentClient.password);
            Debug.WriteLine(currentClient.hostname);
            try
            {
                client.OpenRead(tokenUrlAddress);
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

    }
}
