using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Phonebook_Practice_App.model
{
    public class Phonebook
    {
        const string TABLE_NAME = "phonebook";
        [Description("primary")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }

        public override string ToString()
        {
            return $"({Id}) : {Name}({Number})";
        }
    }
}
