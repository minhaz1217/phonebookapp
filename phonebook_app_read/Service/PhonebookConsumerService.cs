using Autofac;
using MessageCarrier.kafkawrapper;
using MessageCarrier.Messangerwrapper;
using MessageCarrier.model;
using Microsoft.Extensions.Configuration;
using PhonebookRead.Persistence;
using PhonebookRead.Persistence.mapper;
using PhonebookRead.Persistence.model;
using PhonebookRead.Service;
using PhonebookRead;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using PhonebookRead;

namespace PhonebookRead
{
    public class PhonebookConsumerService : IPhonebookConsumerService
    {
        string kafkaHost = "";
        string kafkaGroup = "phonebookReader";
        string phonebookTopic= "phonebook.incoming";

        string elasticPhonebookIndex = "";

        CancellationToken cancellationToken;
        IDBRepository db = null;
        IPhonebookElasticSearch elastic = null;
        IMessangerWrapper messangerWrapper = null;
        //private readonly ILifetimeScope container;

        public PhonebookConsumerService( IDBRepository dBRepository, IPhonebookElasticSearch phonebookElasticSearch, IMessangerWrapper messangerWrapper) 
        {
            //this.container = container;
            this.cancellationToken = new CancellationToken();


            this.kafkaHost = ConfigReader.GetValue<string>("KafkaHost");
            
            this.elasticPhonebookIndex = ConfigReader.GetValue<string>("ElasticPhonebookIndex");
            this.db = dBRepository;
            this.elastic = phonebookElasticSearch;
            this.messangerWrapper = messangerWrapper;
        }

        private void TopicManager(string message)
        {
            WrapperModel<Phonebook> wrapperModel = JsonSerializer.Deserialize<WrapperModel<Phonebook>>(message);
            Helper.Print("Received: " + message);
            if(wrapperModel.Action == "post")
            {
                this.PhonebookPOST(wrapperModel.Child);
                this.elastic.Insert(this.elasticPhonebookIndex, wrapperModel.Child);
            }
            else if (wrapperModel.Action == "put")
            {
                this.PhonebookPUT(wrapperModel.Child);
                this.elastic.Update(this.elasticPhonebookIndex, wrapperModel.Child);
            }
            else if (wrapperModel.Action == "patch")
            {
                this.PhonebookPATCH(wrapperModel.Child);
                this.elastic.Update(this.elasticPhonebookIndex, wrapperModel.Child);
            }
            else if (wrapperModel.Action == "delete")
            {
                this.PhonebookDELETE(wrapperModel.Child);
                this.elastic.Delete(this.elasticPhonebookIndex, wrapperModel.Child);
            }
        }
        public void RegisterMethods()
        {
            //IMessangerWrapper messangerWrapper = this.container.Resolve<IMessangerWrapper>();
            Dictionary<string, Action<string>> dict = new Dictionary<string, Action<string>>();
            dict[phonebookTopic] = this.TopicManager;
            messangerWrapper.Consume(this.kafkaGroup, dict, this.cancellationToken);
        }


        private void PhonebookPOST(Phonebook phonebook)
        {
            this.db.CreatePhonebook(phonebook);
            this.db.CreatePhonebookReadName( PhonebookMapper.PhonebookToPhonebookReadName(phonebook) );
        }
        private void PhonebookPUT(Phonebook phonebook)
        {
            IEnumerable<Phonebook> oldEntries = this.db.GetAllPhonebook( "select * from phonebook where id= ?;", phonebook.Id);
            Phonebook oldEntry = null;
            foreach(Phonebook pb in oldEntries)
            {
                oldEntry = pb;
                break;
            }
            if(oldEntry != null)
            {
                Helper.Print("Old => " + oldEntry.ToString());
                Helper.Print("New => " + phonebook.ToString());
                this.db.DeletePhonebookReadName(PhonebookMapper.PhonebookToPhonebookReadName(oldEntry));
                this.db.UpdatePhonebook(phonebook);
                this.db.CreatePhonebookReadName(PhonebookMapper.PhonebookToPhonebookReadName(phonebook));
            }
        }
        private void PhonebookPATCH(Phonebook phonebook)
        {
            IEnumerable<Phonebook> oldEntries = this.db.GetAllPhonebook("select * from phonebook where id= ?;", phonebook.Id);
            Phonebook oldEntry = null;
            foreach (Phonebook pb in oldEntries)
            {
                oldEntry = pb;
                break;
            }
            if (oldEntry != null)
            {
                Helper.Print("Old => " + oldEntry.ToString());
                Helper.Print("New => " + phonebook.ToString());
                this.db.DeletePhonebookReadName(PhonebookMapper.PhonebookToPhonebookReadName(oldEntry));
                this.db.UpdatePhonebook(phonebook);
                this.db.CreatePhonebookReadName(PhonebookMapper.PhonebookToPhonebookReadName(phonebook));
            }
        }
        private void PhonebookDELETE(Phonebook phonebook)
        {
            this.db.DeletePhonebook(phonebook.Id);
            this.db.DeletePhonebookReadName(PhonebookMapper.PhonebookToPhonebookReadName(phonebook));
        }
    }
}
