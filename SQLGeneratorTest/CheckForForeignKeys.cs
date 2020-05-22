using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace SQLGeneratorTest
{
    [TestClass]
    public class CheckForForeignKeys:BaseForTests
    {
        [TestMethod]
        public void GetForeignKeys()
        {
            SQLiteConnection con = new SQLiteConnection("Data Source = SQLGenerator.sqlite; Version=3;");
            SQLiteCommand cmd;
            SQLiteDataReader dr;
            string query = $"PRAGMA foreign_key_list (PERSONS)";
            con.Open();
            List<string> lists = new List<string>();
            using (cmd = new SQLiteCommand(query, con))
            {
                using (dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var value = dr.GetValue(1);
                        lists.Add(value.ToString());
                    }
                    dr.Close();
                }
            }
            for (int i = 0; i < lists.Count; i++)
            {
                TestContext.WriteLine(lists[i]);
            }
        }
    }
}
