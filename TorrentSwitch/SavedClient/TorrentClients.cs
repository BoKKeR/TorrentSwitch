using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentSwitch.torrentClients
{
    public static class client
    {
        //This can be made static so you can access it all over your application, but remeber to check if it has
        //members before trying to access any of them.
        public static List<Settings> users = new List<Settings>();

        public static void Loop()
        {
            foreach (var setting in users)
            {
                //Loop them here and access them using setting.variable
                Debug.WriteLine(setting.alias);
            }
        }

        public static void RemoveUser(string alias)
        {
            users.Remove(GetByAlias(alias));
        }

        public static Settings GetByAlias(string alias)
        {
            //Will return the first Settings where the alias set in that class matches the alias you provied. 
            //Can also return null if none is existent.
            return users.FirstOrDefault(t => t.alias == alias);
        }

        public static void AddUser(Settings setting)
        {
            //Make sure the setting is not null before trying to add it to the list.
            if (setting != null)
            {
                users.Add(setting);
            }
            else
            {
                throw new NullReferenceException("setting can not be null");
            }
        }

        public static void AddUser(string alias, string hostname, string port, string username, string password, ClientType theClientType, string path,
            string label)
        {
            //Could be a good idea to check that none of the variable is null
            users.Add(new Settings
            {
                alias = alias,
                username = username,
                password = password,
                hostname = hostname,
                port = port,
                label = label,
                path = path,
                ManagerClientType = theClientType
            });
        }
        
    }
    public class Settings
    {
        //I would put this in uppercase and not lowercase.
        public string alias { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string hostname { get; set; }
        public string port { get; set; }
        public string path { get; set; }
        public string label { get; set; }
        public ClientType ManagerClientType { get; set; }
    }

    public enum ClientType
    {
        uTorrent,
        Deluge,
        Transmission,
        Qbittorrent,
        Vuze
    }
}