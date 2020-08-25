using phonebook_practice_app.Persistence;
using Phonebook_Practice_App.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phonebook_app.Persistence
{
    public interface IDBRepository

    {
        IEnumerable<Phonebook> GetAllPhonebooks(string query);
        bool Create(Phonebook item);
        bool Update(Phonebook item);
        bool Delete(Phonebook item);
    }
}
