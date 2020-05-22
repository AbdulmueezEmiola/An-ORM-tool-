using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLGeneratorTest
{
    [TestClass]
    public class AddToDatabaseTest:BaseForTests
    {

        [TestMethod]
        public void TestCreateMethod()
        {
            DatabaseModifier.CreateTable(typeof(Department));
            DatabaseModifier.CreateTable(typeof(Person));
            List<string> tablesInDatabase = CheckTablesInDatabase();
            for (int i = 0; i < tablesInDatabase.Count; i++)
            {
                TestContext.WriteLine(tablesInDatabase[i]);
            }
            CollectionAssert.Contains(tablesInDatabase, "PERSONS");
        }
    }
}
