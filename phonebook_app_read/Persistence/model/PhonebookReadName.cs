using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app_read.Persistence.model
{
    [ElasticsearchType(IdProperty = nameof(Name))]
    public class PhonebookReadName
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public PhonebookReadName(string name, string number)
        {
            this.Name = name;
            this.Number = number;
        }
        public override string ToString()
        {
            return $"{this.Name}({this.Number})";
        }
    }
}
