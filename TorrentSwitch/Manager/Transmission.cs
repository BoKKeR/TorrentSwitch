using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TorrentSwitch.torrent_clients;

namespace TorrentSwitch.managers
{
    public class Transmission
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
            token = getToken(currentClient);
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
            token = null;
            try
            {
                webclient.UploadString(URL, "getToken");
            }
            
            catch (WebException ex)
            {
                //token = ex.Response.Headers["X-Transmission-Session-Id"];
                if (ex.Message != "Unable to connect to the remote server")
                {
                     token = ex.Response.Headers["X-Transmission-Session-Id"];
                }
            }
            return token;
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
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
