using Assignment_2._2.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_2._2
{
    [Table("Persons")]
    class Person
    {
        public Person(int id, string name)
        {
            Id = id;
            Name = name;
        }        
        [Column("ID",primaryKey =true)]
        public int Id { get; set; }

        [Column("NAME",unique =true)]
        public string Name { get; set; }
    }
}
