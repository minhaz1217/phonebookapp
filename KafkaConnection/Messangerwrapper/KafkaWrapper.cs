using Confluent.Kafka;
using KafkaConnection.kafkawrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace KafkaConnection.Messangerwrapper
{
    public class KafkaWrapper : IMessangerWrapper
    {
        private ProducerConfig config = null;
        private string ConnectionString = "";
        public KafkaWrapper(string connectionUrl = "localhost:9092")
        {
            this.ConnectionString = connectionUrl;
        }

        private string CustomMetadata(string message)
        {
            return message;
        }

        public string Produce(string topic, string message)
        {
            config = new ProducerConfig
            {
                BootstrapServers = this.ConnectionString,
                ClientId = Dns.GetHostName(),
            };
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                producer.Produce(topic, new Message<Null, string> { Value = this.CustomMetadata(message) }); ;
            }
            return $"{topic} => {message}";
        }


        public void Consume(string groupId, Dictionary<string, Action<string>> TopicCallbackDict, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = this.ConnectionString,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            //var cancellationToken = new CancellationToken();

            var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            StringBuilder sb = new StringBuilder();
            foreach (string key in TopicCallbackDict.Keys)
            {
                sb.Append(key + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            consumer.Subscribe(sb.ToString());


            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(cancellationToken);
                foreach(KeyValuePair<string, Action<string> > kvp in TopicCallbackDict)
                {
                    if(kvp.Key == consumeResult.Topic)
                    {
                        kvp.Value( consumeResult.Message.Value );
                    }
                }
            }
            consumer.Close();
        }

        ~KafkaWrapper()
        {
            
        }
    }
}
