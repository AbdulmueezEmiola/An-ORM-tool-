using SQLGenerator.Attributes;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections;

namespace SQLGenerator
{
    static class QueryGenerator
    {
        public static string GenerateCreateQuery(Type t)
        {
            StringBuilder createTable = new StringBuilder("create table if not exists ");
            createTable.Append(GetTableName(t));
            createTable.Append("(");
            createTable.Append(string.Join(", ", GetAllColumnInfo(t).Concat(GetAllForeignKeyInfo(t))));
            createTable.Append(")");
            return createTable.ToString();
        }
        private static IEnumerable<string> GetAllColumnInfo(Type t)
        {
            foreach (PropertyInfo property in t.GetProperties())
            {
                Object[] attributes = property.GetCustomAttributes(false);                
                if(attributes[0] is Column)
                {
                    yield return GetColumnInfo(property);
                }
            }
        }
        private static string GetColumnInfo(PropertyInfo property)
        {
            Column columnAttribute = (Column)Attribute.GetCustomAttribute(property, typeof(Column));
            StringBuilder value = new StringBuilder(columnAttribute.Name);
            value.Append(" ");
            value.Append(GetTypeOfProperty(property.PropertyType));
            value.Append(" ");
            value.Append(CheckColumnProprties(columnAttribute));
            return value.ToString();

        }
        private static string CheckColumnProprties(Column columnAttribute)
        {
            StringBuilder value = new StringBuilder();
            value.Append(columnAttribute.PrimaryKey == true ? " PRIMARY KEY " : "");
            value.Append(columnAttribute.NotNull == true ? " NOT NULL " : "");
            value.Append(columnAttribute.Unique == true ? " UNIQUE " : "");
            return value.ToString();
        }
        private static string GetTypeOfProperty(Type t)
        {
            if (t == typeof(Int32))
            {
                return " INTEGER ";
            }
            else if (t == typeof(String))
            {
                return " VARCHAR2 ";
            }
            else if (t == typeof(Decimal))
            {
                return " DECIMAL";
            }
            else
            {
                return " VARCHAR ";
            }
        }
        private static IEnumerable<string> GetAllForeignKeyInfo(Type t)
        {
            foreach (PropertyInfo property in t.GetProperties())
            {
                Object[] attributes = property.GetCustomAttributes(false);
                if (attributes[0] is ForeignKey)
                {
                    yield return GetForeignKeyInfo(property);
                }
            }
        }
        private static string GetForeignKeyInfo(PropertyInfo property)
        {
            ForeignKey columnAttribute = (ForeignKey)Attribute.GetCustomAttribute(property, typeof(Column));
            StringBuilder value = new StringBuilder($"FOREIGN KEY({columnAttribute.Name})");
            value.Append(" REFERENCES ");
            Type table = columnAttribute.ParentKeyType;
            value.Append(GetTableName(table));
            PropertyInfo column = GetKeyColumn(table);
            value.Append($"({GetColumnName(column)})");
            return value.ToString();
        }
        private static PropertyInfo GetKeyColumn(Type t1)
        {
            foreach (PropertyInfo property in t1.GetProperties())
            {
                Column columnAttribute = (Column)Attribute.GetCustomAttribute(property, typeof(Column));
                if (columnAttribute.PrimaryKey == true)
                {
                    return property;
                }
            }
            throw new AttributeException("The table needs to have a primary key");
        }
        private static string GetColumnName(PropertyInfo property)
        {
            Column columnAttribute = (Column)Attribute.GetCustomAttribute(property, typeof(Column));
            return columnAttribute.Name;
        }
        public static string GenerateInsertQuery(Type t)
        {
            StringBuilder insertQuery = new StringBuilder("insert into ");
            insertQuery.Append(GetTableName(t));
            insertQuery.Append("(");
            insertQuery.Append(String.Join(", ", GetPropertyNames(t)));
            insertQuery.Append(") VALUES(");
            insertQuery.Append(String.Join(", ", GetParametrizedPropertyNames(t)));
            insertQuery.Append(")");
            return insertQuery.ToString();
        }
        
        private static string GetTableName(Type t)
        {
            Table tableAttribute = (Table)Attribute.GetCustomAttribute(t, typeof(Table));
            if (tableAttribute == null)
            {
                throw new AttributeException("No attribute in class");
            }
            return tableAttribute.name.ToUpper();
        }
        private static IEnumerable<string> GetPropertyNames(Type type)
        {
            foreach (PropertyInfo property in type.GetProperties())
            {
                Column colAttr = (Column)Attribute.GetCustomAttribute(property, typeof(Column));
                if(colAttr != null)
                {
                    yield return colAttr.Name;
                }                
            }
        }
        public static void ProcessOneToManyInsert(Object obj)
        {
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                OneToMany colAttr = (OneToMany)Attribute.GetCustomAttribute(property, typeof(OneToMany));
                if (colAttr != null)
                {
                    var values = property.GetValue(obj) as IList;
                    foreach( var value in values)
                    {
                        DatabaseModifier.AddData(value);
                    }
                }
            }
        }
        private static IEnumerable<string> GetParametrizedPropertyNames(Type type)
        {
            IEnumerable<string> columnHeaders = GetPropertyNames(type);
            IEnumerable<string> parameters = columnHeaders.Select(c => { c = '@' + c; return c; });
            return parameters;
        }
        public static Dictionary<string, string> GetValuesInADictionary(params Object[] obj)
        {
            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>();
            IEnumerable<string> parameters = GetParametrizedPropertyNames(obj[0].GetType());
            for(int i = 0; i < obj.Length; ++i)
            {
                IEnumerable<String> values = GetPropertyValues(obj[i]);
                for (int j = 0; j < parameters.Count(); ++j)
                {
                    string key = parameters.ElementAt(j);
                    string keyInCaseOfMultipleObjects = i == 0 ?key: key + i.ToString();
                    parameterDictionary.Add(keyInCaseOfMultipleObjects, values.ElementAt(j));
                }
            }            
            return parameterDictionary;
        }
        private static IEnumerable<string> GetPropertyValues(Object obj)
        {
            var type = obj.GetType();
            foreach (PropertyInfo property in type.GetProperties())
            {               
                if(property.GetValue(obj)!= null)
                {
                    string value = property.GetValue(obj).ToString();                    
                    yield return value;
                }
            }
        }
        public static string GenerateSelectQuery(Type t)
        {
            string selectTable = string.Format("Select * From {0}",GetTableName(t));
            return selectTable;
        }
        public static string GenerateDeleteQuery(Type t)
        {
            StringBuilder deleteTable = new StringBuilder($"Delete from {GetTableName(t)} where ");
            IEnumerable<string> names = GetPropertyNames(t);
            IEnumerable<string> parameterNames = GetParametrizedPropertyNames(t);
            deleteTable.Append(ConditionalPartOfAString(names, parameterNames, "AND"));
            return deleteTable.ToString();
        }
        private static string ConditionalPartOfAString(IEnumerable<string>names,IEnumerable<string> parameterNames,string separator)
        {
            StringBuilder condition = new StringBuilder();
            for (int i = 0; i < names.Count(); ++i)
            {
                if (i != 0)
                {
                    condition.Append(" "+separator+" ");
                }
                condition.Append($"{names.ElementAt(i)} = {parameterNames.ElementAt(i)}");
            }
            return condition.ToString();
        }
        public static string GenerateDeleteAllQuery(Type t)
        {
            return $"Delete from  {GetTableName(t)}";
        }
        public static string GenerateUpdateQuery(Type t)
        {
            StringBuilder updateItem = new StringBuilder($"UPDATE {GetTableName(t)} SET ");         
            IEnumerable<string> names = GetPropertyNames(t);
            IEnumerable<string> parameterNames = GetParametrizedPropertyNames(t);
            updateItem.Append(ConditionalPartOfAString(names, parameterNames, ","));
            updateItem.Append(" WHERE ");
            parameterNames = parameterNames.Select(c => { c = c + '1'; return c; });
            updateItem.Append(ConditionalPartOfAString(names, parameterNames, "AND"));            
            return updateItem.ToString();
        }
    }
}
