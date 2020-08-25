using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookRead.Persistence
{
    public interface IDBRepository : IPhonebookRepository, IPhonebookReadNameRepository
    {
    }
}
