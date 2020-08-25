using PhonebookWrite.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookWrite.Service
{
    public interface IMessagePublisher
    {

        bool PublishPost(Phonebook phonebook);
        bool PublishPut(Phonebook phonebook);
        bool PublishPatch(Phonebook phonebook);
        bool PublishDelete(Phonebook phonebook);
    }
}
