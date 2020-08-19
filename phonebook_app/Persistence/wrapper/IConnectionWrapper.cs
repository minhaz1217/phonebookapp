using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_practice_app.Persistence.wrapper
{
    public interface IConnectionWrapper
    {
        IEnumerable<T> GetAll<T>(string query);
        bool Create<T>(T item);
        bool Update<T>(T item);
        bool Delete<T>(T item);
    }
}
