using phonebook_app_read.Persistence.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app_read.Service
{
    interface IPhonebookReadService
    {
        IEnumerable<PhonebookReadName> GetAll();
    }
}
