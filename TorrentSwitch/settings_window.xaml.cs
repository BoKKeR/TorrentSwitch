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
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Add_window add_manager = new Add_window();
            add_manager.Show();
        }

        public struct manager_data
        {
            public string status { set; get; }
            public string host { set; get; }
            public string client_type { set; get; }
        }
        
        public void AddManager(string status, string host, string client_type)
        {
            dataGrid.Items.Add(new manager_data {status = status, host = host, client_type = client_type});
            //dataGrid.Items.Add(status, host, client_type);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
