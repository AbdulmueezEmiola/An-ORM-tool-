using System;
using System.Collections.Generic;
using System.Text;

namespace SQLGenerator.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Method)]
    public class Column : System.Attribute
    {
        public bool NotNull { get; set; }
        public bool PrimaryKey { get; set; }
        public bool Unique { get; set; }
        public string Name { get; set; }
        public Column(string name)
        {
            this.Name = name;
        }
    }
}
