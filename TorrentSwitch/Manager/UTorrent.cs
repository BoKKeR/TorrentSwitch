using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using TorrentSwitch.torrentClients;

namespace TorrentSwitch.managers
{
    /// <summary>
    /// Here is uTorrent client managed, sending a torrent magnet link to the client. And checking the status
    /// </summary>
    class UTorrent
    {

        private static CookieAwareWebClient webclient { get; set; }
        private static string URL { get; set; }
        private static string baseUrl(Settings currentClient)
        
        {
            URL = "http://" + currentClient.hostname + ":" + currentClient.port + "/gui/";
            return URL;
        }

        private static void initializeWebClient(Settings currentClient)
        {
            webclient = new CookieAwareWebClient();
            webclient.Credentials = new NetworkCredential(currentClient.username, currentClient.password);
        }

        private static string getToken(Settings currentClient)
        {
            initializeWebClient(currentClient);
            StreamReader reader = new StreamReader(webclient.OpenRead(URL + "token.html"));
            string token = reader.ReadToEnd();

            token = Regex.Replace(token, "<.*?>", String.Empty);
            token = "&token=" + token;
            return token;
        }
        public bool SendMagnetURI(Settings currentClient, string magnet)
        {
            baseUrl(currentClient);

            try
            {
            ///READ TOKEN
            string token = getToken(currentClient);

                ///SEND MAGNET LINK
                string addUrl = URL + "?action=add-url&s=" + System.Uri.EscapeDataString(magnet) + token;
                webclient.OpenRead(addUrl);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool CheckStatus(Settings currentClient)
        {

            baseUrl(currentClient);

            try
            {
                ///READ TOKEN
                string token = getToken(currentClient);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
