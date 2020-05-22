using System;
using System.Collections.Generic;
using System.Text;

namespace SQLGenerator.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Method)]
    public class ForeignKey : Column
    {
        public ForeignKey(string name, Type type) : base(name)
        {
            ParentKeyType = type;
        }
        public Type ParentKeyType { get; set; }
    }
}
