using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_2._2.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    class Table: System.Attribute
    {
        public string name;
        public Table(string name)
        {
            this.name = name;
        }
    }
}
