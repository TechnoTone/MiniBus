using System;
using System.Threading;

using FluentAssertions;

namespace Tests.TestSupport
{
  public static class Wait
  {
    public static void For(TimeSpan waitTime)
    {
      Thread.Sleep(waitTime);
    }

    public static void For(Func<bool> condition)
    {
      new WaitFor(condition).Wait();
    }

    public static void For(Func<bool> condition, TimeSpan waitTime)
    {
      new WaitFor(condition).Wait(waitTime);
    }

    private class WaitFor
    {
      private readonly Func<bool> condition;
      private DateTime startTime;

      public WaitFor(Func<bool> condition)
      {
        this.condition = condition;
        startTime = DateTime.Now;
      }

      public void Wait()
      {
        while (!condition())
          Thread.Sleep(2.Milliseconds());
      }

      public void Wait(TimeSpan waitTime)
      {
        while (!condition())
          if (timedOut(waitTime))
            throw new TimeoutException();
          else
            Thread.Sleep(2.Milliseconds());
      }

      private bool timedOut(TimeSpan waitTime)
      {
        return (startTime.Add(waitTime) < DateTime.Now);
      }
    }
  }
}
