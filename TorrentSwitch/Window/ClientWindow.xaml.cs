using System;
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
            Load_dataGrid();
        }



        public struct manager_data
        {
            public BitmapImage status { set; get; }
            
            public string alias { set; get; }
            public string host { set; get; }
            public string client_type { set; get; }
        }
        public void Load_dataGrid()
        {
            
            BitmapImage online = new BitmapImage(new Uri("/Image/online.png", UriKind.Relative));
            BitmapImage offline = new BitmapImage(new Uri("/Image/offline.png", UriKind.Relative));

            foreach (var setting in torrent_clients.client.users)
            {
                
                BitmapImage actual_status;
                switch (setting.ManagerClientType) //Checks which type of manager you are adding and checks if the client is online/offline
                {
                    case ClientType.uTorrent:
                    {
                            actual_status = managers.UTorrent.UTorrent.CheckStatus(setting) ? online : offline;
                            break;
                    }

                    case ClientType.Deluge:
                    {
                            actual_status = managers.Deluge.Deluge.CheckStatus(setting) ? online : offline;
                            break;
                    }
                    default:
                    {
                            actual_status = managers.UTorrent.UTorrent.CheckStatus(setting) ? online : offline; // change to transmission later
                            break;
                    }
                }

                dataGrid.Items.Add(new manager_data {
                    
                    status = actual_status,
                    alias = setting.alias,
                    host = setting.username + "@" + setting.hostname + ":" + setting.port,
                    client_type = setting.ManagerClientType.ToString() });
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            windows.AddWindow add_manager = new windows.AddWindow();
            add_manager.ShowDialog();
        }
        
        public static void Refresh_clients()
        {
            _settingsWindowForm.dataGrid.Items.Clear();
            _settingsWindowForm.Load_dataGrid();
            
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
            {
                manager_data dataRow = (manager_data)dataGrid.SelectedItem;
                string selectedAlias = dataRow.alias;
                SqliteDatabase.RemoveEntry(selectedAlias);
                client.removeUser(selectedAlias); 
                dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
                MainWindow.ColumnRemover(selectedAlias);
                Refresh_clients();
            }    
        }
    }
}