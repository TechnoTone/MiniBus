<<<<<<< HEAD
﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBus
{
    public class MessageBus
    {
        private readonly ConcurrentDictionary<WeakReference, WeakReference> subscribers =
            new ConcurrentDictionary<WeakReference, WeakReference>();

        public Subscriber<T> SubscriberFor<T>(Action<T> action)
            where T : class, Message
        {
            var subscriber = new ActionSubscriber<T>(action);

            var weakReference = new WeakReference(subscriber);
            if (!subscribers.TryAdd(weakReference, weakReference))
                throw new InvalidAsynchronousStateException(
                    "Failed to add Subscriber");

            return subscriber;
        }

        public int GetSubscriberCount
        {
            get
            {
                removeAllExpiredSubscribers();
                return subscribers.Count;
            }
        }

        public void SendMessage<T>(T message)
            where T : class, Message
        {
            sendToSubscribers(message);
        }

        private void sendToSubscribers<T>(T message)
            where T : class, Message
        {
            var list = findSubscribersOfType<T>();
            list.ForEach(
                sub =>
                    invokeSubscriberInNewTask(sub, message));
        }

        private List<Subscriber<T>> findSubscribersOfType<T>()
            where T : class, Message
        {
            removeAllExpiredSubscribers();

            return
                subscribers
                    .Values
                    .Where(wr => wr.Target is Subscriber<T>)
                    .Select(wr => wr.Target as Subscriber<T>)
                    .ToList();
        }

        private static void invokeSubscriberInNewTask<T>(Subscriber<T> subscriber, T message)
            where T : class, Message
        {
            if (subscriber == null)
                return;

            var task = new Task(
                () => subscriber.InvokeAction(message));

            task.RunSynchronously();
        }

        private void removeAllExpiredSubscribers()
        {
            var deadKeys =
                subscribers.Keys.Where(isDead).ToList();

            deadKeys.ForEach(delete);
        }

        private bool isDead(WeakReference reference)
        {
            return !reference.IsAlive ||
                   reference.Target == null;
        }

        private void delete(WeakReference reference)
        {
            WeakReference removed;
            subscribers.TryRemove(reference, out removed);
        }
    }
}
=======
﻿using System;
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
                removeAllExpiredSubscribers();
                return subscribers.Count;
            }
        }

        public IEnumerable<Subscriber<T>> FindSubscribersOfType<T>()
            where T : class, Message
        {
            removeAllExpiredSubscribers();

            return
                subscribers
                    .Where(wr => wr.Target is Subscriber<T>)
                    .Select(wr => wr.Target as Subscriber<T>);
        }

        public bool HasSubscriberFor<T>()
            where T : class, Message
        {
            removeAllExpiredSubscribers();

            return
                subscribers
                    .Exists(wr => wr.Target is Subscriber<T>);
        }

        public void SendMessage<T>(T message)
            where T : class, Message
        {
            sendToSubscribers(message);
        }

        private void sendToSubscribers<T>(T message)
            where T : class, Message
        {
            foreach (var subscriber in FindSubscribersOfType<T>())
            {
                invokeSubscriberInNewTask(subscriber, message);
            }
        }

        private static void invokeSubscriberInNewTask<T>(Subscriber<T> subscriber, T message)
            where T : class, Message
        {
            if (subscriber == null)
                return;

            new Task(() => subscriber.InvokeAction(message)).Start();
        }

        private void removeAllExpiredSubscribers()
        {
            subscribers.RemoveAll(wr => !wr.IsAlive);
            subscribers.RemoveAll(wr => wr.Target == null);
        }
    }
}
>>>>>>> 72ec13275f816d3b860a8839b720241bc9ff215e
