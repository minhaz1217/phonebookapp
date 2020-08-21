using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Bogus;
using Confluent.Kafka;
using Elasticsearch.Net;
using KafkaConnection.kafkawrapper;
using KafkaConnection.Messangerwrapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using phonebook_app_read.Persistence;
using phonebook_app_read.Persistence.model;
using phonebook_app_read.Service;
using phonebook_practice_app;
using phonebook_practice_app.Persistence.wrapper;

namespace phonebook_app_read
{
    public class Program
    {
        static void BackgroundTask()
        {
            //var node = new Uri("http://mynode.example.com:8082/apiKey");
            //var config = new ConnectionConfiguration(node);
            //var client = new ElasticClient(config);

            // TODO: uncomment these to get the Kafka consumer running
            //IMessangerWrapper messangerWrapper = new KafkaWrapper("localhost: 9092");
            //IConsumer<Ignore, string> consumer = messangerWrapper.Consume("readTable1", new List<string>(){ "test2", "test3" });
            //var cancellationToken = new CancellationToken();
            //while (true)
            //{
            //    var consumeResult = consumer.Consume(cancellationToken);
            //    Utils.Print($"{consumeResult.Message.Value}  {consumeResult.Message} {consumeResult.Message.Key}");
            //    if (consumeResult.Message.Value == "c")
            //    {
            //        consumer.Close();
            //        break;
            //    }
            //}
            //try
            //{
            //    consumer.Close();
            //}catch(Exception ex)
            //{

            //}
            //Thread th = Thread.CurrentThread;

            //th.Name = "MainThread";

            //Utils.Print("This is {0}", th.Name);

        }
        public static void Main(string[] args)
        {            
            //Utils.Print($"{config.ToString()} {config.GetValue<string>("AllowedHosts")}");

            // TODO: move the tables creation to a seperate class

            //PhonebookElasticSearch es = PhonebookElasticSearch.Instance();
            //string indexName = "phonebooktest2";
            //Phonebook pbMinhaz = new Phonebook((Guid.NewGuid()).ToString(), "Minhazul", "000000000");

            //Faker f = new Faker("en");
            //for (int i = 0; i < 5; i++) 
            //{

            //    Phonebook val1 = new Phonebook((Guid.NewGuid()).ToString(), f.Name.FirstName(), f.Phone.PhoneNumber());
            //    //val1 = new PhonebookReadName("minhaz", "123");
            //    Helper.Print($"{val1.Name} {val1.Number}");
            //    es.Insert(indexName, val1);
            //}
            //string id = "1001";
            //pbMinhaz.Id = id;
            //Helper.Print($"Created: {es.Insert(indexName, pbMinhaz)}");
            //pbMinhaz.Name = "Minhaz";
            //Helper.Print($"Updated: {es.Update(indexName, pbMinhaz)}");
            ////Helper.Print($"Delete: {es.Delete(indexName, pbMinhaz)}");


            //string name = val1.Name;
            //var resp = es.Search(indexName, name);
            ////Helper.Print(resp.Documents.ToString());
            //Helper.Print("Searching");
            //foreach (var x in resp)
            //{
            //    Helper.Print(x.ToString());
            //}







            //IEnumerable<Phonebook> phonebooks = db.GetAllPhonebook("select * from phonebook where id=?", "78f73bdc-845c-46c7-9950-d83c75e8c805");
            //foreach(Phonebook pb in phonebooks)
            //{
            //    Utils.Print(pb.ToString());
            //}
            //Utils.Print(phonebooks[0].ToString());
            //Utils.Print(phonebooks.First().ToString());
            //Utils.Print("PB ENDED");







            //AuxPhonebook aPhonebook = new AuxPhonebook("1111111111", "sfdgsdfg", "0123456");

            //db.CreateAuxPhonebook(aPhonebook);

            //Utils.Print("Get ALL");
            //db.GetAllPhonebookReadName().ToList().ForEach(delegate (PhonebookReadName pb)
            //{
            //    Utils.Print(pb.ToString());
            //});

            //PhonebookReadName phonebook = new PhonebookReadName(aPhonebook.Name, aPhonebook.Number);
            //Utils.Print("Inser PB");
            //db.CreatePhonebookReadName(phonebook);
            //Utils.Print("Get ALL");
            //db.GetAllPhonebookReadName().ToList().ForEach(delegate (PhonebookReadName pb)
            //{
            //    Utils.Print(pb.ToString());
            //});
            //phonebook.Name = "ANIK";
            //db.UpdatePhonebookReadName(aPhonebook, phonebook);
            //Utils.Print("Get ALL");
            //db.GetAllPhonebookReadName().ToList().ForEach(delegate (PhonebookReadName pb)
            //{
            //    Utils.Print(pb.ToString());
            //});
            //Utils.Print("After delete");
            //db.DeletePhonebookReadName(phonebook);
            //Utils.Print("Get ALL");
            //db.GetAllPhonebookReadName().ToList().ForEach(delegate (PhonebookReadName pb)
            //{
            //    Utils.Print(pb.ToString());
            //});




            //Console.ReadKey();

            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
