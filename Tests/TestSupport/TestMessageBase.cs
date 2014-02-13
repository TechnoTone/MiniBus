using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using MiniBus;

namespace Tests.TestSupport
{
    public class TestMessageBase : Message, IDisposable
    {
        private readonly BlockingCollection<ReceivedEvent> receiverLog =
            new BlockingCollection<ReceivedEvent>();

        public IEnumerable<ReceivedEvent> GetReceiverLog
        {
            get
            {
                return receiverLog;
            }
        }

        public void Received(string receiverName)
        {
            receiverLog.Add(new ReceivedEvent
            {
                ReceiverName = receiverName,
                ReceiverThreadId = Thread.CurrentThread.ManagedThreadId
            });
            Thread.Sleep(20);
        }

        public void Dispose()
        {
            receiverLog.Dispose();
        }
    }
}
