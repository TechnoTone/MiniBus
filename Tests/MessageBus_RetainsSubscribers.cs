using System;

using FluentAssertions;

using MiniBus;

using NUnit.Framework;

using Tests.TestSupport;

namespace Tests
{
  [TestFixture]
  public class MyTestClass
  {
    [Test]
    public void WhenTheyAreStillBeingReferenced()
    {
      // Arrange
      var subscribers = new Subscriber<TestMessageTypeA>[100];
      var bus = new MessageBus();
      for (var i = 0; i < 100; i++)
      {
        subscribers[i] = bus.SubscriberFor<TestMessageTypeA>(NoAction);
      }

      // Act
      GC.Collect();

      // Assert
      bus.GetSubscriberCount.Should().Be(100);

    }

    private void NoAction(Message message) { }

  }
}
