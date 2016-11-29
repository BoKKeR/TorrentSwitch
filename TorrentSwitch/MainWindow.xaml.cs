using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using BencodeNET.Torrents;
using BencodeNET.Parsing;
using MahApps.Metro.Controls;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using TorrentSwitch.managers;
using TorrentSwitch.torrent_clients;
using Settings = TorrentSwitch.torrent_clients.Settings;


namespace TorrentSwitch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow
    {

            static MainWindow _mainWindow;
            public MainWindow()
            {
                _mainWindow = this;

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
            ClientWindow setWin = new ClientWindow();
            setWin.ShowDialog();
        }

        #region dataGrid managment

        /// <summary>
        /// Generates a new column for each client.
        /// </summary>
        /// <param name="alias">The alias.</param>
        public static void ColumnLoader(string alias)
        {
            var buttonTemplate = new FrameworkElementFactory(typeof(Button));
            //buttonTemplate.SetBinding(Button.ContentProperty, new Binding("button"));
            
            buttonTemplate.AddHandler(
                Button.ClickEvent,
                new RoutedEventHandler((o, e) => _mainWindow.send_torrent(o, e))
            );

            _mainWindow.dataGrid.Columns.Add(
                new DataGridTemplateColumn()
                {
                    Header = alias,
                    CellTemplate = new DataTemplate() { VisualTree = buttonTemplate }
                }
            );
        }

        public static void ColumnRemover(string alias)
        {

            //int position = ((DataTable) _mainWindow.dataGrid.ItemsSource).Columns[alias].Ordinal;
            //_mainWindow.dataGrid.Columns.RemoveAt(position);
            //_mainWindow.dataGrid.Columns.Remove(alias);
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
        /// <param name="torrentFile">The torrent file.</param>
        public void DataGridLoadTorrent(string torrentFile)
        {
            if (torrent_check(torrentFile))
            {
                DataGridAddRow(TorrentExtractor(torrentFile).Item1,
                    TorrentExtractor(torrentFile).Item2,
                    TorrentExtractor(torrentFile).Item3);
            }
        }

        /// <summary>
        /// Handles every button from the last Columns of the DataGrid, on button click event it reads from the selected row:
        /// magnet link, actual_client, clientType
        /// Sends the magnet link to the right client. 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public void send_torrent(object sender, RoutedEventArgs e)
        {
            TorrentData row = (TorrentData)dataGrid.SelectedItems[0];

            string actualClient = dataGrid.CurrentColumn.Header.ToString();
            string magnet = row.Magnet;

            dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);

            Settings clientSettings = torrent_clients.client.GetByAlias(actualClient);
            ClientType currentType = clientSettings.ManagerClientType;
            switch (currentType)
            {
                case ClientType.uTorrent:
                    uTorrent.send_magnet_uri(clientSettings, magnet);
                    break;
                case ClientType.Deluge:
                    Deluge.send_magnet_uri(clientSettings, magnet);
                    break;
                case ClientType.Transmission:
                    Transmission.send_magnet_uri(clientSettings, magnet);
                    break;
            }


            uTorrent.send_magnet_uri(clientSettings, magnet);
        }

        /// <summary>
        /// Extracts the torrent properties, file/files, size, magnet link.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <returns></returns>
        public Tuple<string, string, string> TorrentExtractor(string torrentFile)
        {
            var parser = new BencodeParser();
            
            try
            {
                this.torrent = parser.Parse<Torrent>(torrentFile);
            }
            catch (Exception)
            {
                //ADD DIALOG 
            }

            switch (torrent.FileMode)
            {
                case TorrentFileMode.Single:
                    return Tuple.Create(torrent.File.FileName, SizeSuffix(torrent.File.FileSize), torrent.GetMagnetLink());
                case TorrentFileMode.Multi:
                    return Tuple.Create(torrent.Files.DirectoryName, "N/A", torrent.GetMagnetLink()); ///FIX SIZE
                default:
                    return Tuple.Create("Not recognized", "Not recognized", "Not recognized");
            }
        }

        ///// <summary>
        ///// Removes the column that belonged the client that got removed.
        ///// </summary>
        ///// <param name="alias">The alias.</param>
        //public static void RemoveColumn(string alias)
        //{
        //    DataGridTextColumn textColumn = new DataGridTextColumn();
        //    textColumn.Header = alias;
        //    _mainWindow.dataGrid.Columns.Remove(textColumn);
        //    RefreshColumns();
        //}

        public static void RefreshColumns()
        {
            var temp = _mainWindow.dataGrid.ItemsSource;
            _mainWindow.dataGrid.ItemsSource = null;
            _mainWindow.dataGrid.ItemsSource = temp;
            _mainWindow.dataGrid.Items.Refresh();
        }

        /// <summary>
        /// Adds a row to the dataGrid including the buttons
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="size">The size.</param>
        /// <param name="magnet">The magnet.</param>
        public void DataGridAddRow(string name, string size, string magnet)
        { 
            dataGrid.Items.Add(new TorrentData {Name = name, Size = size, Magnet = magnet});
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
                    DataGridLoadTorrent(torrentFile);
                }
            }
        }

        public Torrent torrent { get; set; }

        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        /// <summary>
        /// converts the size from bits to more readable format
        /// </summary>
        static string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        public struct TorrentData
        {
            public string Name { set; get; }
            public string Size { set; get; }
            public string Magnet { set; get; }
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
                    DataGridLoadTorrent(element);
            }
        }
        #endregion
    }
}
    
