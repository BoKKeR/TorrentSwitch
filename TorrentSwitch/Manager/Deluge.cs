using System;
using TorrentSwitch.torrent_clients;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace TorrentSwitch.managers.Deluge
{

    class Deluge
    {
        private static CookieAwareWebClient webclient { get; set; }
        private static string URL { get; set; }

        private static void initializeWebClient()
        {
            webclient = new CookieAwareWebClient();
            webclient.Encoding = Encoding.UTF8;
            webclient.Headers.Set("content-type", "application/json");
        }

        private static void getURL(Settings currentClient)
        {
            URL = "http://" + currentClient.hostname + ":" + currentClient.port + "/json";
        }

        private static byte[] buildRequest(string method, string parameter, string label)
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



        private static byte[] sendRequest(string method, string parameter, string label = "") =>
           (webclient.UploadData(URL, "POST", buildRequest(method, parameter, label)));

        private static string responseToString(byte[] response)
        {
            MemoryStream output = new MemoryStream();

            using (GZipStream g = new GZipStream(new MemoryStream(response), CompressionMode.Decompress))
            {
                g.CopyTo(output);
            }
            string JsonResponse = Encoding.UTF8.GetString(output.ToArray());

            return JsonResponse;
        }
        public static async Task<bool> StartTask(Settings currentClient, string magnet)
        {
            Task<bool> task = SendMagnetURI(currentClient, magnet);
            bool result = await task;
            return result;

        }

        public static async Task<bool> SendMagnetURI(Settings currentClient, string magnet)
        {
            initializeWebClient();
            getURL(currentClient);
            
            try
            {
                // Authorization
               string authorization = responseToString(sendRequest("auth.login", currentClient.password));
            }
            catch (Exception)
            {
                return false;
            }
            //TorrentAdding
            string addingTorrent = responseToString(sendRequest("core.add_torrent_magnet", magnet));
            if (!string.IsNullOrEmpty(currentClient.label))
            {
                // Check for label -NOT FINISHED      


                // Set label for torrent
                setLabel(currentClient.label, magnet);
            }

            return true;
        }
        private static void setLabel(string label, string magnet)
        {

            string hash = logic.TorrentHandler.MagnetToHash(magnet);
            string settingLabel = responseToString(sendRequest("label.set_torrent", hash, label));

        }
        //private static void CheckForLabel(string label)
        //{
        //    string getLabels = ResponseToString(SendRequest("label.get_labels", "movie", true));
        //    if ()
        //    {

        //        CreateLabel(label);
        //    }
        //    else
        //    {

        //    }

        //}

        //private static void CreateLabel(string label)
        //{
        //    label = "aaa";
        //    string labelResponse = ResponseToString(SendRequest("label.add", label));
        //}

        public static bool CheckStatus(Settings currentClient)
        {
            initializeWebClient();
            getURL(currentClient);
            try
            {
                string authorization = responseToString(sendRequest("auth.login", currentClient.password));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
