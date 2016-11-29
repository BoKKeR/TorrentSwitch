using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TorrentSwitch.torrent_clients;



namespace TorrentSwitch.managers
{
    class Deluge
    {
        public static bool send_magnet_uri(Settings currentClient, string magnet)
        {
            string baseUrl = "http://" + currentClient.hostname + ":" + currentClient.port + "/json";

            CookieAwareWebClient client = new CookieAwareWebClient();
            client.Headers.Set("content-type", "application/json");
            try
            {
                string dataStringJson1 = "{\"method\": \"auth.login\", \"params\": [\"" + currentClient.password + "\"], \"id\": \"1\"}";
                client.UploadString(new Uri(baseUrl), "POST", dataStringJson1);

                string dataStringJson2 = "{\"method\": \"core.add_torrent_magnet\", \"params\": [\"" + magnet + "\", {}], \"id\": \"2\"}";
                client.UploadString(new Uri(baseUrl), "POST", dataStringJson2);
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

            string baseUrl = "http://" + currentClient.hostname + ":" + currentClient.port + "/json";

            CookieAwareWebClient client = new CookieAwareWebClient();
            client.Headers.Set("content-type", "application/json");
            try
            {
                string dataStringJson1 = "{\"method\": \"auth.login\", \"params\": [\""+ currentClient.password +"\"], \"id\": \"1\"}";
                string responsebytes1 = client.UploadString(new Uri(baseUrl), "POST", dataStringJson1);

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
