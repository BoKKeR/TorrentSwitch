using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Xaml;

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

            _mutex = new Mutex(true, @"Global\" + "TorrentSwitch", out aIsNewInstance);

            GC.KeepAlive(_mutex);

            if (!aIsNewInstance)        
            {
                pipeClient.messagingClient();
                Current.Shutdown();
            }
        }
    }
}