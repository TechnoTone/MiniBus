using FluentAssertions;

using MiniBus;

using NUnit.Framework;

using Tests.TestSupport;

namespace Tests
{
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
