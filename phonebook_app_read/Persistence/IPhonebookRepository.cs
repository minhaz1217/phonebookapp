using phonebook_app_read.Persistence.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app_read.Persistence
{
    // This is the interface for the table AuxPhonebook where we keep demo data similar to the write table with (id, name, number)
    interface IPhonebookRepository
    {
        IEnumerable<Phonebook> GetAllPhonebook(string query, params object[] args);
        bool CreatePhonebook(Phonebook auxPhonebook);
        bool UpdatePhonebook(Phonebook auxPhonebook);
        bool DeletePhonebook(string auxPhonebookId);
    }
}
