using KafkaConnection.kafkawrapper;
using KafkaConnection.Messangerwrapper;
using KafkaConnection.model;
using Microsoft.Extensions.Configuration;
using phonebook_app_read.Persistence;
using phonebook_app_read.Persistence.mapper;
using phonebook_app_read.Persistence.model;
using phonebook_practice_app;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace phonebook_app_read
{
    public class PhonebookConsumerService
    {
        CancellationToken cancellationToken;
        IDBRepository db = null;
        public PhonebookConsumerService() 
        {
            this.cancellationToken = new CancellationToken();
            var config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true)
                            .Build();

            Utils.Print($"{config.ToString()} {config.GetValue<string>("AllowedHosts")}");
            this.db = CassandraDBRepository.Instance(config.GetValue<string>("CASSANDRA_SERVER_NAME"), config.GetValue<string>("CASSANDRA_KEYSPACE_NAME"));

        }

        public void TopicManager(string message)
        {
            WrapperModel<Phonebook> wrapperModel = JsonSerializer.Deserialize<WrapperModel<Phonebook>>(message);
            if(wrapperModel.Action == "post")
            {
                this.PhonebookPOST(wrapperModel.Child);
            }
            else if (wrapperModel.Action == "put")
            {
                this.PhonebookPUT(wrapperModel.Child);
            }
            else if (wrapperModel.Action == "patch")
            {
                this.PhonebookPATCH(wrapperModel.Child);
            }
            else if (wrapperModel.Action == "delete")
            {
                this.PhonebookDELETE(wrapperModel.Child);
            }
        }
        public void RegisterMethods()
        {
            IMessangerWrapper messangerWrapper = new KafkaWrapper("localhost: 9092");
            Dictionary<string, Action<string>> dict = new Dictionary<string, Action<string>>();
            dict["phonebook"] = this.TopicManager;
            messangerWrapper.Consume("phonebookGroup7", dict, this.cancellationToken);
        }


        public void PhonebookPOST(Phonebook phonebook)
        {
            this.db.CreatePhonebook(phonebook);
            this.db.CreatePhonebookReadName( PersistenceMapper.PhonebookToPhonebookReadName(phonebook) );
        }
        public void PhonebookPUT(Phonebook phonebook)
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
                Utils.Print("Old => " + oldEntry.ToString());
                Utils.Print("New => " + phonebook.ToString());
                this.db.DeletePhonebookReadName(PersistenceMapper.PhonebookToPhonebookReadName(oldEntry));
                this.db.UpdatePhonebook(phonebook);
                this.db.CreatePhonebookReadName(PersistenceMapper.PhonebookToPhonebookReadName(phonebook));
            }
        }
        public void PhonebookPATCH(Phonebook phonebook)
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
                Utils.Print("Old => " + oldEntry.ToString());
                Utils.Print("New => " + phonebook.ToString());
                this.db.DeletePhonebookReadName(PersistenceMapper.PhonebookToPhonebookReadName(oldEntry));
                this.db.UpdatePhonebook(phonebook);
                this.db.CreatePhonebookReadName(PersistenceMapper.PhonebookToPhonebookReadName(phonebook));
            }
        }
        public void PhonebookDELETE(Phonebook phonebook)
        {
            this.db.DeletePhonebook(phonebook.Id);
            this.db.DeletePhonebookReadName(PersistenceMapper.PhonebookToPhonebookReadName(phonebook));
        }
    }
}
