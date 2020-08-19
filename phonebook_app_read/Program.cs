using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaConnection.kafkawrapper;
using KafkaConnection.Messangerwrapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using phonebook_app_read.Persistence;
using phonebook_app_read.Persistence.model;
using phonebook_practice_app;
using phonebook_practice_app.Persistence.wrapper;

namespace phonebook_app_read
{
    public class Program
    {
        static void BackgroundTask()
        {
            IMessangerWrapper messangerWrapper = new KafkaWrapper("localhost: 9092");
            IConsumer<Ignore, string> consumer = messangerWrapper.Consume("readTable1", new List<string>(){ "test2", "test3" });
            var cancellationToken = new CancellationToken();
            while (true)
            {
                var consumeResult = consumer.Consume(cancellationToken);
                Utils.Print($"{consumeResult.Message.Value}  {consumeResult.Message} {consumeResult.Message.Key}");
                if (consumeResult.Message.Value == "c")
                {
                    consumer.Close();
                    break;
                }
            }
            try
            {
                consumer.Close();
            }catch(Exception ex)
            {

            }
            Thread th = Thread.CurrentThread;

            th.Name = "MainThread";

            Utils.Print("This is {0}", th.Name);

        }
        public static void Main(string[] args)
        {
            Thread StaticCaller = new Thread(new ThreadStart(Program.BackgroundTask));

            var config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true)
                            .AddCommandLine(args)
                            .Build();
            
            Utils.Print($"{config.ToString()} {config.GetValue<string>("AllowedHosts")}");

            IDBRepository db = CassandraDBRepository.Instance(config.GetValue<string>("CASSANDRA_SERVER_NAME"), config.GetValue<string>("CASSANDRA_KEYSPACE_NAME"));
            if (!db.TableExists("phonebookreadname"))
            {
                db.CreateTable("CREATE TABLE phonebookreadname(name text,number text,PRIMARY KEY(name, number));");
                Utils.Print("phonebookreadname created");
            }

            if (!db.TableExists("auxphonebook"))
            {
                db.CreateTable("CREATE TABLE auxphonebook(id text,name text,number text,PRIMARY KEY(id)); ");
                Utils.Print("auxphonebook created");
            }

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


            // Start the thread.
            StaticCaller.Start();


            //Console.ReadKey();

            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
