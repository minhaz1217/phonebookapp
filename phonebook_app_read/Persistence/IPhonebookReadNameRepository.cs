using PhonebookRead.Persistence.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookRead.Persistence
{
    public interface IPhonebookReadNameRepository
    {

        IEnumerable<PhonebookReadName> GetAllPhonebookReadName();
        bool CreatePhonebookReadName(PhonebookReadName phonebookReadName);
        bool DeletePhonebookReadName(PhonebookReadName phonebookReadName);
    }
}
