﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookRead.Service.DTO
{
    public class PhonebookDTO
    {
        string Name { get; set; }
        string Number { get; set; }
        public PhonebookDTO(string name, string number)
        {
            this.Name = name;
            this.Number = number;
        }

    }
}
