using Cassandra;
using Cassandra.Mapping;
using CqlPoco;
using Microsoft.VisualBasic.CompilerServices;
using phonebook_app_read.Persistence.model;
using phonebook_practice_app;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace phonebook_app_read.Persistence
{
    public class CassandraDBRepository : IDBRepository
    {

        private static CassandraDBRepository instance = null;
        private static string SERVER_NAME;
        private static string KEYSPACE_NAME;
        private static Cluster cluster = null;
        private static Session session = null;
        private static IMapper mapper = null;
        private CassandraDBRepository() { }
        public static CassandraDBRepository Instance(string serverName, string keySpaceName)
        {

            // DBController.GetInstance();
            // new DBController();
            if (instance == null)
            {
                instance = new CassandraDBRepository();
                SERVER_NAME = serverName;
                KEYSPACE_NAME = keySpaceName;
                if (cluster == null)
                {
                    cluster = (Cluster)Cluster.Builder().AddContactPoints(SERVER_NAME).Build();
                    session = (Session)cluster.Connect(KEYSPACE_NAME);
                }
                else
                {
                    session = (Session)cluster.Connect(KEYSPACE_NAME);
                }
                mapper = new Mapper(session);

            }
            return instance;
        }
        public bool TableExists(string tableName)
        {

            //phonebook_practice_app.Utils.Print("REACHED " + tableName);
            PreparedStatement ps = session.Prepare($"SELECT count(table_name) as count FROM system_schema.tables WHERE keyspace_name=? and table_name = ?;");
            BoundStatement statement = ps.Bind(KEYSPACE_NAME, tableName);
            RowSet rowSet = session.Execute(statement);
            foreach (Row row in rowSet)
            {
                return row.GetValue<long>("count")>0?true:false;
            }
            //session.Execute($"SELECT count(table_name) as count FROM system_schema.tables WHERE keyspace_name='{KEYSPACE_NAME}' and table_name = '{tableName}';");
            return false;
        }
        public bool CreateTable(string query)
        {
            RowSet rowSet = session.Execute(query);
            return true;
        }

        public AuxPhonebook GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AuxPhonebook> GetAllAuxPhonebook()
        {
            IEnumerable<AuxPhonebook> auxPhonebooks= mapper.Fetch<AuxPhonebook>("SELECT * FROM auxphonebook");
            return auxPhonebooks;
        }

        public AuxPhonebook GetSingleAuxPhonebook(string auxPhonebookId)
        {
            throw new NotImplementedException();
        }

        public bool CreateAuxPhonebook(AuxPhonebook auxPhonebook)
        {
            mapper.Insert(auxPhonebook);
            return true;
        }

        public bool UpdateAuxPhonebook(AuxPhonebook auxPhonebook)
        {
            mapper.Update<AuxPhonebook>("SET number=?, name = ? WHERE id = ?", auxPhonebook.Number, auxPhonebook.Name, auxPhonebook.Id);
            return true;
        }


        public bool DeleteAuxPhonebook(string auxPhonebookId)
        {
            mapper.Delete<AuxPhonebook>("WHERE id = ?", auxPhonebookId);
            //mapper.Delete(auxPhonebookId);
            return true;
        }

        public long CheckAuxPhonebookCount(string auxPhonebookId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PhonebookReadName> GetAllPhonebookReadName()
        {
            IEnumerable<PhonebookReadName> phonebookReadNames = mapper.Fetch<PhonebookReadName>("SELECT * FROM phonebookreadname");
            return phonebookReadNames;
        }

        public bool CreatePhonebookReadName(PhonebookReadName phonebookReadName)
        {
            mapper.Insert(phonebookReadName);
            return true;
        }

        public bool DeletePhonebookReadName(PhonebookReadName phonebookReadName)
        {
            phonebook_practice_app.Utils.Print($"DELETING {phonebookReadName.ToString()}");
            mapper.Delete<PhonebookReadName>("WHERE name = ? and number = ?", phonebookReadName.Name, phonebookReadName.Number);
            //mapper.Delete(auxPhonebookId);
            return true;
        }
    }
}
