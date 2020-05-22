using Assignment_2._2.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
namespace Assignment_2._2
{
    static class SqlGenerator
    {
        private static List<string> columnHeaders;
        public static List<string> GetAttribute(IList list)
        {
            List<string> lists = new List<string>();
            lists.Add(GetCreateMethod(list));
            lists.AddRange(GetAdderMethod(list));
            return lists;
        }        
        private static List<string> GetAdderMethod(IList list)
        {
            List<string> retList = new List<string>();
            var t = list.GetType().GetGenericArguments()[0];
            Table tableAttribute = (Table)Attribute.GetCustomAttribute(t, typeof(Table));
            if (tableAttribute == null)
            {
                throw new AttributeException("No attribute in class");
            }
            string template = "insert into " + tableAttribute.name + "(" + string.Join(",", columnHeaders.ToArray()) + ")VALUES("; 
            foreach(object obj in list)
            {
                string value = "";
                PropertyInfo[] properties = obj.GetType().GetProperties();                
                for(int i =0;i<properties.Length;++i)
                {
                    value += i==0?properties[i].GetValue(obj).ToString():", "+ properties[i].GetValue(obj).ToString();
                }
                string adder = template + value + ")";
                retList.Add(adder);
            }
            return retList;
        }
        private static string  GetCreateMethod(IEnumerable list)
        {
            var t = list.GetType().GetGenericArguments()[0];
            columnHeaders = new List<string>();
            Table tableAttribute = (Table)Attribute.GetCustomAttribute(t, typeof(Table));
            if (tableAttribute == null)
            {
                throw new AttributeException("No attribute in class");
            }
            string createTable = "create table " + tableAttribute.name + "(";
            PropertyInfo[] myMembers = t.GetProperties();
            for (int i = 0; i < myMembers.Length; i++)
            {
                Column colAttr = (Column)Attribute.GetCustomAttribute(myMembers[i], typeof(Column));
                createTable += i == 0 ? colAttr.name + " " : "," + colAttr.name + " ";
                createTable += getTypeOfProperty(myMembers[i].PropertyType.ToString()) + " ";
                createTable += colAttr.primaryKey == true ? "PRIMARY KEY" : "";
                createTable += colAttr.NotNull == true ? "NOT NULL" : "";
                createTable += colAttr.unique == true ? "UNIQUE" : "";
                columnHeaders.Add(colAttr.name);
            }
            createTable += ")";
            return createTable;
        }
        private static string getTypeOfProperty(string t)
        {
            switch (t)
            {
                case "System.Int32":
                    return "INTEGER";
                case "System.String":
                    return "VARCHAR";
                case "System.Decimal":
                    return "DECIMAL";
                default:
                    return "VARCHAR";
            }
        }
        public static Type GetEnumeratedType<T>(this IEnumerable<T> _)
        {
            return typeof(T);
        }
    }
}
