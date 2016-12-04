using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using TorrentSwitch.torrent_clients;

namespace TorrentSwitch.managers
{
    /// <summary>
    /// Here is uTorrent client managed, sending a torrent magnet link to the client. And checking the status
    /// </summary>
    class uTorrent
    {
        public static bool send_magnet_uri(Settings currentClient, string magnet)
        {
            string baseUrl = "http://" + currentClient.hostname + ":" + currentClient.port + "/gui/";

            ///READ TOKEN
            string tokenUrlAddress = baseUrl + "token.html";

            CookieAwareWebClient client = new CookieAwareWebClient();
            client.Credentials = new NetworkCredential(currentClient.username, currentClient.password);

            try
            {
                
                StreamReader reader = new StreamReader(client.OpenRead(tokenUrlAddress));
                string token = reader.ReadToEnd();

                token = Regex.Replace(token, "<.*?>", String.Empty);
                ///SEND MAGNET LINK
                string addUrl = baseUrl + "?action=add-url&s=" + System.Uri.EscapeDataString(magnet) + "&token=" + token;
                client.OpenRead(addUrl);
            }
            catch (Exception)
            {
                return false;
            }
            return true;


        }

        public static bool check_status(string alias)
        {
            var currentClient = torrent_clients.client.GetByAlias(alias);

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
