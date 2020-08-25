using PhonebookRead.Persistence.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookRead.Service
{
    public interface IPhonebookElasticSearch
    {
        bool Insert(string index, Phonebook phonebook);
        bool Update(string index, Phonebook phonebook);
        bool Delete(string index, Phonebook phonebook);
        IReadOnlyCollection<Phonebook> GetAll(string index, string attributeType, string attributeValue = "");
    }
}
