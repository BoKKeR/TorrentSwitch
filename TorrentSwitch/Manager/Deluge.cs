using System;
using TorrentSwitch.torrentClients;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace TorrentSwitch.managers
{
    class Deluge
    {
        private string URL { get; set; }
        private static CookieAwareWebClient webclient { get; set; }

        private void initializeWebClient()
        {
            webclient = new CookieAwareWebClient();
            webclient.Encoding = Encoding.UTF8;
        }

        private void setContentType()
        {
            webclient.Headers.Set("content-type", "application/json");
        }

        private void getURL(Settings currentClient)
        {
            URL = "http://" + currentClient.hostname + ":" + currentClient.port + "/json";
        }

        private byte[] buildRequest(string method, string parameter, string label)
        {
            JArray jarrayObj = new JArray();
            jarrayObj.Add(parameter);

            if (method == "core.add_torrent_magnet")
            {
                jarrayObj.Add(new JObject());
            }

            if (method == "label.set_torrent" && !string.IsNullOrEmpty(label))
            {
                jarrayObj.Add(label);
            }

            JObject X = new JObject(
                                new JProperty("method", method),
                                new JProperty("params", jarrayObj),
                                new JProperty("id", "1"));

            string json = JsonConvert.SerializeObject(X, Formatting.None);
            byte[] request = Encoding.ASCII.GetBytes(json);

            return request;
        }



        private byte[] sendRequest(string method, string parameter, string label = "")
        {
            setContentType();
            byte[] requestResponse = webclient.UploadData(URL, "POST", buildRequest(method, parameter, label));

            return requestResponse;
        }
        


        private string responseToString(byte[] response)
        {
            MemoryStream output = new MemoryStream();

            using (GZipStream g = new GZipStream(new MemoryStream(response), CompressionMode.Decompress))
            {
                g.CopyTo(output);
            }
            string JsonResponse = Encoding.UTF8.GetString(output.ToArray());

            return JsonResponse;
        }

        public bool SendMagnetURI(Settings currentClient, string magnet)
        {
            initializeWebClient();
            getURL(currentClient);

            try
            {
                // Authorization
               string authorization = responseToString(sendRequest("auth.login", currentClient.password));
            }
            catch (Exception e)
            {
                return false;
            }
            //TorrentAdding
            string addingTorrent = responseToString(sendRequest("core.add_torrent_magnet", magnet));
            if (!string.IsNullOrEmpty(currentClient.label))
            {
                // Set label for torrent
                setLabel(currentClient.label, magnet);
            }

            return true;
        }
        private void setLabel(string label, string magnet)
        {
            string hash = logic.TorrentHandler.MagnetToHash(magnet);
            string settingLabel = responseToString(sendRequest("label.set_torrent", hash, label));

        }

        public bool CheckStatus(Settings currentClient)
        {
            initializeWebClient();
            getURL(currentClient);
            try
            {
                string authorization = responseToString(sendRequest("auth.login", currentClient.password));
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
