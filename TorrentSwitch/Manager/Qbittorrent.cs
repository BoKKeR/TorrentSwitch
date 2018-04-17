using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using TorrentSwitch.torrentClients;

namespace TorrentSwitch.managers
{
    public class Qbittorrent
    {

        private static CookieAwareWebClient webclient { get; set; }
        private static string URL { get; set; }
        private static string token { get; set; }

        private static void initializeWebClient(Settings currentClient)
        {
            webclient = new CookieAwareWebClient();
            webclient.Encoding = Encoding.UTF8;
            webclient.Credentials = new NetworkCredential(currentClient.username, currentClient.password);
            webclient.Headers.Set("content-type", "application/json");
            // Get-Set token
            string token = getToken(currentClient);
            webclient.Headers.Set("X-Transmission-Session-Id", token);
        }

        private static void getURL(Settings currentClient)
        {
            URL = "http://" + currentClient.hostname + ":" + currentClient.port + "/transmission/rpc/";
        }

        private static byte[] buildRequest(string method, string parameter, string content)
        {
            JArray jarrayObj = new JArray();
            JObject container = new JObject();
            JProperty jTitle = new JProperty("filename", content);

            container.Add(jTitle);
            jarrayObj.Add(container);

            JObject X = new JObject(
                                new JProperty("method", method),
                                new JProperty("arguments", container),
                                new JProperty("tag", "144"));

            string json = JsonConvert.SerializeObject(X, Formatting.None);
            byte[] request = Encoding.ASCII.GetBytes(json);
            return request;
        }

        private static byte[] sendRequest(string method, string parameter, string label = "") =>
           (webclient.UploadData(URL, "POST", buildRequest(method, parameter, label)));

        private static string responseToString(byte[] response)
        {
            MemoryStream output = new MemoryStream();

            string JsonResponse = Encoding.UTF8.GetString(output.ToArray());

            return JsonResponse;
        }

        private static string getToken(Settings currentClient)
        {
            try
            {
                webclient.UploadString(URL, "getToken");
            }
            catch (WebException ex)
            {
                string token = ex.Response.Headers["X-Transmission-Session-Id"];
                return token;
            }
            return null;
        }

        public static bool SendMagnetURI(Settings currentClient, string magnet)
        {
            getURL(currentClient);
            initializeWebClient(currentClient);
            string token = getToken(currentClient);

            //TorrentAdding
            string torrentAdding = responseToString(sendRequest("torrent-add","filename", magnet));
            
            return true;
        }
        //private static void setLabel(string label, string magnet)
        //{

        //    string hash = logic.TorrentHandler.MagnetToHash(magnet);
        //    string settingLabel = responseToString(sendRequest("label.set_torrent", hash, label));
        //}

        public static bool CheckStatus(Settings currentClient)
        {
            getURL(currentClient);
            initializeWebClient(currentClient);

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
