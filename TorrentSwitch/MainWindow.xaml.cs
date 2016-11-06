using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using BencodeNET.Torrents;
using BencodeNET.Parsing;
using MahApps.Metro.Controls;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Data.SQLite;
using TorrentSwitch.managers;

namespace TorrentSwitch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    /* ADD
     * Data saving for manager settings  //in progress
     * Deluge support
     * Transmission support
     * Magnet link support
     * Torrent/magnet link association
     * Debug window
     * Color options
     * color/icon options for metro
     * path support
     * label support
    */

    /* FIX
     * size for multi torrents
     * GUI scalability                     ///DONE
     * Drop to load torrent                ///DONE
     * Utorrent add torrent                
     * Utorrent add magnet links           ///DONE 
    */


        public enum ManagerType
        {
            uTorrent,
            Deluge,
            Transmission
        };

        public class SettingsClass
            {
                public ManagerType Type;
                public string user;
                public string password;
                public string ip;
                public string port;
        
            }
        

        
        public partial class MainWindow : MetroWindow
        {
            public MainWindow()
            {

                InitializeComponent();
                ArgumentLoader();
                SqliteDatabase.check_for_database();
                SqliteDatabase.load_database();
            }



        public void get_torrent(string torrent_file)
        {
            
            if (torrent_check(torrent_file))
            { 
            UpdateText(TorrentLoader(torrent_file).Item1, TorrentLoader(torrent_file).Item2,  "a");
            }
        }

        public void UpdateText(string Name, string Size, string first)
        { 
            dataGrid.Items.Add(new torrent_data {Name = Name, Size = Size, first = "Deluge"});
        }

        private void button1_Click(object sender, EventArgs e)
        {
            get_torrent("one.torrent");
        }

        public static Boolean torrent_check(string torrent)
        {
            if (torrent.EndsWith(".torrent") && (File.Exists(torrent)))
            {
                return true;
            }
            return false;

        }

       
        public void ArgumentLoader()
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (var torrent_file in args)
            {
                if (torrent_check(torrent_file))
                {
                    get_torrent(torrent_file);
                }
            }
        }


        private Torrent _torrent;

        public Torrent torrent
        {
            get
            {
                return _torrent;
            }
            set
            {
                _torrent = value;
            }
        }

        public Tuple<string, string> TorrentLoader(string torrent_file)
        {
            Debug.WriteLine(torrent_file);
            var parser = new BencodeParser();

            try
            {
                 this.torrent = parser.Parse<Torrent>(torrent_file);
            }
            catch (Exception)
            {
                //ADD DIALOG 

            }

            switch (torrent.FileMode)
            {
                case TorrentFileMode.Single:
                    return Tuple.Create(torrent.File.FileName, SizeSuffix(torrent.File.FileSize).ToString());
                case TorrentFileMode.Multi:
                    return Tuple.Create(torrent.Files.DirectoryName, "N/A"); ///FIX SIZE
                default:
                    return Tuple.Create("Not recognized", "Not recognized");
            }
        }

        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        public struct torrent_data
        {
            public string Name { set; get; }
            public string Size { set; get; }
            public string first { set; get; } 
        }

        private void remove_me(object sender, RoutedEventArgs e)
        {
            dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
        }

        private void client_button(object sender, RoutedEventArgs e)
        {
            settings_window set_win = new settings_window();
            set_win.Show();
        }

        private void dataGrid_Drop(object sender, DragEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
        
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] torrentList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                foreach (string element in torrentList)
                    get_torrent(element);
            }
            
        }

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

        private void uTorrent_test_Click(object sender, RoutedEventArgs e)
        {
            //uTorrent.send_magnet_uri("127.0.0.1", "8787", "admin", "admin", "magnet:?xt=urn:btih:9F9165D9A281A9B8E782CD5176BBCC8256FD1871&dn=ubuntu-16.04.1-desktop-amd64.iso&tr=http%3a%2f%2ftorrent.ubuntu.com%3a6969%2fannounce&tr=http%3a%2f%2fipv6.torrent.ubuntu.com%3a6969%2fannounce");
            uTorrent.check_status("127.0.0.1", "8787", "admin", "admin");

        }

    }
}
    
