using SQLGenerator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLGeneratorTest
{

    [Table("Persons")]
    class Person
    {
        public Person(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public Person()
        {

        }
        [Column("ID", PrimaryKey = true)]
        public int Id { get; set; }

        [Column("NAME", Unique = true)]
        public string Name { get; set; }

        [ForeignKey("Department_Ids",typeof(Department))]
        public int departmentId { get; set; }
        public bool Equals(Person person)
        {
            return person.Id == this.Id && person.Name == this.Name;
        }
    }
}
