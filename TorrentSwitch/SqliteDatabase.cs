using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace TorrentSwitch
{
    static class SqliteDatabase
    {
        private static string getPath()
        {
            string programPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            programPath = programPath.Substring(0, programPath.Length - 17);

            return programPath;
        }

        public static void CheckForDatabase()
        {
            if (!File.Exists(getPath() + "settings.sqlite"))
            {
                createDatabase();
            }
            else
            {
                Debug.WriteLine("Database Exists");
            }
        }

        private static void createDatabase()
        {
            SQLiteConnection.CreateFile(getPath() + "settings.sqlite");
            var m_create_structure = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            m_create_structure.Open();
            string sql = "create table managers " +
             "(alias varchar(20), " +
             "hostname varchar(20), " +
             "port varchar(20), " +
             "username varchar(20), " +
             "password varchar(20), " +
             "type varchar(20), " +
             "path varchar(20), " +
             "label varchar(20))";
            SQLiteCommand command = new SQLiteCommand(sql, m_create_structure);
            command.ExecuteNonQuery();
            m_create_structure.Close();
        }
        
        public static void AddEntry(string alias, string hostname, string port, string username, string password, string type, string path, string label)
        {
            var m_write_entry = new SQLiteConnection("Data Source=" + getPath() + "settings.sqlite;Version=3;");
            m_write_entry.Open();
            string sql = "insert into managers (alias, hostname, port, username, password, type, path, label)  values " +
                         "('" + alias + "', " +
                         "'" + hostname + "'," +
                         "'" + port + "'," +
                         "'" + username+ "'," +
                         "'" + password + "'," +
                         "'" + type + "'," +
                         "'" + path + "'," +
                         "'" + label + "')";
            SQLiteCommand command = new SQLiteCommand(sql, m_write_entry);
            command.ExecuteNonQuery();
            m_write_entry.Close();
        }

        public static void RemoveEntry(string alias)
        {
            var m_write_entry = new SQLiteConnection("Data Source=" + getPath() + "settings.sqlite;Version=3;");
            m_write_entry.Open();
            string sql = "DELETE FROM managers WHERE alias = '" + alias + "'; ";
            SQLiteCommand command = new SQLiteCommand(sql, m_write_entry);
            command.ExecuteNonQuery();
            m_write_entry.Close();
        }

        public static void LoadDatabase() 
        {
            var m_read_entry = new SQLiteConnection("Data Source=" + getPath() + "settings.sqlite;Version=3;");
            m_read_entry.Open();
            string sql = "select * from managers order by alias desc";
            SQLiteCommand command = new SQLiteCommand(sql, m_read_entry);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                MainWindow.ColumnLoader(reader["alias"].ToString());
                torrentClients.ClientType client_type = (torrentClients.ClientType)Enum.Parse(typeof(torrentClients.ClientType), reader["type"].ToString());

                torrentClients.client.AddUser(
                    reader["alias"].ToString(),
                    reader["hostname"].ToString(),
                    reader["port"].ToString(),
                    reader["username"].ToString(),
                    reader["password"].ToString(),
                    client_type,
                    reader["path"].ToString(),
                    reader["label"].ToString());
            }
        }
    }
}
