using PhonebookRead.Persistence.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookRead.Service
{
    interface IPhonebookReadService
    {
        IEnumerable<PhonebookReadName> GetAll();
    }
}
