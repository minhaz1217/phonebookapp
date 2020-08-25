using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app_read.Persistence.wrapper
{
    public interface ICassandraWrapper
    {
        bool TableExists(string tableName);
        bool CreateTable(string query);
        IEnumerable<T> GetAll<T>(string query, params object[] args);
        bool Create<T>(T item);
        bool Update<T>(string cql, params object[] args);
        bool Delete<T>(string cql, params object[] args);
    }
}
