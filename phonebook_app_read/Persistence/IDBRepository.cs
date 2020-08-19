using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app_read.Persistence
{
    interface IDBRepository : IPhonebookRepository, IPhonebookReadNameRepository
    {
        bool TableExists(string tableName);
        bool CreateTable(string query);
    }
}
