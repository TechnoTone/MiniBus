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

    public void InvokeAction(T message, Action<AggregateException> exceptionListener)
    {
      Task
        .Run(
          () => action(message))
        .ContinueWith(
          t =>
          {
            if (t.Exception != null)
              exceptionListener(t.Exception);
          });
    }
  }
}
