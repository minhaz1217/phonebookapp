using Confluent.Kafka;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace KafkaConnection.kafkawrapper
{
    public interface IMessangerWrapper
    {
        string Produce(string topic, string message);
        IConsumer<Ignore, string> Consume(string groupId, IEnumerable<string> Topics);
    }
}
