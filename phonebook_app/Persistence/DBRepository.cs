using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app.Persistence
{
    public class DBRepository : IDBRepository
    {
        NpgsqlConnection connection = null;

        public DBRepository(string connectionString)
        {
            this.connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }
        public bool CreateTable(string query)
        {
            try
            {
                this.connection.Execute(query);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public bool TableExists(string tableName)
        {
            var res = connection.Query<bool>($"select exists(SELECT * FROM information_schema.tables where table_name = '{tableName}'); ");
            foreach(bool exists in res)
            {
                return exists;
            }
            return false;
        }

        ~DBRepository()
        {
            if(this.connection != null)
            {
                this.connection.Close();
            }
        }
    }
}
