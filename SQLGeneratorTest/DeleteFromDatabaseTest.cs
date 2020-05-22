using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLGeneratorTest
{
    [TestClass]
    public class DeleteFromDatabaseTest:BaseForTests
    {

        [TestMethod]
        public void TestDeleteMethod()
        {
            Person person = new Person(104, "Delete104");
            DatabaseModifier.AddData(person);
            int count = CountItemsInDatabase();
            DatabaseModifier.DeleteItemFromDatabase(person);
            int countAfter = CountItemsInDatabase();
            Assert.AreEqual(count - 1, countAfter);
        }
        [TestMethod]
        public void TestDeleteMethodWhereItemDoesntExist()
        {
            Person person = new Person(203, "Delete203");
            int count = CountItemsInDatabase();
            DatabaseModifier.DeleteItemFromDatabase(person);
            int countAfter = CountItemsInDatabase();
            Assert.AreEqual(count, countAfter);
        }
        [TestMethod]
        public void DeleteAllItemsInADatabase()
        {
            DatabaseModifier.DeleteAll(typeof(Person));
            int count = CountItemsInDatabase();
            Assert.AreEqual(0, count);
        }
    }
}
