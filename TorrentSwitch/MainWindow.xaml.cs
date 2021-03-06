﻿using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using TorrentSwitch.managers;
using TorrentSwitch.torrentClients;
using Settings = TorrentSwitch.torrentClients.Settings;
using System.Threading.Tasks;
using TorrentSwitch.Window;

namespace TorrentSwitch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : MetroWindow
    {
        managers.Deluge deluge = new managers.Deluge();
        managers.Transmission transmission = new managers.Transmission();
        managers.UTorrent uTorrent = new managers.UTorrent();
        managers.Vuze vuze  = new managers.Vuze();

        public static MainWindow _mainWindow;

        public MainWindow()
        {
            _mainWindow = this;
            InitializeComponent();
            ArgumentLoader();
            SqliteDatabase.CheckForDatabase();
            SqliteDatabase.LoadDatabase();
            pipeServer.StartAsyncServer();
        }
        
        /// <summary>
        /// Handles the top client button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void clientButton(object sender, RoutedEventArgs e)
        {
            ClientWindow clientWin= new ClientWindow();
            clientWin.ShowDialog();
        }


        #region dataGrid managment

        /// <summary>
        /// Generates a new column for each client.
        /// </summary>
        /// <param name="alias">The alias.</param>
        public static void ColumnLoader(string alias)
        {
            FrameworkElementFactory buttonTemplate = new FrameworkElementFactory (typeof(Button));
            var text = new FrameworkElementFactory(typeof(TextBlock));
            text.SetValue(TextBlock.TextProperty, alias);
            buttonTemplate.AppendChild(text);

            buttonTemplate.AddHandler(
                Button.ClickEvent,
                new RoutedEventHandler((o, e) => _mainWindow.sendTorrent(o, e))
                
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
        private void removeMe(object sender, RoutedEventArgs e)
        {
            dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
        }


        /// <summary>
        /// Handles every button from the last Columns of the DataGrid, on button click event it reads from the selected row:
        /// magnet link, actual_client, clientType
        /// Sends the magnet link to the right client. 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        public async void sendTorrent(object sender, RoutedEventArgs e)
        {
            TorrentData TorrentDataFromRow = (TorrentData)dataGrid.SelectedItems[0];
            
            string actualClient = dataGrid.CurrentColumn.Header.ToString();
            string magnet = TorrentDataFromRow.Magnet;

            Settings clientSettings = client.GetByAlias(actualClient);
            ClientType currentType = clientSettings.ManagerClientType;

            switch (currentType)
            {
                case ClientType.uTorrent:
                    if (await Task.Run(() => uTorrent.SendMagnetURI(clientSettings, magnet)))
                    {
                        dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
                    }
                    else
                    {
                        
                    }
                    break;

                case ClientType.Deluge:
                    if (await Task.Run(() => deluge.SendMagnetURI(clientSettings, magnet)))
                    {
                        dataGrid.RowBackground = Brushes.Yellow;
                        dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
                    }
                    else
                    {
                        //change color to red
                    }
                    break;

                case ClientType.Transmission:
                    if (await Task.Run(() => transmission.SendMagnetURI(clientSettings, magnet)))
                    {
                        dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
                    }
                    else
                    {
                        
                        //change color to red
                    }
                    break;

                case ClientType.Qbittorrent:
                    if (await Task.Run(() => managers.Qbittorrent.SendMagnetURI(clientSettings, magnet)))
                    {
                        dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
                    }
                    else
                    {
                        //change color to red
                    }
                    break;

                case ClientType.Vuze:
                    if (await Task.Run(() => vuze.SendMagnetURI(clientSettings, magnet)))
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
                logic.dataGrid.LoadTarget(torrentFile);
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
        private void dataGridDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] torrentList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                foreach (string torrentFile in torrentList)
                    logic.dataGrid.LoadTarget(torrentFile);
            }
        }
#endregion
    }
}