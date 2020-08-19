using phonebook_app_read.Persistence.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app_read.Persistence
{
    // This is the interface for the table AuxPhonebook where we keep demo data similar to the write table with (id, name, number)
    interface IAuxPhonebookRepository
    {
        AuxPhonebook GetById(string id);
        IEnumerable<AuxPhonebook> GetAllAuxPhonebook();
        bool CreateAuxPhonebook(AuxPhonebook auxPhonebook);
        bool UpdateAuxPhonebook(AuxPhonebook auxPhonebook);
        bool DeleteAuxPhonebook(string auxPhonebookId);
        AuxPhonebook GetSingleAuxPhonebook(string auxPhonebookId);
        long CheckAuxPhonebookCount(string auxPhonebookId);
    }
}
