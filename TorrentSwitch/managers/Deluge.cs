using System;
using System.Collections.Generic;
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
        Dictionary<string, int> points = new Dictionary<string, int>
        {
            { "Ja", 9001 },
            { "J", 3474 },
            { "Js", 11926 }
        };

        public static bool send_magnet_uri(Settings currentClient, string magnet)
        {

            CookieAwareWebClient client = new CookieAwareWebClient();
            client.Credentials = new NetworkCredential("deluge", "deluge");
            client.Headers.Set("content-type","application/json");
            
            var reqparm = new System.Collections.Specialized.NameValueCollection();
            reqparm.Add("", "<any> kinds & of = ? strings");

            byte[] responsebytes = client.UploadValues("http://10.0.0.123:8112/json", "POST", reqparm);
            string responsebody = Encoding.UTF8.GetString(responsebytes);

            return true;

        }

    }
}
