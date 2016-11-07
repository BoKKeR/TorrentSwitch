using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace TorrentSwitch
{
    /// <summary>
    /// This handles adding a new client
    /// </summary>
    public partial class Add_window : MetroWindow
    {
        public Add_window()
        {
            InitializeComponent();
        }


        private void validate_fields()
        {
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            torrent_clients.Type client_type = (torrent_clients.Type)Enum.Parse(typeof(torrent_clients.Type), type_comboBox.Text);
            SqliteDatabase.add_entry(label_textBox.Text, hostname_textBox.Text, port_textBox.Text, username_textBox.Text, passwordBox.Password, type_comboBox.Text, Custom_Path.Text, Label.Text);
            torrent_clients.client.AddUser(alias_textBox.Text, hostname_textBox.Text, port_textBox.Text, username_textBox.Text, passwordBox.Password, client_type, Custom_Path.Text, label_textBox.Text);
        }
    }
}
