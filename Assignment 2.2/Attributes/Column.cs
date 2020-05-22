using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_2._2.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property|System.AttributeTargets.Method)]
    class Column: System.Attribute
    {
        public bool NotNull;
        public bool primaryKey;
        public bool unique;
        public string name;
        public Column(string name)
        {
            this.name = name;
        }
    }
}
