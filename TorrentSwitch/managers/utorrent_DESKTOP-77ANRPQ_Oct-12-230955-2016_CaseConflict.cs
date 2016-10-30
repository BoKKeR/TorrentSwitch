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
            ///READ TOKEN
            string base_url = "http://" + ip + ":" + port + "/gui/";

            string token = null;
            string token_urlAddress = base_url + "token.html";
            MainWindow.CookieAwareWebClient client = new MainWindow.CookieAwareWebClient()
            {
                Credentials = new NetworkCredential(username, password)
            };
            
            StreamReader Reader = new StreamReader(client.OpenRead(token_urlAddress));
            string read_token = Reader.ReadToEnd();
            token = Regex.Replace(read_token, "<.*?>", String.Empty);
            ///SEND TORRENT FILE
            string sURL = base_url + "?action=add-file";
            client.Headers.Add("Content-Type", "multipart/form-data");

            var reqparm = new System.Collections.Specialized.NameValueCollection();
            byte[] bytes = System.IO.File.ReadAllBytes("one.torrent");
            reqparm.Add("torrent_file", bytes.ToString());
            
            byte[] responsebytes = client.UploadData(sURL, "POST", Encoding.Default.GetBytes(reqparm.ToString()));//Encoding.Default.GetBytes("{'file' : 'torrent_file' : 'one.torrent'}"));
            string responsebody = Encoding.UTF8.GetString(responsebytes);
            
            Debug.WriteLine(responsebody);
        }
    }
}
