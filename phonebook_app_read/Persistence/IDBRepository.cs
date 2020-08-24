using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app_read.Persistence
{
    interface IDBRepository : IPhonebookRepository, IPhonebookReadNameRepository
    {
    }
}
