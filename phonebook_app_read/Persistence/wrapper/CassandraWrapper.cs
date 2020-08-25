using Cassandra;
using Cassandra.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookRead.Persistence.wrapper
{
    public class CassandraWrapper : ICassandraWrapper
    {

        private static string SERVER_NAME;
        private static string KEYSPACE_NAME;
        private static Cluster cluster = null;
        private static Session session = null;
        private static IMapper mapper = null;
        public CassandraWrapper(string serverName, string keySpaceName)
        {
            SERVER_NAME = serverName;
            KEYSPACE_NAME = keySpaceName;
            if (cluster == null)
            {
                cluster = (Cluster)Cluster.Builder().AddContactPoints(SERVER_NAME).Build();
            }
            if (session == null)
            {
                session = (Session)cluster.Connect(KEYSPACE_NAME);
            }
            mapper = new Mapper(session);
        }

        public IEnumerable<T> GetAll<T>(string query, params object[] args)
        {
            IEnumerable<T> items = mapper.Fetch<T>(query, args);
            return items;
        }


        public bool Create<T>(T auxPhonebook)
        {
            mapper.Insert(auxPhonebook);
            return true;
        }

        public bool Update<T>(string cql, params object[] args)
        {
            mapper.Update<T>(cql, args);
            return true;
        }


        public bool Delete<T>(string cql, params object[] args)
        {
            //phonebook_practice_app.Helper.Print($"DeletePhonebook => {auxPhonebookId.ToString()}");
            mapper.Delete<T>(cql,args);
            //mapper.Delete(auxPhonebookId);
            return true;
        }



        public bool TableExists(string tableName)
        {

            //phonebook_practice_app.Utils.Print("REACHED " + tableName);
            PreparedStatement ps = session.Prepare($"SELECT count(table_name) as count FROM system_schema.tables WHERE keyspace_name=? and table_name = ?;");
            BoundStatement statement = ps.Bind(KEYSPACE_NAME, tableName);
            RowSet rowSet = session.Execute(statement);
            foreach (Row row in rowSet)
            {
                return row.GetValue<long>("count") > 0 ? true : false;
            }
            //session.Execute($"SELECT count(table_name) as count FROM system_schema.tables WHERE keyspace_name='{KEYSPACE_NAME}' and table_name = '{tableName}';");
            return false;
        }
        public bool CreateTable(string query)
        {
            try
            {
                RowSet rowSet = session.Execute(query);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        ~CassandraWrapper()
        {
            session.Dispose();
            cluster.Dispose();
        }
    }
}
