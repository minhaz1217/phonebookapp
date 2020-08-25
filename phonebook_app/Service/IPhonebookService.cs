using Phonebook_Practice_App.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app.Service
{
    public interface IPhonebookService
    {
        IEnumerable<Phonebook> GetAll(string query);
        IEnumerable<Phonebook> GetAllPhonebooks();
        IEnumerable<Phonebook> GetPhonebooksById(string id);
        bool Create(Phonebook item);
        bool Update(Phonebook item);
        bool Put(Phonebook item);
        bool Patch(Phonebook item);
        bool Delete(Phonebook item);
    }
}
