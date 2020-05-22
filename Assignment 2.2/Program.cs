using System;
using System.Collections.Generic;

namespace Assignment_2._2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Person> persons = new List<Person>();
            persons.Add(new Person(1, "Jack"));
            persons.Add(new Person(12, "Abdul"));
            SqlGenerator.GetAttribute(persons);
        }
    }
}
