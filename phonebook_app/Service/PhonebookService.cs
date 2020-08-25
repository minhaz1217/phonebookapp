using Autofac;
using MessageCarrier.kafkawrapper;
using MessageCarrier.model;
using PhonebookWrite.Persistence;
using PhonebookWrite;
using PhonebookWrite.Persistence.wrapper;
using PhonebookWrite.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PhonebookWrite.Service
{
    public class PhonebookService : IPhonebookService
    {
        private string kafkaTopic = "phonebook.incoming";
        private IMessagePublisher messagePublisher = null;
        private IDBRepository dbRepository = null;
        public PhonebookService(IMessagePublisher messagePublisher, IDBRepository dbRepository)
        {
            this.messagePublisher = messagePublisher;
            //this.messagePublisher = this.container.Resolve<IMessagePublisher>();
            this.dbRepository = dbRepository;
        }
        public bool Create(Phonebook phonebook)
        {
            phonebook.Id = Guid.NewGuid().ToString();
            this.messagePublisher.PublishPost(phonebook);
            return this.dbRepository.Create(phonebook);
        }

        public bool Delete(Phonebook phonebook)
        {
            this.messagePublisher.PublishDelete(phonebook);
            return this.dbRepository.Delete(phonebook);
        }

        public IEnumerable<Phonebook> GetAll(string query)
        {
            //List<Phonebook> myBooks = (List<Phonebook>)(this.container.Resolve<IConnectionWrapper>()).GetAll<Phonebook>(query);
            List<Phonebook> myBooks = (List<Phonebook>) this.dbRepository.GetAllPhonebooks(query);
            return myBooks;
        }
        public IEnumerable<Phonebook> GetAllPhonebooks()
        {
            List<Phonebook> myBooks = (List<Phonebook>)this.dbRepository.GetAllPhonebooks("select * from phonebook;");
            return myBooks;
        }
        public IEnumerable<Phonebook> GetPhonebooksById(string id)
        {
            Guid gId = new Guid();
            Guid.TryParse(id, out gId);
            if(gId== new Guid())
            {
                return new List<Phonebook>();
            }
            List<Phonebook> myBooks = (List<Phonebook>)this.dbRepository.GetAllPhonebooks($"select * from phonebook where id='{id}';");
            return myBooks;
        }
        public bool Put(Phonebook phonebook)
        {
            this.messagePublisher.PublishPut(phonebook);
            return this.dbRepository.Update(phonebook);
        }

        public bool Patch(Phonebook phonebook)
        {
            this.messagePublisher.PublishPatch(phonebook);
            return this.dbRepository.Update(phonebook);
        }

        public bool Update(Phonebook phonebook)
        {
            return this.dbRepository.Update(phonebook);
        }

    }
}
