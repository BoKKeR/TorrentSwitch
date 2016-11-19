using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace TorrentSwitch
{
    /// <summary>
    /// Interaction logic for settings_window.xaml
    /// </summary>
    public partial class settings_window : MetroWindow

    {
        static settings_window _settingsWindowForm;
        
        public settings_window()
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
            BitmapImage online = new BitmapImage(new Uri("/images/online.png", UriKind.Relative));
            BitmapImage offline = new BitmapImage(new Uri("/images/offline.png", UriKind.Relative));

            foreach (var setting in torrent_clients.client.users)
            {
                
                BitmapImage actual_status;
                actual_status = managers.uTorrent.check_status(setting.alias) ? online : offline;
                dataGrid.Items.Add(new manager_data {
                    
                    status = actual_status,
                    alias = setting.alias,
                    host = setting.username + "@" + setting.hostname + ":" + setting.port,
                    client_type = setting.ManagerClientType.ToString() });
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            windows.Add_window add_manager = new windows.Add_window();
            add_manager.Show();
        }
        
        public static void Refresh_clients()
        {
            _settingsWindowForm.dataGrid.Items.Refresh();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex != -1)
            {
                Debug.WriteLine(dataGrid.SelectedIndex);
                manager_data dataRow = (manager_data)dataGrid.SelectedItem;
                string selectedAlias = dataRow.alias;
                Debug.WriteLine(selectedAlias);
                SqliteDatabase.remove_entry(selectedAlias);
                dataGrid.Items.RemoveAt(dataGrid.SelectedIndex);
                MainWindow.RemoveColumn(selectedAlias);
                Refresh_clients();
            }    
        }
    }
}
