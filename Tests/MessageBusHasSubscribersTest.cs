using FluentAssertions;

using MiniBus;

using NUnit.Framework;

using Tests.TestSupport;

namespace Tests
{
    [TestFixture]
    public class MessageBusHasSubscribersTest
    {
        [Test]
        public void FalseWithNoSubscribers()
        {
            var bus = new MessageBus();
            bus.HasSubscriberFor<Message>()
                .Should().BeFalse(
                    "no subscribers have been added");
        }

        [Test]
        public void TrueWithExactMatch()
        {
            var bus = new MessageBus();
            bus.SubscriberFor<Message>(m => { });
            bus.HasSubscriberFor<Message>()
                .Should().BeTrue(
                    "subscriber for type was added");
        }

        [Test]
        public void FalseWithNonMatch()
        {
            var bus = new MessageBus();
            bus.SubscriberFor<TestMessageTypeA>(m => { });
            bus.HasSubscriberFor<TestMessageTypeB>()
                .Should().BeFalse(
                    "no subscriber for type was added");
        }

        [Test]
        public void TrueWithDerivedType()
        {
            var bus = new MessageBus();
            bus.SubscriberFor<TestMessageBase>(m => { });
            bus.HasSubscriberFor<TestMessageTypeA>()
                .Should().BeTrue(
                    "subscriber for super type was added");
        }

        [Test]
        public void FalseWithSuperType()
        {
            var bus = new MessageBus();
            bus.SubscriberFor<TestMessageTypeA>(m => { });
            bus.HasSubscriberFor<TestMessageBase>()
                .Should().BeFalse(
                    "subscriber for derived type was added");
        }

        [Test]
        public void CanFindExplicitTypes()
        {
            var bus = new MessageBus();
            bus.SubscriberFor<TestMessageBase>(m => { });
            bus.SubscriberFor<TestMessageTypeA>(m => { });
            bus.SubscriberFor<TestMessageTypeB>(m => { });

            bus.FindSubscribersOfExplicitType(typeof(TestMessageTypeA))
                .Should().HaveCount(1);
            bus.FindSubscribersOfExplicitType(typeof(TestMessageBase))
                .Should().HaveCount(1);
        }
    }
}
