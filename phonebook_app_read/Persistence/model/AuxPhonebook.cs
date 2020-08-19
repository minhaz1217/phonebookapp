using Cassandra.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app_read.Persistence.model
{
    [TableName("auxphonebook")]
    [PrimaryKey("id")]
    public class AuxPhonebook
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Number{ get; set; }
        public AuxPhonebook(string id, string name, string number) 
        {
            this.Id = id;
            this.Name = name;
            this.Number = number;
        }
        public override string ToString()
        {
            return $"{this.Id} => {this.Name} {this.Number}";
        }

    }
}
