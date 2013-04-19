using System;
using System.Threading.Tasks;

namespace MiniBus
{
    public class ActionSubscriber<T> : Subscriber<T>
        where T : class, Message
    {
        private readonly Action<T> action;

        public ActionSubscriber(Action<T> action)
        {
            this.action = action;
        }

        public void InvokeAction(T message)
        {
            var task = new Task(() => action(message));
            task.Start();
        }
    }
}
