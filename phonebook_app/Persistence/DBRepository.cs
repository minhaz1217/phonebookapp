using Autofac;
using Dapper;
using Npgsql;
using phonebook_practice_app;
using phonebook_practice_app.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app.Persistence
{
    public class DBRepository : IDBRepository
    {
        NpgsqlConnection connection = null;

        public DBRepository(ILifetimeScope container)
        {
            if(this.connection == null)
            {

            this.connection = container.Resolve<IServerConnectionProvider>().GetConnection();

            #if DEBUG
                            Helper.Print("CONNECTION STARTING");
                            if (!this.TableExists("phonebook"))
                            {
                                Helper.Print("TABLE MADE");
                                this.CreateTable("CREATE TABLE phonebook(id text,name text,number text, PRIMARY KEY(id)); ");
                            }
            #endif
            }
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
