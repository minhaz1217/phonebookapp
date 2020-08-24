using Autofac;
using Cassandra;
using Cassandra.Mapping;
using CqlPoco;
using Microsoft.VisualBasic.CompilerServices;
using phonebook_app_read.Persistence.model;
using phonebook_app_read.Persistence.wrapper;
using phonebook_practice_app;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
namespace phonebook_app_read.Persistence
{
    public class CassandraDBRepository : IDBRepository
    {

        public ILifetimeScope container { get; private set; }
        private static CassandraDBRepository instance = null;
        private static ICassandraWrapper wrapper = null;
        private CassandraDBRepository() { }
        public static CassandraDBRepository Instance(ILifetimeScope container)
        {
            if (instance == null)
            {
                instance = new CassandraDBRepository();
                wrapper = container.Resolve<ICassandraWrapper>();
            }
            #if DEBUG
                if (!wrapper.TableExists("phonebookreadname"))
                {
                wrapper.CreateTable("CREATE TABLE phonebookreadname(name text,number text, PRIMARY KEY(name, number));");
                    //Helper.Print("phonebookreadname created");
                }

                if (!wrapper.TableExists("phonebook"))
                {
                wrapper.CreateTable("CREATE TABLE phonebook(id text,name text,number text, PRIMARY KEY(id)); ");
                    //Helper.Print("phonebook created");
                }
            #endif

            return instance;
        }

        public IEnumerable<Phonebook> GetAllPhonebook(string query, params object[] args)
        {
            phonebook_practice_app.Helper.Print($"GetAllPhonebook => {query}");
            IEnumerable<Phonebook> auxPhonebooks= wrapper.GetAll<Phonebook>(query, args);
            return auxPhonebooks;
        }


        public bool CreatePhonebook(Phonebook auxPhonebook)
        {
            phonebook_practice_app.Helper.Print($"CreatePhonebook => {auxPhonebook.ToString()}");
            wrapper.Create(auxPhonebook);
            return true;
        }

        public bool UpdatePhonebook(Phonebook auxPhonebook)
        {
            phonebook_practice_app.Helper.Print($"UpdatePhonebook => {auxPhonebook.ToString()}");
            wrapper.Update<Phonebook>("SET number=?, name = ? WHERE id = ?", auxPhonebook.Number, auxPhonebook.Name, auxPhonebook.Id);
            return true;
        }


        public bool DeletePhonebook(string auxPhonebookId)
        {
            phonebook_practice_app.Helper.Print($"DeletePhonebook => {auxPhonebookId.ToString()}");
            wrapper.Delete<Phonebook>("WHERE id = ?", auxPhonebookId);
            //mapper.Delete(auxPhonebookId);
            return true;
        }


        public IEnumerable<PhonebookReadName> GetAllPhonebookReadName()
        {
            IEnumerable<PhonebookReadName> phonebookReadNames = wrapper.GetAll<PhonebookReadName>("SELECT * FROM phonebookreadname");
            return phonebookReadNames;
        }

        public bool CreatePhonebookReadName(PhonebookReadName phonebookReadName)
        {
            phonebook_practice_app.Helper.Print($"CreatePhonebookReadName => {phonebookReadName.ToString()}");
            wrapper.Create<PhonebookReadName>(phonebookReadName);
            return true;
        }

        public bool DeletePhonebookReadName(PhonebookReadName phonebookReadName)
        {
            phonebook_practice_app.Helper.Print($"DeletePhonebookReadName => {phonebookReadName.ToString()}");
            wrapper.Delete<PhonebookReadName>("WHERE name = ? and number = ?", phonebookReadName.Name, phonebookReadName.Number);
            //mapper.Delete(auxPhonebookId);
            return true;
        }
        ~CassandraDBRepository() 
        {
        }
    }
}
