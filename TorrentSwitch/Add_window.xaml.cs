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
    /// Interaction logic for Add_window.xaml
    /// </summary>
    public partial class Add_window : Window
    {
        public Add_window()
        {
            InitializeComponent();
        }


        private void check_fields()
        {
            
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            SqliteDatabase.add_entry(label_textBox.Text, hostname_textBox.Text, port_textBox.Text, username_textBox.Text, passwordBox.Password, Type.Text, path_textBox.Text, alias_textBox.Text);
        }
    }
}
