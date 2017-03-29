﻿using System;
using System.Windows;
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



        private void ValidateFields()
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            torrent_clients.ClientType client_type = (torrent_clients.ClientType)Enum.Parse(typeof(torrent_clients.ClientType), type_comboBox.Text);
            SqliteDatabase.AddEntry(
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

            ClientWindow.RefreshClients();
            MainWindow.ColumnLoader(alias_textBox.Text);
        }
    }
}