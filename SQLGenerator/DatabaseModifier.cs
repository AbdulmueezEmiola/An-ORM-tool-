using SQLGenerator.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SQLGenerator
{
    public static class DatabaseModifier
    {
        private static IDatabaseWrapper dbWrapper;
       
        static DatabaseModifier()
        {
            dbWrapper = new SQLiteWrapper("SQLGenerator.sqlite");
            string query = "PRAGMA foreign_keys = ON";
            dbWrapper.ExecuteCommand(query);
        }
        public static void CreateTable(Type t)
        {            
            string query = QueryGenerator.GenerateCreateQuery(t);
            dbWrapper.ExecuteCommand(query);
        }
        
        public static void AddData(Object obj)
        {            
            try
            {
                QueryGenerator.ProcessOneToManyInsert(obj);
                string query = QueryGenerator.GenerateInsertQuery(obj.GetType());
                Dictionary<string, string> parameters = QueryGenerator.GetValuesInADictionary(obj);
                dbWrapper.ExecuteCommand(query, parameters);
            }catch(SQLiteException e)
            {
                throw new AttributeException("A row containing the same data already exists "+e.Message);
            }
        }
        public static IEnumerable<T> SelectData<T>()
        {
            string query = QueryGenerator.GenerateSelectQuery(typeof(T));
            List<T> Objects = new List<T>();
            using (DbDataReader dataReader = dbWrapper.ReadData(query))
            {
                try
                {
                    Objects = ReadAllDataReader<T>(dataReader).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dataReader.Close();
                }
            }
            return Objects;
        }
        private static IEnumerable<T> ReadAllDataReader<T>(DbDataReader dataReader)
        {
            List<T> Objects = new List<T>();
            while (dataReader.Read())
            {
                Objects.Add(MapDataReaderToObject<T>(dataReader));
            }
            return Objects;
        }
        private static T MapDataReaderToObject<T>(DbDataReader reader)
        {
            T data = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in data.GetType().GetProperties())
            {
                Column colAttr = (Column)Attribute.GetCustomAttribute(prop, typeof(Column));
                if(colAttr != null)
                {
                    if (!object.Equals(reader[colAttr.Name], DBNull.Value))
                    {
                        var value = reader[colAttr.Name];
                        if(value == null)
                        {
                            continue;
                        }
                        var objectInRightType = Convert.ChangeType(reader[colAttr.Name], prop.PropertyType);
                        prop.SetValue(data, objectInRightType);
                    }
                }
            }
            return data;
        }
        public static void DeleteItemFromDatabase(Object obj)
        {
            string query = QueryGenerator.GenerateDeleteQuery(obj.GetType());
            Dictionary<string, string> parameters = QueryGenerator.GetValuesInADictionary(obj);
            dbWrapper.ExecuteCommand(query, parameters);
        }

        public static void DeleteAll(Type t)
        {
            string query = QueryGenerator.GenerateDeleteAllQuery(t);
            dbWrapper.ExecuteCommand(query);
        }

        public static void UpdateItem(Object oldObject, Object newObject)
        {
            if (oldObject.GetType() != newObject.GetType())
            {
                throw new AttributeException("The type of the current object and the new object doesn't match");
            }
            string query = QueryGenerator.GenerateUpdateQuery(oldObject.GetType());
            Dictionary<string, string> parameters = QueryGenerator.GetValuesInADictionary(oldObject, newObject);
            dbWrapper.ExecuteCommand(query, parameters);
        }
    }
}
