using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app.Persistence
{
    interface IDBRepository
    {
        bool TableExists(string tableName);
        bool CreateTable(string query);
    }
}
