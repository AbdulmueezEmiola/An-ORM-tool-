using System;
using System.Collections.Generic;
using System.Text;

namespace SQLGenerator.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Method)]
    public class OneToMany: System.Attribute
    {
    }
}
