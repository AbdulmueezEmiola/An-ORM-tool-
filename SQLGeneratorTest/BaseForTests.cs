using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLGenerator;
using SQLGenerator.Attributes;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
namespace SQLGeneratorTest
{
    [TestClass]
    public class BaseForTests
    {
        private TestContext testContextInstance;        
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }                

        [TestInitialize]
        public void initializeForTests()
        {
            DatabaseModifier.CreateTable(typeof(Department));
            DatabaseModifier.CreateTable(typeof(Person));
        }
        public int CountItemsInDatabase()
        {
            SQLiteConnection con = new SQLiteConnection("Data Source = SQLGenerator.sqlite; Version=3;");
            SQLiteCommand cmd;
            SQLiteDataReader dr;
            con.Open();
            int value = 0;
            string query = "select Count(*) from PERSONS";
            using (cmd = new SQLiteCommand(query, con))
            {
                using (dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        value = int.Parse(dr[0].ToString());
                    }
                }
            }
            return value;
        }
        public List<string> CheckTablesInDatabase()
        {
            SQLiteConnection con = new SQLiteConnection("Data Source = SQLGenerator.sqlite; Version=3;");
            SQLiteCommand cmd;
            SQLiteDataReader dr;
            List<string> values = new List<string>();
            con.Open();
            string query = "select name from sqlite_master where type = 'table'";
            using (cmd = new SQLiteCommand(query, con))
            {
                using (dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        values.Add(dr["name"].ToString());
                    }
                }
            }
            return values;
        }
    }
    
}
