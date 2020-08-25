using Autofac;
using Dapper;
using Npgsql;
using phonebook_practice_app;
using phonebook_practice_app.Persistence;
using phonebook_practice_app.Persistence.wrapper;
using Phonebook_Practice_App.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app.Persistence
{
    public class DBRepository :IDBRepository
    {
        IConnectionWrapper wrapper = null;

        public DBRepository(string connectionString, IConnectionWrapper wrapper)
        {

            this.wrapper = wrapper;
#if DEBUG
            if (!this.wrapper.TableExists("phonebook"))
            {
                this.wrapper.CreateTable("CREATE TABLE phonebook(id text,name text,number text, PRIMARY KEY(id)); ");
            }
#endif
        }
        public IEnumerable<Phonebook> GetAllPhonebooks(string query)
        {
            return (List<Phonebook>)wrapper.GetAll<Phonebook>(query);
        }

        public bool Create(Phonebook phonebook)
        {
            return wrapper.Create<Phonebook>(phonebook);

        }

        public bool Update(Phonebook phonebook)
        {
            return wrapper.Update<Phonebook>(phonebook);
        }

        public bool Delete(Phonebook phonebook)
        {
            return wrapper.Delete<Phonebook>(phonebook);
        }

        ~DBRepository()
        {

        }
    }
}
