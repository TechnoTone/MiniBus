<<<<<<< HEAD
﻿using System;

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

        private void NoAction(Message message) { }
    }
}
=======
﻿using System;

using FluentAssertions;

using MiniBus;

using NUnit.Framework;

using Tests.TestSupport;

namespace Tests
{
    [TestFixture]
    public class MessageBusSubscriberCountTests
    {
        private MessageBus bus;

        [TestFixtureSetUp]
        public void SetUp()
        {
            bus = new MessageBus();
            bus.SubscriberFor<Message>(m => { });
        }

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
            if (bus.GetSubscriberCount != 1)
                Assert.Inconclusive(
                    "SubscriberCount should be 1 when subscriber added");

            GC.Collect();

            bus.GetSubscriberCount.Should().Be(0);
        }
    }

    [TestFixture]
    public class MessageBusHasSubscribersTest
    {
        private MessageBus bus;

        [TestFixtureSetUp]
        public void SetUp()
        {
            bus = new MessageBus();
        }

        [Test]
        public void FalseWithNoSubscribers()
        {
            bus.HasSubscriberFor<Message>()
                .Should().BeFalse(
                    "no subscribers have been added");
        }

        [Test]
        public void TrueWithExactMatch()
        {
            bus.SubscriberFor<Message>(m => { });
            bus.HasSubscriberFor<Message>()
                .Should().BeTrue(
                    "subscriber for type was added");
        }

        [Test]
        public void FalseWithNonMatch()
        {
            bus.SubscriberFor<TestMessageTypeA>(m => { });
            bus.HasSubscriberFor<TestMessageTypeB>()
                .Should().BeFalse(
                    "no subscriber for type was added");
        }

        [Test]
        public void TrueWithDerivedType()
        {
            bus.SubscriberFor<TestMessageBase>(m => { });
            bus.HasSubscriberFor<TestMessageTypeA>()
                .Should().BeTrue(
                    "subscriber for super type was added");
        }

        [Test]
        public void FalseWithSuperType()
        {
            bus.SubscriberFor<TestMessageTypeA>(m => { });
            bus.HasSubscriberFor<TestMessageBase>()
                .Should().BeFalse(
                    "subscriber for derived type was added");
        }
    }
}
>>>>>>> 72ec13275f816d3b860a8839b720241bc9ff215e
