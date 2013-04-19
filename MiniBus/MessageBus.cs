using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBus
{
    public class MessageBus
    {
        private readonly List<WeakReference> subscribers =
            new List<WeakReference>();

        public Subscriber<T> SubscriberFor<T>(Action<T> action)
            where T : class, Message
        {
            var subscriber = new ActionSubscriber<T>(action);
            subscribers.Add(new WeakReference(subscriber));
            return subscriber;
        }

        public int GetSubscriberCount
        {
            get
            {
                RemoveAllExpiredSubscribers();
                return subscribers.Count;
            }
        }

        public void SendMessage<T>(T message)
            where T : class, Message
        {
            SendToSubscribers(message);
        }

        private void SendToSubscribers<T>(T message)
            where T : class, Message
        {
            foreach (var subscriber in FindSubscribersOfType<T>())
            {
                InvokeSubscriberInNewTask(subscriber, message);
            }
        }

        private IEnumerable<Subscriber<T>> FindSubscribersOfType<T>()
            where T : class, Message
        {
            RemoveAllExpiredSubscribers();

            return
                subscribers
                    .Where(wr => wr.Target is Subscriber<T>)
                    .Select(wr => wr.Target as Subscriber<T>);
        }

        private static void InvokeSubscriberInNewTask<T>(Subscriber<T> subscriber, T message)
            where T : class, Message
        {
            if (subscriber == null)
                return;

            new Task(() => subscriber.InvokeAction(message)).Start();
        }

        private void RemoveAllExpiredSubscribers()
        {
            subscribers.RemoveAll(wr => !wr.IsAlive);
            subscribers.RemoveAll(wr => wr.Target == null);
        }
    }
}
