using PhonebookRead.Persistence.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookRead.Persistence.mapper
{
    public class PhonebookMapper
    {
        public static PhonebookReadName PhonebookToPhonebookReadName(Phonebook phonebook)
        {
            return new PhonebookReadName( phonebook.Name, phonebook.Number );
        }
    }
}
