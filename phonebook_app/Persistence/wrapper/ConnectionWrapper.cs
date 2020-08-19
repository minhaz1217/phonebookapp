using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace phonebook_practice_app.Persistence.wrapper
{
    public class ConnectionWrapper : IConnectionWrapper
    {
        private readonly IConfiguration _config;
        private string _connectionString = "";
        NpgsqlConnection connection = null;

        public static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        public ConnectionWrapper(IConfiguration config)
        {
            this._config = config;
            _connectionString = _config.GetValue<string>("DapperConnectionString");
            if(connection == null)
            {
                this.connection = new NpgsqlConnection(this._connectionString);
                this.connection.Open();
            }
        }


        private string GenerateInsertQuery<T>(T item)
        {
            string tableName = typeof(T).Name.ToString().ToLower();
            var insertQuery = new StringBuilder($"INSERT INTO {tableName} ");
            Dictionary<string, dynamic> myDict = new Dictionary<string, dynamic>();
            var columnPart = new StringBuilder("(");
            var valuesPart = new StringBuilder();
            // (from prop in listOfProperties let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
            //  where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore" select prop.Name
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                var attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if(attributes.Length <=0 || ((attributes[0] as DescriptionAttribute)?.Description != "ignore"))
                {
                    object propValue = prop.GetValue(item, null);
                    myDict[prop.Name] = propValue;
                    columnPart.Append($"{prop.Name},");
                    valuesPart.Append(propValue.GetType() == typeof(string) ? $"'{propValue}'" : propValue);
                    valuesPart.Append(",");
                }
            }
            columnPart.Remove(columnPart.Length - 1, 1).Append(") VALUES (");
            valuesPart.Remove(valuesPart.Length - 1, 1).Append(");");
            insertQuery.Append(columnPart);
            insertQuery.Append(valuesPart);
            return insertQuery.ToString();
        }

        public bool Create<T>(T item)
        {
            if(connection != null)
            {
                var insertQuery = GenerateInsertQuery<T>(item);
                Utils.Print(insertQuery);
                try
                {
                    var x = connection.Execute(insertQuery);
                    Utils.Print($"INSERTED {x.ToString()}");
                    if (x > 0)
                    {
                        return true;
                    }
                }catch(Exception e)
                {
                    Utils.Print(e.Message);
                }
            }
            return false;
        }

        public bool Delete<T>(T item)
        {
            if(this.connection != null)
            {
                string tableName = typeof(T).Name.ToString().ToLower();
                StringBuilder stringBuilder = new StringBuilder($"delete from {tableName} where ");
                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    var attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attributes.Length <= 0 || ((attributes[0] as DescriptionAttribute)?.Description == "primary"))
                    {
                        object propValue = prop.GetValue(item, null);
                        stringBuilder.Append($"{prop.Name} = ");
                        stringBuilder.Append(propValue.GetType() == typeof(string) ? $"'{propValue}'" : propValue);
                        stringBuilder.Append(";");
                        int result = this.connection.Execute(stringBuilder.ToString());
                        //Utils.Print("DELTED "+result + " => "  + prop.GetValue(item, null));
                        return true;
                    }
                }
            }
            //Utils.Print("DELETE ERROR ");
            return false;
        }

        public IEnumerable<T> GetAll<T>(string query)
        {
            List<T> myList = new List<T>();
            if (connection != null)
            {
                //query = "select * from phonebook";
                var x = connection.Query<T>(query);
                foreach (var z in x)
                {
                    myList.Add(z);
                }
            }
            return myList;
        }
        private string GenerateUpdateQuery<T>(T item)
        {

            string tableName = typeof(T).Name.ToString().ToLower();
            var updateQuery = new StringBuilder($"UPDATE {tableName} SET ");
            Dictionary<string, dynamic> myDict = new Dictionary<string, dynamic>();
            var wherePart = new StringBuilder(" WHERE ");
            var valuesPart = new StringBuilder();
            // (from prop in listOfProperties let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
            //  where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore" select prop.Name
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                var attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length <= 0 || ((attributes[0] as DescriptionAttribute)?.Description != "ignore"))
                {
                    Utils.Print(attributes.Length.ToString());
                    if( attributes.Length > 0 && (attributes[0] as DescriptionAttribute)?.Description == "primary")
                    {
                        wherePart.Append($"");
                        object propValue = prop.GetValue(item, null);
                        wherePart.Append($"{prop.Name} = ");
                        wherePart.Append(propValue.GetType() == typeof(string) ? $"'{propValue}'" : propValue);
                    }
                    else
                    {
                        object propValue = prop.GetValue(item, null);
                        myDict[prop.Name] = propValue;
                        valuesPart.Append($"{prop.Name} = ");
                        valuesPart.Append(propValue.GetType() == typeof(string) ? $"'{propValue}'" : propValue);
                        valuesPart.Append(",");
                    }
                }
            }
            valuesPart.Remove(valuesPart.Length - 1, 1);
            updateQuery.Append(valuesPart);
            updateQuery.Append(wherePart);
            updateQuery.Append(";");
            return updateQuery.ToString();
        }
        public bool Update<T>(T item)
        {
            if(this.connection != null)
            {
                var x = this.connection.Execute(GenerateUpdateQuery<T>(item));
                Utils.Print($"UPDATED {x}");
                return true;
            }
            //Utils.Print(GenerateUpdateQuery<T>(item));
            return false;
        }
        ~ConnectionWrapper()
        {
            this.connection.Close();
            this.connection.Dispose();
        }
    }
}
