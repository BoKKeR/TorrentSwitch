using System;
using System.Data;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using BencodeNET.Torrents;
using BencodeNET.Parsing;
using MahApps.Metro.Controls;
using System.IO;
using System.Diagnostics;
using System.Security.Policy;
using System.Windows.Data;
using System.Windows.Documents;
using TorrentSwitch.managers;
using TorrentSwitch.Properties;
using TorrentSwitch.torrent_clients;
using Settings = TorrentSwitch.torrent_clients.Settings;


namespace TorrentSwitch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : MetroWindow
        {
            static MainWindow mainWindow;
            public MainWindow()
            {
                mainWindow = this;

                InitializeComponent();
                ArgumentLoader();
                SqliteDatabase.check_for_database();
                SqliteDatabase.load_database(); 
            }

        /// <summary>
        /// Handles the top client button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void client_button(object sender, RoutedEventArgs e)
        {
            settings_window set_win = new settings_window();
            set_win.Show();
        }

        #region dataGrid managment

        /// <summary>
        /// Generates a new column for each client.
        /// </summary>
        /// <param name="alias">The alias.</param>
        public static void ColumnLoader(string alias)
        { 
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = alias;
            textColumn.Binding = new Binding(alias);
            mainWindow.dataGrid.Columns.Add(textColumn);
        }



        /// <summary>
        /// Removes the torrent from the dataGrid.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void remove_me(object sender, RoutedEventArgs e)
        {
            dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
        }

        /// <summary>
        /// Loads the torrent to the dataGrid
        /// </summary>
        /// <param name="torrent_file">The torrent file.</param>
        public void dataGridLoadTorrent(string torrent_file)
        {
            if (torrent_check(torrent_file))
            {
                DataGridAddRow(TorrentExtractor(torrent_file).Item1,
                    TorrentExtractor(torrent_file).Item2,
                    TorrentExtractor(torrent_file).Item3, 
                    "a");
            }
        }
        public void send_torrent(object sender, RoutedEventArgs e)
        {
            string actual_client = null;
            string hash = null;
            DataRowView row = (DataRowView)((Button)e.Source).DataContext;
            Debug.WriteLine(row);
            dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
            Settings currentClient = torrent_clients.client.GetByAlias(actual_client);
            managers.uTorrent.send_magnet_uri(currentClient, hash);
        }

        /// <summary>
        /// Extracts the torrent properties, file/files, size, hash.
        /// </summary>
        /// <param name="torrent_file">The torrent file.</param>
        /// <returns></returns>
        public Tuple<string, string, string> TorrentExtractor(string torrent_file)
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
                    return Tuple.Create(torrent.File.FileName, SizeSuffix(torrent.File.FileSize), torrent.GetInfoHash());
                case TorrentFileMode.Multi:
                    return Tuple.Create(torrent.Files.DirectoryName, "N/A", torrent.GetInfoHash()); ///FIX SIZE
                default:
                    return Tuple.Create("Not recognized", "Not recognized", "Not recognized");
            }
        }

        /// <summary>
        /// Removes the column that belonged the client that got removed.
        /// </summary>
        /// <param name="alias">The alias.</param>
        public static void RemoveColumn(string alias)
        {

            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = alias;
            mainWindow.dataGrid.Columns.Remove(textColumn);
            RefreshColumns();
        }

        public static void RefreshColumns()
        {
            var temp = mainWindow.dataGrid.ItemsSource;
            mainWindow.dataGrid.ItemsSource = null;
            mainWindow.dataGrid.ItemsSource = temp;
        }

        /// <summary>
        /// Adds a row to the dataGrid including the buttons
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <param name="Size">The size.</param>
        /// <param name="first">The first.</param>
        /// <param name="hash">The hash.</param>
        public void DataGridAddRow(string Name, string Size, string first, string hash)
        { 
            dataGrid.Items.Add(new torrent_data {Name = Name, Size = Size, Hash = hash});
        }
        #endregion

        #region torrent managment  
        /// <summary>
        /// Quick check to see if torrent passes a basic test
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        /// <returns></returns>
        public static Boolean torrent_check(string torrent)
        {
            if (torrent.EndsWith(".torrent") && (File.Exists(torrent)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loads the torrents when using arguments
        /// </summary>
        public void ArgumentLoader()
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (var torrentFile in args)
            {
                if (torrent_check(torrentFile))
                {
                    TorrentExtractor(torrentFile);
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

        /// <summary>
        /// converts the size from bits to more readable format
        /// </summary>
        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        /// <summary>
        /// Sizes the suffix.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
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
            public string Hash { set; get; }
            public string First { set; get; } 
        }
        /// <summary>
        /// Handles the Drop event of the dataGrid control. Supports multi-file-drop
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void dataGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] torrentList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                foreach (string element in torrentList)
                    TorrentExtractor(element);
            }
        }
        #endregion  
    }
}
    
