using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;


namespace TorrentSwitch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex _mutex;

        public App()
        {
            bool aIsNewInstance;

            _mutex = new Mutex(true, @"Global\" + "MyUniqueWPFApplicationName", out aIsNewInstance);

            GC.KeepAlive(_mutex);

            if (aIsNewInstance) return;

            MessageBox.Show("There is already an instance running.",
                "Instance already running.",
                MessageBoxButton.OK, MessageBoxImage.Information);

            Current.Shutdown();
        }
    }
}