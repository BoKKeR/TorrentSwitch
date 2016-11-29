using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace TorrentSwitch.windows
{
    /// <summary>
    /// This handles adding a new client
    /// </summary>
    public partial class AddWindow : MetroWindow
    {
        public AddWindow()
        {
            InitializeComponent();
        }



        private void validate_fields()
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            torrent_clients.ClientType client_type = (torrent_clients.ClientType)Enum.Parse(typeof(torrent_clients.ClientType), type_comboBox.Text);
            SqliteDatabase.add_entry(
                alias_textBox.Text, 
                hostname_textBox.Text, 
                port_textBox.Text, 
                username_textBox.Text,
                passwordBox.Password, 
                type_comboBox.Text, 
                Custom_Path.Text, 
                label_textBox.Text);

            torrent_clients.client.AddUser(alias_textBox.Text, 
                hostname_textBox.Text, 
                port_textBox.Text, 
                username_textBox.Text, 
                passwordBox.Password, 
                client_type, Custom_Path.Text, 
                label_textBox.Text);

            ClientWindow.Refresh_clients();
            MainWindow.ColumnLoader(alias_textBox.Text);
        }
    }
}
