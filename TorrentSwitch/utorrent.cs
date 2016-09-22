using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using BencodeNET.Torrents;
using BencodeNET.Parsing;
using MahApps.Metro.Controls;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace TorrentSwitch
{
    class utorrent
    {
        public class CookieAwareWebClient : WebClient
        {
            private readonly CookieContainer m_container = new CookieContainer();

            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);
                HttpWebRequest webRequest = request as HttpWebRequest;
                if (webRequest != null)
                {
                    webRequest.CookieContainer = m_container;
                }
                return request;
            }
        }


        public void utorrent_manager();
        {
         string hash_example = "9F9165D9A281A9B8E782CD5176BBCC8256FD1871";

        string token = null;
        string token_urlAddress = "http://127.0.0.1:8787/gui/token.html";

        CookieAwareWebClient client = new CookieAwareWebClient() { Credentials = new NetworkCredential("admin", "admin") }; ;
            StreamReader Reader = new StreamReader(client.OpenRead(token_urlAddress));
        string read_token = Reader.ReadToEnd();
        token = Regex.Replace(read_token, "<.*?>" , String.Empty);
            
            
        Debug.WriteLine(token);




            




            string sURL = "http://127.0.0.1:8787/gui/?action=start&hash=" + hash_example + "&token=" + token;
        string test_url = "http://127.0.0.1:8787/gui/?action=getsettings&token=" + token;
        Debug.WriteLine(sURL);



            Reader = new StreamReader(client.OpenRead(sURL);
            string response = Reader.ReadToEnd();
        Debug.WriteLine(response);
        }
    }
}
