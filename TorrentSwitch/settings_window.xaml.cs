using System;
using System.Collections.Generic;
using System.Linq;
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
        public settings_window()
        {
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
                if (managers.uTorrent.check_status(setting.alias))
                {
                    actual_status = online;
                }
                    else
                {
                    actual_status = offline;
                }
                dataGrid.Items.Add(new manager_data {
                    
                    status = actual_status,
                    alias = setting.alias,
                    host = setting.username + "@" + setting.hostname + ":" + setting.port,
                    client_type = setting.Type.ToString() });
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {

        }



        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Add_window add_manager = new Add_window();
            add_manager.Show();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
