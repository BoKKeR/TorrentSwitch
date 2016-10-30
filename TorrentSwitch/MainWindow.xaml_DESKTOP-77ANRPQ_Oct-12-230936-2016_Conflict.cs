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
    */

    /* FIX
     * size for multi torrents
     * GUI scalability                     ///DONE
     * Drop to load torrent                ///DONE
     * Utorrent add torrent                 
     * Index reload if torrent removed from dataGrid
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
                LoadSettings();
                ArgumentLoader();
                sqlite_database.check_for_database();
                


            }

        public void LoadSettings()
        {
            sqlite_database.check_for_database();
            Debug.WriteLine("over_sqlite");

            bool settings_available = true;
            if (settings_available)
            {
                
            }
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
            int row = Grid.GetRow(dataGrid);
            dataGrid.Items.Add(new MyData {Name = Name, Size = Size, first = "Deluge"});

        }
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateText(TorrentLoader("Naid.torrent").Item1, "a", "a");
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

        public Tuple<string, string> TorrentLoader(string torrent_file)
        {
            
            
            Debug.WriteLine(torrent_file);
            var parser = new BencodeParser();
            var torrent = parser.Parse<Torrent>(torrent_file);


            switch (torrent.FileMode)
            {
                case TorrentFileMode.Single:
                    return Tuple.Create(torrent.File.FileName, SizeSuffix(torrent.File.FileSize).ToString());
                case TorrentFileMode.Multi:
                    return Tuple.Create(torrent.Files.DirectoryName, "N/A"); ///FIX SIZE
                default:
                    return Tuple.Create("Not recognized","Not recognized");
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

        public struct MyData
        {
            public string Name { set; get; }
            public string Size { set; get; }
            public string first { set; get; }
            
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void remove_me(object sender, RoutedEventArgs e)
        {
            dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
        }



        private void settings_button(object sender, RoutedEventArgs e)
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

        public void Deluge()
        {
            
        }


        private void uTorrent_test_Click(object sender, RoutedEventArgs e)
        {
        
            managers.uTorrent.get_token("127.0.0.1","8787","admin","admin");
        }

    }
}
    
