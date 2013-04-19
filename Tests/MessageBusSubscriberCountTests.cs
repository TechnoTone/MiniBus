using System;

using FluentAssertions;

using MiniBus;

using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MessageBusSubscriberCountTests
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

        private MessageBus bus;
        private Subscriber<Message> subscriber;

        [TestFixtureSetUp]
        public void SetUp()
        {
            bus = new MessageBus();
            subscriber = bus.SubscriberFor<Message>(NoAction);
        }

        [Test]
        public void DecrementsWhenSubscribersAreGarbageCollected()
        {
            if (bus.GetSubscriberCount != 1)
                Assert.Inconclusive(
                    "SubscriberCount should be 1 when subscriber added");

            subscriber = null;
            GC.Collect();

            bus.GetSubscriberCount.Should().Be(0);
        }

        private void NoAction(Message message) { }
    }
}
