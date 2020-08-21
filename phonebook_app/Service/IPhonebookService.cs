using Phonebook_Practice_App.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app.Service
{
    interface IPhonebookService
    {
        IEnumerable<Phonebook> GetAll(string query);
        bool Create(Phonebook item);
        bool Update(Phonebook item);
        bool Delete(Phonebook item);
        bool PublishPost(Phonebook phonebook);
        bool PublishPut(Phonebook phonebook);
        bool PublishPatch(Phonebook phonebook);
        bool PublishDelete(Phonebook phonebook);
    }
}
