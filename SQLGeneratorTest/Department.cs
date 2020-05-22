using SQLGenerator.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLGeneratorTest
{
    [Table("OFFICE")]
    class Department
    {
        [Column("ID",PrimaryKey =true)]
        public int Id { get; set; }

        [OneToMany]
        public List<Person> persons { get; set; }
    }
}
