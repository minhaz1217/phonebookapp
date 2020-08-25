using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PhonebookWrite.Persistence.wrapper;

namespace PhonebookWrite.model
{
    public class Phonebook
    {
        const string TABLE_NAME = "phonebook";
        [WrapperTable("primary")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }

        public override string ToString()
        {
            return $"({Id}) : {Name}({Number})";
        }
    }
}
