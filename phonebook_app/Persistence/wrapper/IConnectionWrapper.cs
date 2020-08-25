using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookWrite.Persistence.wrapper
{
    public interface IConnectionWrapper
    {
        bool TableExists(string tableName);
        bool CreateTable(string query);
        IEnumerable<T> GetAll<T>(string query);
        bool Create<T>(T item);
        bool Update<T>(T item);
        bool Delete<T>(T item);
    }
}
