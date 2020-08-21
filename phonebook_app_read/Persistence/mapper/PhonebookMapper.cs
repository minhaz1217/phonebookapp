using phonebook_app_read.Persistence.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app_read.Persistence.mapper
{
    public class PhonebookMapper
    {
        public static PhonebookReadName PhonebookToPhonebookReadName(Phonebook phonebook)
        {
            return new PhonebookReadName( phonebook.Name, phonebook.Number );
        }
    }
}
