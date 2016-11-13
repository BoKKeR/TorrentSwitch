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
        public class Deluge_Request
        {
            public string magnet { get; set; }

        }
        public void load_torrent()
        {
            MainWindow.CookieAwareWebClient client = new MainWindow.CookieAwareWebClient();
            client.Credentials = new NetworkCredential("nas", "deluge");
            client.Headers.Set("content-type","application/json");



        }

    }
}
