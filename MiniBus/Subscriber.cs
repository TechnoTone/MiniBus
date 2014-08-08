using System;

namespace MiniBus
{
  public interface Subscriber<in T>
      where T : class, Message
  {
    void InvokeAction(T message, Action<AggregateException> exceptionListener);
  }
}
