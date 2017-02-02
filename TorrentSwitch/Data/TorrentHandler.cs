using BencodeNET.Parsing;
using BencodeNET.Torrents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TorrentSwitch.logic
{
    class TorrentHandler
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
                    return Tuple.Create(torrent.File.FileName, logic.TorrentHandler.SizeSuffix(torrent.File.FileSize), torrent.GetMagnetLink());
                case TorrentFileMode.Multi:
                    return Tuple.Create(torrent.Files.DirectoryName, logic.TorrentHandler.SizeSuffix(torrent.TotalSize), torrent.GetMagnetLink());
                default:
                    return Tuple.Create("Not recognized", "Not recognized", "Not recognized");
            }
        }

        public static string MagnetToHash(string magnet)
        {
            string hash = "";
            string[] words = magnet.Split('&');
            Regex regex = new Regex("btih:(.*)");
            foreach (string word in words)
            {
                Match match = regex.Match(word);
                if (match.Success)
                {
                    hash = match.Value;
                    hash = hash.Replace("btih:", "");
                }
            }
            return hash;
        }
    }

}
