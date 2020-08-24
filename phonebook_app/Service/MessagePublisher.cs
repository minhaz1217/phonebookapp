using Autofac;
using KafkaConnection.kafkawrapper;
using KafkaConnection.model;
using Phonebook_Practice_App.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace phonebook_app.Service
{
    public class MessagePublisher
    {
        private string kafkaTopic = "phonebook.incoming";

        private ILifetimeScope container = null;
        public MessagePublisher(ILifetimeScope container)
        {
            this.container = container;
        }
        public bool PublishPost(Phonebook phonebook)
        {
            IMessangerWrapper kafkaWrapper = this.container.Resolve<IMessangerWrapper>();
            WrapperModel<Phonebook> wrapperModel = new WrapperModel<Phonebook>("post", "phonebook", DateTime.Now.ToString(), DateTime.Now.ToString(), "Phonebook.Write", "Phonebook.Write", "Phonebook.Write", phonebook);
            string kfStr = kafkaWrapper.Produce(this.kafkaTopic, JsonSerializer.Serialize(wrapperModel));
            //Helper.Print(kfStr);
            return true;
        }
        public bool PublishPut(Phonebook phonebook)
        {
            IMessangerWrapper kafkaWrapper = this.container.Resolve<IMessangerWrapper>();
            WrapperModel<Phonebook> wrapperModel = new WrapperModel<Phonebook>("put", "phonebook", DateTime.Now.ToString(), DateTime.Now.ToString(), "Phonebook.Write", "Phonebook.Write", "Phonebook.Write", phonebook);
            string kfStr = kafkaWrapper.Produce(this.kafkaTopic, JsonSerializer.Serialize(wrapperModel));
            //Helper.Print(kfStr);
            return true;
        }
        public bool PublishPatch(Phonebook phonebook)
        {
            IMessangerWrapper kafkaWrapper = this.container.Resolve<IMessangerWrapper>();
            WrapperModel<Phonebook> wrapperModel = new WrapperModel<Phonebook>("patch", "phonebook", DateTime.Now.ToString(), DateTime.Now.ToString(), "Phonebook.Write", "Phonebook.Write", "Phonebook.Write", phonebook);
            string kfStr = kafkaWrapper.Produce(this.kafkaTopic, JsonSerializer.Serialize(wrapperModel));
            //Helper.Print(kfStr);
            return true;
        }
        public bool PublishDelete(Phonebook phonebook)
        {
            IMessangerWrapper kafkaWrapper = this.container.Resolve<IMessangerWrapper>();
            WrapperModel<Phonebook> wrapperModel = new WrapperModel<Phonebook>("delete", "phonebook", DateTime.Now.ToString(), DateTime.Now.ToString(), "Phonebook.Write", "Phonebook.Write", "Phonebook.Write", phonebook);
            string kfStr = kafkaWrapper.Produce(this.kafkaTopic, JsonSerializer.Serialize(wrapperModel));
            //Helper.Print(kfStr);
            return true;
        }
    }
}
