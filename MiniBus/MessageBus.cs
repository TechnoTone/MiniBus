using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBus
{
    public class MessageBus
    {
        private readonly ConcurrentDictionary<WeakReference, Type> subscribers =
            new ConcurrentDictionary<WeakReference, Type>();

        public Subscriber<T> SubscriberFor<T>(Action<T> action)
            where T : class, Message
        {
            var subscriber = new ActionSubscriber<T>(action);

            var weakReference = new WeakReference(subscriber);
            var added =
                subscribers.TryAdd(
                    weakReference,
                    action.GetType().GenericTypeArguments[0]);

            if (!added)
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

        public bool HasSubscriberFor<T>()
            where T : class, Message
        {
            removeAllExpiredSubscribers();

            return
                subscribers.Keys
                    .Any(wr => wr.Target is Subscriber<T>);
        }

        public IEnumerable<Subscriber<Message>>
            FindSubscribersOfExplicitType(Type type)
        {
            return
                subscribers
                    .Keys
                    .Where(
                        wr =>
                            isCorrectGenericType(type, wr) &&
                            isSubscriberType(type, wr))
                    .Select(wr => wr.Target as Subscriber<Message>);
        }

        private static bool isCorrectGenericType(Type type, WeakReference wr)
        {
            return wr.Target.GetType().GenericTypeArguments[0] == type;
        }

        private bool isSubscriberType(Type type, WeakReference wr)
        {
            return subscribers[wr] == type;
        }

        public void SendMessage<T>(T message)
            where T : class, Message
        {
            sendToSubscribers(message);
        }

        private void sendToSubscribers<T>(T message)
            where T : class, Message
        {
            var list = findSubscribersOfType<T>().ToList();
            list.ForEach(
                sub =>
                    invokeSubscriberInNewTask(sub, message));
        }

        private IEnumerable<Subscriber<T>> findSubscribersOfType<T>()
            where T : class, Message
        {
            removeAllExpiredSubscribers();

            return
                subscribers.Keys
                    .Where(wr => wr.Target is Subscriber<T>)
                    .Select(wr => wr.Target as Subscriber<T>);
        }

        private static void invokeSubscriberInNewTask<T>(Subscriber<T> subscriber, T message)
            where T : class, Message
        {
            if (subscriber == null)
                return;

            var task = new Task(
                () => subscriber.InvokeAction(message));

            task.RunSynchronously();
            System.Threading.Thread.Sleep(1);
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
            Type removed;
            subscribers.TryRemove(reference, out removed);
        }
    }
}
