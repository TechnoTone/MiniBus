using System;
using System.Collections.ObjectModel;
using System.Threading;

using MiniBus;

using NUnit.Framework;

using Tests.TestSupport;

namespace Tests
{
  [TestFixture]
  public class MessageBus_HandlesExceptions
  {
    [Test]
    public void WhenSubscriberThrowsException()
    {
      // Arrange
      var bus = new MessageBus();
      bus.SubscriberFor<Message>(
        m =>
        {
          throw new Exception();
        });
      var exceptions = new Collection<AggregateException>();
      bus.ExceptionListener = exceptions.Add;

      // Act
      bus.SendMessage(new TestMessageTypeA());

      // Assert
      while (exceptions.Count == 0)
      {
        Thread.Sleep(100);
      }

    }
  }
}
