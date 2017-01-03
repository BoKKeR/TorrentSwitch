namespace TorrentSwitch.logic
{
    public class dataGrid_logic
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
            if (torrent_logic.torrent_check(targetTorrent))
            {
                dataGrid_logic.DataGridAddRow(torrent_logic.TorrentInfoExtractor(targetTorrent).Item1,
                    torrent_logic.TorrentInfoExtractor(targetTorrent).Item2,
                    torrent_logic.TorrentInfoExtractor(targetTorrent).Item3);
            }
            if (torrent_logic.magnet_check(targetTorrent))
            {
                dataGrid_logic.DataGridAddRow(targetTorrent, "N/A", targetTorrent);
            }

        }
    }


}