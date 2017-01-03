using System;
using System.Text;
using TinyIpc.Messaging;
using TorrentSwitch;
using TorrentSwitch.logic;


class pipeClient
{


    public static void messagingClient()
    {
        using (var messagebus1 = new TinyMessageBus("ExampleChannel"))

        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (var torrentFile in args)
            {
                if (!torrentFile.EndsWith(".exe") )//string message = @"X:\one.torrent";
                    messagebus1.PublishAsync(Encoding.UTF8.GetBytes(torrentFile));
            }
        }
    }
}