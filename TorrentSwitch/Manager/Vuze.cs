using System;
using TorrentSwitch.torrentClients;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Text;

namespace TorrentSwitch.managers
{
    class Vuze
    {
        private string URL { get; set; }

        // Sets global URL string using the current client hostname and port. ex.:
        // http://127.0.0.1:9091/vuze/rpc?json=
        private void getURL(Settings currentClient)
        {
            URL = "http://" + currentClient.hostname + ":" + currentClient.port + "/vuze/rpc?json=";
        }

        //creates a JSON string in this format:
        //{"method":"torrent-add","arguments":{"filename":"MAGNET/FILE"}}
        private string buildJson(string parameter)
        {
            JObject X = new JObject(
                                new JProperty("method", "torrent-add"),
                                new JProperty("arguments",
                                        new JObject(
                                            new JProperty("filename", parameter))));

            string json = JsonConvert.SerializeObject(X, Formatting.None);
            return json;
        }

        private string sendRequest(string requestURL)
        {
            
            WebRequest request = WebRequest.Create(requestURL);
            request.Proxy = null; //cuts 10 seconds from the request

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Get the stream associated with the response.
            Stream receiveStream = response.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            //Stream dataStream;
            string ResponseJson = readStream.ReadToEnd();
            response.Close();
            return ResponseJson;
        }
        

        //NEEDS WORK
        private void processResponse(string ResponseJson)
        {
        
        }

        public bool SendMagnetURI(Settings currentClient, string magnet)
        {
            // Sets global URL string depending on the client settings 
            getURL(currentClient);

            //Creates url in this format and makes request:
            //http://127.0.0.1:9091/vuze/rpc?json={"method":"torrent-add","arguments":{"filename":"MAGNET"}}
            sendRequest(URL + buildJson(magnet));

            return true;
        }

        public bool CheckStatus(Settings currentClient)
        {
            getURL(currentClient);
            try
            {
                sendRequest(URL);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
