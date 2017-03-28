﻿using System;
using System.Windows;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using TorrentSwitch.torrent_clients;

namespace TorrentSwitch
{
    /// <summary>
    /// Interaction logic for settings_window.xaml
    /// </summary>
    public partial class ClientWindow: MetroWindow

    {
        static ClientWindow _settingsWindowForm;
        
        public ClientWindow()
        {
            _settingsWindowForm = this;
            InitializeComponent();
            LoadDataGrid();
        }



        public struct manager_data
        {
            public BitmapImage status { set; get; }
            
            public string alias { set; get; }
            public string host { set; get; }
            public string clientType { set; get; }
        }
        public void LoadDataGrid()
        {
            
            BitmapImage online = new BitmapImage(new Uri("/Image/online.png", UriKind.Relative));
            BitmapImage offline = new BitmapImage(new Uri("/Image/offline.png", UriKind.Relative));

            foreach (var setting in torrent_clients.client.users)
            {
                
                BitmapImage managerClientStatus = null;
                switch (setting.ManagerClientType) //Checks which type of manager you are adding and checks if the client is online/offline
                {
                    case ClientType.uTorrent:
                        {
                            managerClientStatus = managers.UTorrent.UTorrent.CheckStatus(setting) ? online : offline;
                            break;
                        }

                    case ClientType.Deluge:
                        {
                            managerClientStatus = managers.Deluge.Deluge.CheckStatus(setting) ? online : offline;
                            break;
                        }
                    case ClientType.Transmission:
                        {
                            managerClientStatus = managers.Transmission.Transmission.CheckStatus(setting) ? online : offline;
                            break;
                        }
                    case ClientType.Qbittorrent:
                        {
                            managerClientStatus = managers.Transmission.Transmission.CheckStatus(setting) ? online : offline;
                            break;
                        }

                    default:
                        {
                            managerClientStatus = managers.Qbittorrent.Qbittorrent.CheckStatus(setting) ? online : offline;
                            break;
                        }
                }

                dataGrid.Items.Add(new manager_data {
                    
                    status = managerClientStatus,
                    alias = setting.alias,
                    host = setting.username + "@" + setting.hostname + ":" + setting.port,
                    clientType = setting.ManagerClientType.ToString() });
            }
        }

        private void addClick(object sender, RoutedEventArgs e)
        {
            windows.AddWindow add_manager = new windows.AddWindow();
            add_manager.ShowDialog();
        }
        
        public static void RefreshClients()
        {
            _settingsWindowForm.dataGrid.Items.Clear();
            _settingsWindowForm.LoadDataGrid();
            
        }

        private void removeClick(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
            {
                manager_data dataRow = (manager_data)dataGrid.SelectedItem;
                string selectedAlias = dataRow.alias;
                SqliteDatabase.RemoveEntry(selectedAlias);
                client.RemoveUser(selectedAlias); 
                dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
                MainWindow.ColumnRemover(selectedAlias);
                RefreshClients();
            }    
        }
    }
}