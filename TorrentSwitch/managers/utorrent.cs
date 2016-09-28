using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TorrentSwitch.managers
{
    class uTorrent
    {
        public static void get_token(string ip, string port, string username, string password)
        {
            string base_url = "http://" + ip + ":" + port + "/gui/";
            string hash_example = "9F9165D9A281A9B8E782CD5176BBCC8256FD1871";
            string token = null;
            string token_urlAddress = base_url + "token.html";
            MainWindow.CookieAwareWebClient client = new MainWindow.CookieAwareWebClient()
            {
                Credentials = new NetworkCredential(username, password)
            };
            ;
            StreamReader Reader = new StreamReader(client.OpenRead(token_urlAddress));
            string read_token = Reader.ReadToEnd();
            token = Regex.Replace(read_token, "<.*?>", String.Empty);
            //Debug.WriteLine(token);
            string token_url = base_url + "?action=add-file";
            send_file(token_url);
        }
        public static void send_file(string token_url)
        {
            //string sURL = "?action=start&hash=" + hash_example + "&token=" + token;

            /* multipart / form - data
               file, torrent_file 
            */
            MainWindow.CookieAwareWebClient client = new MainWindow.CookieAwareWebClient()
            {
                Credentials = new NetworkCredential("admin", "admin")
            };

            client.Headers.Add("Content-Type", "multipart/form-data");

            var reqparm = new System.Collections.Specialized.NameValueCollection();
            reqparm.Add("file", "torrent_file=one.torrent");
            string json = @"{'file':[{'torrent_file':'C:/Bork/one.torrent'}]}";
            byte[] responsebytes = client.UploadData("sURL", "POST", Encoding.Default.GetBytes(json));//Encoding.Default.GetBytes("{'file' : 'torrent_file' : 'one.torrent'}"));
            string responsebody = Encoding.UTF8.GetString(responsebytes);
            
            Debug.WriteLine(responsebody);
        }
    }
}
