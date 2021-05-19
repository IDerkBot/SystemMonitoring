using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SystemMonitoring
{
    class Database
    {
        public static MySqlConnection conn;
        static MySqlConnectionStringBuilder db;
        public static MySqlConnection Connect()
        {
            db = new MySqlConnectionStringBuilder
            {
                Server = DataBank.Host,
                Database = DataBank.Db,
                UserID = DataBank.User,
                Password = DataBank.Password,
                Port = DataBank.Port,
                CharacterSet = "utf8"
            };
            conn = new MySqlConnection(db.ConnectionString);
            try { conn.Open(); }
            catch (Exception ex)
            { MessageBox.Show("Проблемы с подключением к БД \n\r" + ex.ToString()); }
            return conn;
        }
        public struct Person {
            public int id;
            public string login, password, access;
        }
        public static List<Person> GetUsers(string where = null){
            var list = new List<Person>();
            Person data;
            var cmd = new MySqlCommand($"select * from `users` {where}", Connect());
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.id = int.Parse(reader["id"].ToString());
                data.login = reader["login"].ToString();
                data.password = reader["password"].ToString();
                data.access = reader["access"].ToString();
                list.Add(data);
            }
            return list;
        }
        public static List<Person> GetUserData(string login)
        {
            var list = new List<Person>();
            Person data;
            var cmd = new MySqlCommand($"select * from `users` where `login` = '{login}'", Connect());
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.id = int.Parse(reader["id"].ToString());
                data.login = reader["login"].ToString();
                data.password = reader["password"].ToString();
                data.access = reader["access"].ToString();
                list.Add(data);
            }
            return list;
        }
        public static string[] GetTableOrgan(string where = null)
        {
            MySqlCommand cmd = new MySqlCommand($"select * from `organization` {where}", Connect());
            string[] User = new string[7];
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                User[0] = reader["id"].ToString();
                User[1] = reader["name"].ToString();
                User[2] = reader["number"].ToString();
                User[3] = reader["position"].ToString();
                User[4] = reader["address"].ToString();
                User[5] = reader["area"].ToString();
                User[6] = reader["type"].ToString();
            }
            reader.Close();
            return User;
        }
    }
}
