using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLGeneratorTest
{
    [TestClass]
    public class TestAdder:BaseForTests
    {
        [TestMethod]
        public void TestAdderAndSelectMethod()
        {
            Person person = new Person(39, "Test39");
            person.departmentId = 39;
            int count = CountItemsInDatabase();
            DatabaseModifier.AddData(person);
            int countAfter = CountItemsInDatabase();
            Assert.AreEqual(count + 1, countAfter);
            List<Person> lists = DatabaseModifier.SelectData<Person>().ToList();
            foreach(Person p in lists)
            {
                TestContext.WriteLine(p.departmentId.ToString());
            }
        }
        [TestMethod]
        public void AddToDepartment()
        {
            List<Person> persons = new List<Person>()
            {
                new Person
                {
                    Id=3,
                    Name="name3",
                    departmentId =52
                },
                new Person
                {
                    Id =4,
                    Name = "name4",
                    departmentId =52
                }
            };
            Department department = new Department()
            {
                Id = 52,
                persons = persons
            };
            DatabaseModifier.AddData(department);
        }
    }
}
