using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace SQLGenerator
{
    class SQLiteWrapper:IDatabaseWrapper
    {
        private SQLiteConnection connection;
        private SQLiteCommand command;
        private SQLiteDataReader dataReader;
        public SQLiteWrapper(string sqlFile)
        {
            if (!File.Exists(sqlFile))
            {
                SQLiteConnection.CreateFile(sqlFile);
            }
            connection = new SQLiteConnection("Data Source = SQLGenerator.sqlite; Version=3;");
            SQLiteCommandBuilder builder = new SQLiteCommandBuilder();           
        }
        public void ExecuteCommand(string query)
        {
            try {
                connection.Open();
                command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }
        public void ExecuteCommand(string query, Dictionary<string,string> values)
        {

            try {
                connection.Open();
                command = new SQLiteCommand(query, connection);
                foreach (var item in values)
                {
                    command.Parameters.AddWithValue(item.Key, item.Value);
                }
                command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }
        public DbDataReader ReadData(string query)
        {
            connection.Open();
            command = new SQLiteCommand(query, connection);
            dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return dataReader;
        }
    }
}
