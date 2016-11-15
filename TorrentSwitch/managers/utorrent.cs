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
using RestSharp;
using RestSharp.Authenticators;
using System.Web;
using TorrentSwitch.torrent_clients;

namespace TorrentSwitch.managers
{
    /// <summary>
    /// Here is uTorrent client managed, both sending a torrent file and a magnet link. And checking the status
    /// </summary>
    class uTorrent
    {


        public static void send_torrent(string ip, string port, string username, string password)
        {
            ///READ TOKEN
            string base_url = "http://" + ip + ":" + port + "/gui/";

            string token = null;
            string token_urlAddress = base_url + "token.html";
            WebRequest tokenRequest = WebRequest.Create(token_urlAddress);
            /*
            MainWindow.CookieAwareWebClient client = new MainWindow.CookieAwareWebClient()
            {
                Credentials = new NetworkCredential(username, password)
            };
            */
            tokenRequest.Credentials = new NetworkCredential(username, password);
            //StreamReader Reader = new StreamReader(webRequest(token_urlAddress));
            StreamReader Reader = new StreamReader(tokenRequest.GetResponse().GetResponseStream());
            string read_token = Reader.ReadToEnd();
            token = Regex.Replace(read_token, "<.*?>", String.Empty);
            Debug.WriteLine(token);
            ///SEND TORRENT FILE
            //string sURL = base_url + "?action=add-url&s=magnet:?xt=urn:btih:f9e40a2663ea396d631e1cb2a0b5d10b0f322bd9&dn=Carson.Daly.2016.10.10.Mekhi.Phifer.480p.x264-mSD&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969";





            string sURL = base_url + "?action=add-torrent";
            WebRequest torrent_post = WebRequest.Create(sURL);
            torrent_post.ContentType = "multipart/form-data";
            torrent_post.Method = "POST";
            var reqparm = new System.Collections.Specialized.NameValueCollection();
            byte[] bytes = System.IO.File.ReadAllBytes("one.torrent");
            Debug.WriteLine(bytes[5]);
            using (Stream requestStream = torrent_post.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            //hm
            var client = new RestClient(sURL);
            var request = new RestRequest("resource/{id}", Method.POST);
            request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
            client.Authenticator = new HttpBasicAuthenticator(username, password);
            request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource

            // easily add HTTP Headers
            request.AddHeader("header", "value");
            //request.AddFile();

            // add files to upload (works with compatible verbs)

            IRestResponse response = client.Execute(request);
            var content = response.Content;




            //reqparm.Add("torrent_file", bytes.ToString());
            //reqparm.Add("", "");
            //byte[] responsebytes = client.UploadData(sURL, "POST", Encoding.Default.GetBytes(reqparm.ToString()));//Encoding.Default.GetBytes("{'file' : 'torrent_file' : 'one.torrent'}"));
            //byte[] responsebytes = client.UploadValues(sURL, "POST", reqparm);
            //string responsebody = Encoding.UTF8.GetString(responsebytes);

            //Debug.WriteLine(responsebody);
        }

        //public static void send_magnet_uri(string ip, string port, string username, string password, string hash)
        public static void send_magnet_uri(Settings currentClient, string hash)
        {
            
            string magnet = "magnet:?xt=urn:btih:" + hash;

            string base_url = "http://" + currentClient.hostname + ":" + currentClient.port + "/gui/";

            ///READ TOKEN
            string token_urlAddress = base_url + "token.html";

            CookieAwareWebClient client = new CookieAwareWebClient();
            client.Credentials = new NetworkCredential(currentClient.username, currentClient.password);

            client.OpenRead(token_urlAddress);

            StreamReader Reader = new StreamReader(client.OpenRead(token_urlAddress));
            string token = Reader.ReadToEnd();

            token = Regex.Replace(token, "<.*?>", String.Empty);
            ///SEND MAGNET LINK
            string add_url = base_url + "?action=add-url&s=" + WebUtility.UrlEncode(magnet) + "&token=" + token;
            client.OpenRead(add_url);
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
