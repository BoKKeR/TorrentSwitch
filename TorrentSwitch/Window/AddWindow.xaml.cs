using System;
using System.Windows;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Linq;

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



        private void type_comboBox_SelectedItemChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            int selectedIndex = type_comboBox.SelectedIndex;
            Object selectedItem = type_comboBox.SelectedItem;

            string tempItem = selectedItem.ToString();
            //STUPID HACK NEEDS TO BE FIXED
            string item = tempItem.Split(' ').Last();
            //STUPID HACK NEEDS TO BE FIXED
            switch (item)
            {
                case "uTorrent":
                    {
                        port_textBox.Text = "";
                    }
                    break;

                case "Transmission":
                    {
                        port_textBox.Text = "9091";
                    }
                    break;

                case "Deluge":
                    {
                        port_textBox.Text = "8112";
                    }
                    break;

                case "qBittorrent":
                    {
                        port_textBox.Text = "";
                    }
                    break;

                case "Vuze":
                    {
                        port_textBox.Text = "9091";
                    }
                    break;
            }
        }
        internal bool ValidateFields()
        {

            if (!string.IsNullOrEmpty(alias_textBox.Text) &&
                !string.IsNullOrEmpty(hostname_textBox.Text) &&
                !string.IsNullOrEmpty(port_textBox.Text) &&
                !string.IsNullOrEmpty(username_textBox.Text) &&
                !string.IsNullOrEmpty(passwordBox.Password) &&
                !string.IsNullOrEmpty(type_comboBox.Text))
                //add int check for port. 
                //add path check for path
                //check for illegal characters
                return true;
            else
                return false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            if (ValidateFields())
            {

            torrentClients.ClientType client_type = (torrentClients.ClientType)Enum.Parse(typeof(torrentClients.ClientType), type_comboBox.Text);
            SqliteDatabase.AddEntry(
                alias_textBox.Text, 
                hostname_textBox.Text, 
                port_textBox.Text, 
                username_textBox.Text,
                passwordBox.Password, 
                type_comboBox.Text, 
                Custom_Path.Text, 
                label_textBox.Text);

            torrentClients.client.AddUser(alias_textBox.Text, 
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

        private void type_comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        //private void type_comboBox_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{

        //}
    }
}