using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLGeneratorTest
{
    [TestClass]
    public class UpdateDatabaseTest
    {
        [TestMethod]
        public void UpdateItemInADatabase()
        {
            Person oldPerson = new Person()
            {
                Id = 410,
                Name = "Update410"
            };
            //DatabaseModifier.AddData(oldPerson);
            Person newPerson = new Person()
            {
                Id = 460,
                Name = "Update460",
                departmentId = 460
            };
            //List<Person> lists = DatabaseModifier.SelectData<Person>().ToList();
            DatabaseModifier.UpdateItem(oldPerson, newPerson);
            //List<Person> lists2 = DatabaseModifier.SelectData<Person>().ToList();
            //Assert.AreNotEqual(lists, lists2);
        }
    }
}
