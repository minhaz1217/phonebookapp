using Autofac;
using KafkaConnection.kafkawrapper;
using KafkaConnection.model;
using phonebook_app_read;
using phonebook_practice_app;
using phonebook_practice_app.Persistence.wrapper;
using Phonebook_Practice_App.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace phonebook_app.Service
{
    public class PhonebookService : IPhonebookService
    {
        private string kafkaTopic = "phonebooktest101";

        private ILifetimeScope container = null;
        public PhonebookService(ILifetimeScope container)
        {
            this.container = container;
        }
        public bool Create(Phonebook phonebook)
        {
            IConnectionWrapper wrapper = this.container.Resolve<IConnectionWrapper>();
            phonebook.Id = Guid.NewGuid().ToString();
            return wrapper.Create<Phonebook>(phonebook);
        }

        public bool Delete(Phonebook phonebook)
        {
            IConnectionWrapper wrapper = this.container.Resolve<IConnectionWrapper>();
            return wrapper.Delete<Phonebook>(phonebook);
        }

        public IEnumerable<Phonebook> GetAll(string query)
        {
            List<Phonebook> myBooks = (List<Phonebook>)(this.container.Resolve<IConnectionWrapper>()).GetAll<Phonebook>(query);
            return myBooks;
        }

        public bool Update(Phonebook phonebook)
        {
            IConnectionWrapper wrapper = this.container.Resolve<IConnectionWrapper>();
            return wrapper.Update<Phonebook>(phonebook);
        }


        public bool PublishPost(Phonebook phonebook)
        {
            IMessangerWrapper kafkaWrapper = this.container.Resolve<IMessangerWrapper>();
            WrapperModel<Phonebook> wrapperModel = new WrapperModel<Phonebook>("post", "phonebook", phonebook);
            string kfStr = kafkaWrapper.Produce(this.kafkaTopic, JsonSerializer.Serialize(wrapperModel));
            Helper.Print(kfStr);
            return true;
        }
        public bool PublishPut(Phonebook phonebook)
        {
            IMessangerWrapper kafkaWrapper = this.container.Resolve<IMessangerWrapper>();
            WrapperModel<Phonebook> wrapperModel = new WrapperModel<Phonebook>("put", "phonebook", phonebook);
            string kfStr = kafkaWrapper.Produce(this.kafkaTopic, JsonSerializer.Serialize(wrapperModel));
            Helper.Print(kfStr);
            return true;
        }
        public bool PublishPatch(Phonebook phonebook)
        {
            IMessangerWrapper kafkaWrapper = this.container.Resolve<IMessangerWrapper>();
            WrapperModel<Phonebook> wrapperModel = new WrapperModel<Phonebook>("patch", "phonebook", phonebook);
            string kfStr = kafkaWrapper.Produce(this.kafkaTopic, JsonSerializer.Serialize(wrapperModel));
            Helper.Print(kfStr);
            return true;
        }
        public bool PublishDelete(Phonebook phonebook)
        {
            IMessangerWrapper kafkaWrapper = this.container.Resolve<IMessangerWrapper>();
            WrapperModel<Phonebook> wrapperModel = new WrapperModel<Phonebook>("delete", "phonebook", phonebook);
            string msg = kafkaWrapper.Produce(this.kafkaTopic, JsonSerializer.Serialize(wrapperModel));
            return true;
        }
    }
}
