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

        public Phonebook GetById(string id)
        {
            //PreparedStatement ps = session.Prepare($"SELECT * FROM {Product.TABLE_NAME} where {Product.COL_ID}=?;");
            //BoundStatement statement = ps.Bind(productId.ToString());
            //RowSet rowSet = session.Execute(statement);
            //foreach (Row row in rowSet)
            //{
            //    return CassandraMapper.DBProductMapper(row);
            //}
            throw new NotImplementedException();
        }

        public IEnumerable<Phonebook> GetAllPhonebook(string query, params object[] args)
        {
            phonebook_practice_app.Utils.Print($"GetAllPhonebook => {query}");
            IEnumerable<Phonebook> auxPhonebooks= mapper.Fetch<Phonebook>(query, args);
            return auxPhonebooks;
        }

        public Phonebook GetSinglePhonebook(string auxPhonebookId)
        {
            throw new NotImplementedException();
        }

        public bool CreatePhonebook(Phonebook auxPhonebook)
        {
            phonebook_practice_app.Utils.Print($"CreatePhonebook => {auxPhonebook.ToString()}");
            mapper.Insert(auxPhonebook);
            return true;
        }

        public bool UpdatePhonebook(Phonebook auxPhonebook)
        {
            phonebook_practice_app.Utils.Print($"UpdatePhonebook => {auxPhonebook.ToString()}");
            mapper.Update<Phonebook>("SET number=?, name = ? WHERE id = ?", auxPhonebook.Number, auxPhonebook.Name, auxPhonebook.Id);
            return true;
        }


        public bool DeletePhonebook(string auxPhonebookId)
        {
            phonebook_practice_app.Utils.Print($"DeletePhonebook => {auxPhonebookId.ToString()}");
            mapper.Delete<Phonebook>("WHERE id = ?", auxPhonebookId);
            //mapper.Delete(auxPhonebookId);
            return true;
        }

        public long CheckPhonebookCount(string auxPhonebookId)
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
            phonebook_practice_app.Utils.Print($"CreatePhonebookReadName => {phonebookReadName.ToString()}");
            mapper.Insert(phonebookReadName);
            return true;
        }

        public bool DeletePhonebookReadName(PhonebookReadName phonebookReadName)
        {
            phonebook_practice_app.Utils.Print($"DeletePhonebookReadName => {phonebookReadName.ToString()}");
            mapper.Delete<PhonebookReadName>("WHERE name = ? and number = ?", phonebookReadName.Name, phonebookReadName.Number);
            //mapper.Delete(auxPhonebookId);
            return true;
        }
    }
}
