﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using System.Reflection;

namespace TorrentSwitch
{
    static class SqliteDatabase
    {
        public static void check_for_database()
        {

            if (!File.Exists("settings.sqlite"))
            {
                create_database();
            }
            else
            {
                Debug.WriteLine("Database Exists");
            }

        }

        public static void create_database()
        {
            SQLiteConnection.CreateFile("settings.sqlite");
            var m_create_structure = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            m_create_structure.Open();
            string sql = "create table managers " +
             "(ip varchar(20), " +
             "port varchar(20), " +
             "username varchar(20), " +
             "password varchar(20), " +
             "alias varchar(20), " +
             "type varchar(20), " +
             "path varchar(20), " +
             "label varchar(20))";
            SQLiteCommand command = new SQLiteCommand(sql, m_create_structure);
            command.ExecuteNonQuery();
            m_create_structure.Close();
        }

        public static void load_files()
        {
            var m_load_structure = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            m_load_structure.Open();
            string sql = "select * from managers order by name desc";
            
            SQLiteCommand command = new SQLiteCommand(sql, m_load_structure);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tuser: " + reader["user"]);
        }

        public static void add_entry(string ip, string port, string username, string password, string alias, string type, string path, string label)
        {
            var m_write_entry = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            m_write_entry.Open();
            string sql = "insert into managers (ip, port , username, password , alias , type, path, label)  values " +
                         "('" + ip + "', " +
                         "'" + port + "'," +
                         "'" + username + "'," +
                         "'" + password + "'," +
                         "'" + alias + "'," +
                         "'" + type + "'," +
                         "'" + path + "'," +
                         "'" + label + "')";
            SQLiteCommand command = new SQLiteCommand(sql, m_write_entry);
            command.ExecuteNonQuery();
            m_write_entry.Close();
        }

        public static void remove_entry(string name)
        {
            var m_write_entry = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            m_write_entry.Open();
            string sql = "DELETE FROM managers WHERE name = '"+ name +"'; ";
            SQLiteCommand command = new SQLiteCommand(sql, m_write_entry);
            command.ExecuteNonQuery();
            m_write_entry.Close();
        }

        public static void load_database() 
        {
            var m_read_entry = new SQLiteConnection("Data Source=settings.sqlite;Version=3;");
            m_read_entry.Open();
            string sql = "select * from managers order by alias desc";
            SQLiteCommand command = new SQLiteCommand(sql, m_read_entry);
            SQLiteDataReader reader = command.ExecuteReader();
            settings_window window = new settings_window();
            while (reader.Read())
            {
                Debug.WriteLine("\nalias: " + reader["alias"] + "\nuser: " + reader["username"]);
                string status = "On";
                string host = reader["alias"] + "//" + reader["username"] + "@" + reader["ip"] + ":" + reader["port"];
                //string client_type = reader["type"];
                string client_type = "uTorrent";
                new settings_window.manager_data { status = status, host = host, client_type = client_type };


                window.AddManager(status, host, host);
            }

        }
    }
}
