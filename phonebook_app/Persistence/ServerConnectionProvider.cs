using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace phonebook_practice_app.Persistence
{
    public class ServerConnectionProvider : IServerConnectionProvider
    {
        NpgsqlConnection connection = null;
        string _connectionString = "";
        public ServerConnectionProvider(string connectionString)
        {
            this._connectionString = connectionString;
        }
        public NpgsqlConnection GetConnection()
        {
            //Console.WriteLine("HELLO WORLD "+this._connectionString);
            if(this.connection == null)
            {
                this.connection = new NpgsqlConnection(this._connectionString);
                this.connection.Open();

            }

            return this.connection;
        }
        ~ServerConnectionProvider()
        { 
            if(this.connection != null)
            {
                this.connection.Close();
            }
        }
    }
}
