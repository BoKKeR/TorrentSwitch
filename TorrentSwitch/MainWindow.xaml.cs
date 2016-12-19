﻿using System;
using System.Windows;
using System.Windows.Controls;
using BencodeNET.Torrents;
using BencodeNET.Parsing;
using MahApps.Metro.Controls;
using System.IO;
using System.Threading.Tasks;
using TorrentSwitch.managers;
using TorrentSwitch.torrent_clients;
using TorrentSwitch.logic;
using Settings = TorrentSwitch.torrent_clients.Settings;


namespace TorrentSwitch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : MetroWindow
    {

        static MainWindow _mainWindow;

        public MainWindow()
        {

            _mainWindow = this;
            InitializeComponent();
            ArgumentLoader();
            SqliteDatabase.check_for_database();
            SqliteDatabase.load_database();
            Task task = Task.Run((Action)pipe_server.messaging_server);



        }

        public static void crazy_debug(string debug)
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"x:\log.txt", true))
            {
                file.WriteLine(debug);
            }
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
        /// <param name="targetTorrent">The torrent file.</param>
        public void DataGridLoadTarget(string targetTorrent)
        {
            if (torrent_logic.torrent_check(targetTorrent))
            {
                DataGridAddRow(torrent_logic.TorrentInfoExtractor(targetTorrent).Item1,
                    torrent_logic.TorrentInfoExtractor(targetTorrent).Item2,
                    torrent_logic.TorrentInfoExtractor(targetTorrent).Item3);
            }
            if (torrent_logic.magnet_check(targetTorrent))
            {
                DataGridAddRow(targetTorrent, "N/A", targetTorrent);
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

            Settings clientSettings = client.GetByAlias(actualClient);
            ClientType currentType = clientSettings.ManagerClientType;
            switch (currentType)
            {
                case ClientType.uTorrent:
                    if(uTorrent.send_magnet_uri(clientSettings, magnet))
                    {
                        dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
                    }
                    else
                    {
                        //change color to red
                    }
                    break;

                case ClientType.Deluge:
                    if(Deluge.send_magnet_uri(clientSettings, magnet))
                    {
                        dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
                    }
                    else
                    {
                        //change color to red
                    }
                    break;

                case ClientType.Transmission:
                    if (Transmission.send_magnet_uri(clientSettings, magnet))
                    {
                        dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
                    }
                    else
                    {
                        //change color to red
                    }
                    break;
            }
        }

        

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
        /// Loads the torrents when using arguments
        /// </summary>
        public void ArgumentLoader()
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (var torrentFile in args)
            {
                    DataGridLoadTarget(torrentFile);
            }
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

                foreach (string file in torrentList)
                    DataGridLoadTarget(file);
            }
        }
        #endregion
    }
}