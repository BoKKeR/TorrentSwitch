﻿using System;
using System.Windows;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using TorrentSwitch.torrentClients;
using System.Threading.Tasks;
using TorrentSwitch.managers;

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

        static bool CheckClientStatus(Settings setting)
        {
            switch (setting.ManagerClientType)
            {
                //Needs refactoring
                case ClientType.uTorrent:
                    {
                        managers.UTorrent utorrent = new managers.UTorrent();
                        return utorrent.CheckStatus(setting);
                    }

                case ClientType.Deluge:
                    {
                        managers.Deluge deluge = new managers.Deluge();
                        return deluge.CheckStatus(setting);
                    }
                    
                case ClientType.Transmission:
                    {
                        managers.Transmission transmission = new Transmission();
                        return transmission.CheckStatus(setting);
                    }
                case ClientType.Qbittorrent:
                    {
                        return managers.Qbittorrent.CheckStatus(setting);
                    }
                case ClientType.Vuze:
                    {
                        managers.Vuze vuze = new Vuze();
                        return vuze.CheckStatus(setting);
                    }
                default:
                    {
                        managers.UTorrent utorrent = new managers.UTorrent();
                        return utorrent.CheckStatus(setting);
                    }
            }
        }




        public async void LoadDataGrid()
        {

            BitmapImage online = new BitmapImage(new Uri("/Image/online.png", UriKind.Relative));
            BitmapImage offline = new BitmapImage(new Uri("/Image/offline.png", UriKind.Relative));
            //Async call would be appreciated here maybe even checking the status in the background before the window is active:
            foreach (var setting in torrentClients.client.users)
            {

                BitmapImage managerClientStatus = null;
                
                managerClientStatus = await Task.Run(() => CheckClientStatus(setting)) ? online : offline;

                dataGrid.Items.Add(new managerData
                {
                    status = managerClientStatus,
                    alias = setting.alias,
                    host = setting.username + "@" + setting.hostname + ":" + setting.port,
                    clientType = setting.ManagerClientType.ToString()
                });
            }
        }
        public struct managerData
        {
            public BitmapImage status { set; get; }
            
            public string alias { set; get; }
            public string host { set; get; }
            public string clientType { set; get; }
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
                managerData dataRow = (managerData)dataGrid.SelectedItem;
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