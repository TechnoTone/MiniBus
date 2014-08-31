using System;

using FluentAssertions;

using MiniBus;

using NUnit.Framework;

using Tests.TestSupport;

namespace Tests
{
  [TestFixture]
  public class MessageBus_SubscriberCount
  {
    [Test]
    public void StartsWithZero()
    {
      var bus = new MessageBus();

      bus.GetSubscriberCount.Should().Be(0);
    }

    [Test]
    public void IncrementsWhenAddingSubscribers()
    {
      var bus = new MessageBus();

      if (bus.GetSubscriberCount != 0)
        Assert.Inconclusive("SubscriberCount should start at 0");

      bus.SubscriberFor<Message>(_ => { });

      bus.GetSubscriberCount.Should().Be(1);
    }

    [Test]
    public void DecrementsWhenSubscribersAreGarbageCollected()
    {
      var bus = new MessageBus();
      // ReSharper disable once NotAccessedVariable
      var subscriber = bus.SubscriberFor<Message>(NoAction);

      Wait.For(() => bus.GetSubscriberCount == 1);

      // ReSharper disable once RedundantAssignment
      subscriber = null;
      GC.Collect();

      Wait.For(() => bus.GetSubscriberCount == 0);

      bus.GetSubscriberCount.Should().Be(0);
    }

    [Test]
    public void CopesWithHighConcurrencySubscriptions()
    {
      // Arrange
      var bus = new MessageBus();

      // Act
      for (var i = 0; i < 1000; i++)
        bus.SubscriberFor<Message>(NoAction);

      // Assert
      bus.GetSubscriberCount
          .Should().Be(1000);
    }

    [Test]
    [Timeout(60000)]
    public void RetainsSubscriptions()
    {
      // Arrange
      var bus = new MessageBus();

      // Act
      for (var i = 0; i < 1000; i++)
        bus.SubscriberFor<Message>(NoAction);

      // Assert
      bus.GetSubscriberCount
          .Should().Be(1000);
    }

    private void NoAction(Message message) { }
  }
}