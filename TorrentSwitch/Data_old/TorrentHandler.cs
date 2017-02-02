using BencodeNET.Parsing;
using BencodeNET.Torrents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentSwitch.logic
{
    class torrentHandler
    {
        public static Boolean magnet_check(string torrent)
        {
            if (torrent.StartsWith("magnet:?"))
            {
                return true;
            }
            return false;
        }

        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        /// <summary>
        /// converts the size from bits to more readable format
        /// </summary>
        public static string SizeSuffix(Int64 value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }
        /// <summary>
        /// Quick check to see if torrent passes a basic test
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        /// <returns></returns>
        public static Boolean torrent_check(string torrent)
        {
            if (torrent.EndsWith(".torrent") && (File.Exists(torrent)))
            {
                return true;
            }
            return false;
        }
        public static Torrent torrent { get; set; }


        /// <summary>
        /// Extracts the torrent properties, file/files, size and even the magnet link.
        /// </summary>
        /// <param name="torrentFile">The torrent file.</param>
        /// <returns></returns>
        public static Tuple<string, string, string> TorrentInfoExtractor(string torrentFile)
        {
            
            var parser = new BencodeParser();

            try
            {
                torrent = parser.Parse<Torrent>(torrentFile);
            }
            catch (Exception)
            {
                //add dialog
            }

            switch (torrent.FileMode)
            {
                case TorrentFileMode.Single:
                    return Tuple.Create(torrent.File.FileName, logic.torrentHandler.SizeSuffix(torrent.File.FileSize), torrent.GetMagnetLink());
                case TorrentFileMode.Multi:
                    return Tuple.Create(torrent.Files.DirectoryName, logic.torrentHandler.SizeSuffix(torrent.TotalSize), torrent.GetMagnetLink());
                default:
                    return Tuple.Create("Not recognized", "Not recognized", "Not recognized");
            }
        }
    }

}
