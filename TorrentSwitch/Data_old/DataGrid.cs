namespace TorrentSwitch.logic
{
    public class dataGrid
    {
        /// <summary>
        /// Adds a row to the dataGrid including the buttons
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="size">The size.</param>
        /// <param name="magnet">The magnet.</param>
        public static void DataGridAddRow(string name, string size, string magnet)
        {
            MainWindow._mainWindow.dataGrid.Items.Add(new MainWindow.TorrentData { Name = name, Size = size, Magnet = magnet });
        }


        /// <summary>
        /// Loads the torrent to the dataGrid
        /// </summary>
        /// <param name="targetTorrent">The torrent file.</param>
        public static void DataGridLoadTarget(string targetTorrent)
        {
            if (torrentHandler.torrent_check(targetTorrent))
            {
                dataGrid.DataGridAddRow(torrentHandler.TorrentInfoExtractor(targetTorrent).Item1,
                    torrentHandler.TorrentInfoExtractor(targetTorrent).Item2,
                    torrentHandler.TorrentInfoExtractor(targetTorrent).Item3);
            }
            if (torrentHandler.magnet_check(targetTorrent))
            {
                dataGrid.DataGridAddRow(targetTorrent, "N/A", targetTorrent);
            }

        }
    }


}