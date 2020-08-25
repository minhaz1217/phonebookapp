﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using phonebook_app.Persistence.wrapper;

namespace Phonebook_Practice_App.model
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
