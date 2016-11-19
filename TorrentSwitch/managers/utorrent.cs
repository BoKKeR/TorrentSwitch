using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using TorrentSwitch.torrent_clients;

namespace TorrentSwitch.managers
{
    /// <summary>
    /// Here is uTorrent client managed, sending a torrent magnet link to the client. And checking the status
    /// </summary>
    class uTorrent
    {
        public static void send_magnet_uri(Settings currentClient, string magnet)
        {
            string base_url = "http://" + currentClient.hostname + ":" + currentClient.port + "/gui/";

            ///READ TOKEN
            string token_urlAddress = base_url + "token.html";

            CookieAwareWebClient client = new CookieAwareWebClient();
            client.Credentials = new NetworkCredential(currentClient.username, currentClient.password);

            //try
            //{
                client.OpenRead(token_urlAddress);
                StreamReader Reader = new StreamReader(client.OpenRead(token_urlAddress));
                string token = Reader.ReadToEnd();

                token = Regex.Replace(token, "<.*?>", String.Empty);
                ///SEND MAGNET LINK
                string add_url = base_url + "?action=add-url&s=" + magnet + "&token=" + token;
                client.OpenRead(add_url);
            //}
            //catch (Exception)
            //{
                
            //}


            
        }

        public static bool check_status(string alias)
        {
            torrent_clients.Settings actual_client = new torrent_clients.Settings();
            actual_client = torrent_clients.client.GetByAlias(alias);
            string base_url = "http://" + actual_client.hostname + ":" + actual_client.port + "/gui/";
            
            ///READ TOKEN
            string token_urlAddress = base_url + "token.html";

            CookieAwareWebClient client = new CookieAwareWebClient();
            client.Credentials = new NetworkCredential(actual_client.username, actual_client.password);
            Debug.WriteLine(actual_client.hostname);
            try
            {
                client.OpenRead(token_urlAddress);
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

    }
}
