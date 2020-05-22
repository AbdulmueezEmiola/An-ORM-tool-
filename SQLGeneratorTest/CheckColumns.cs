using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;

namespace SQLGeneratorTest
{
    [TestClass]
    public class CheckColumns:BaseForTests
    {
        [TestMethod]
        public void checkColumnAlreadyExists()
        {
            SQLiteConnection con = new SQLiteConnection("Data Source = SQLGenerator.sqlite; Version=3;");
            SQLiteCommand cmd;
            SQLiteDataReader dr;
            string query = $"PRAGMA table_info (PERSONS)";
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
            for(int i = 0; i < lists.Count; i++)
            {
                TestContext.WriteLine(lists[i]);
            }
        }
    }
}
