using Confluent.Kafka;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MessageCarrier.kafkawrapper
{
    public interface IMessangerWrapper
    {
        string Produce(string topic, string message);
        void Consume(string groupId, Dictionary<string, Action<string>> TopicCallbackDict, CancellationToken cancellationToken);
    }
}
