using System.Linq;
using System.Threading;

using FluentAssertions;

using MiniBus;

using NUnit.Framework;

using Tests.TestSupport;

namespace Tests
{
    [TestFixture]
    public class MessageBusTests
    {
        private MessageBus bus;
        private Subscriber<TestMessageTypeA> subscriberA;
        private Subscriber<TestMessageTypeB> subscriberB;
        private Subscriber<TestMessageBase> subscriberC;
        private TestMessageTypeA testMessageA;
        private TestMessageTypeB testMessageB;
        private TestMessageTypeC testMessageC;

        [TestFixtureSetUp]
        public void SetUp()
        {
            bus = new MessageBus();

            var receiverForA = new TestMessageReceiver("receiverForA");
            var receiverForB = new TestMessageReceiver("receiverForB");
            var receiverForAll = new TestMessageReceiver("receiverForAll");

            subscriberA = bus.SubscriberFor<TestMessageTypeA>(receiverForA.Receive);
            subscriberB = bus.SubscriberFor<TestMessageTypeB>(receiverForB.Receive);
            subscriberC = bus.SubscriberFor<TestMessageBase>(receiverForAll.Receive);
            testMessageA = new TestMessageTypeA();
            testMessageB = new TestMessageTypeB();
            testMessageC = new TestMessageTypeC();
            bus.SendMessage(testMessageB);
            bus.SendMessage(testMessageC);

            Thread.Sleep(100);
        }

        [Test]
        public void CanInstantiateBus()
        {
            bus.Should().BeOfType<MessageBus>();
        }

        [Test]
        public void CanGetSubscriberForMessageTypeA()
        {
            subscriberA.Should().BeAssignableTo<Subscriber<TestMessageTypeA>>();
        }

        [Test]
        public void CanGetSubscriberForMessageTypeB()
        {
            subscriberB.Should().BeAssignableTo<Subscriber<TestMessageTypeB>>();
        }

        [Test]
        public void TestMessageAWasNotReceived()
        {
            testMessageA.GetReceiverLog.Should().HaveCount(0);
        }

        [Test]
        public void TestMessageBWasReceivedBySubscriberForB()
        {
            testMessageB
                .GetReceiverLog.Should()
                .Contain(_ => _.ReceiverName == "receiverForB");
        }

        [Test]
        public void TestMessageBWasReceivedBySubscriberForAll()
        {
            testMessageB
                .GetReceiverLog.Should()
                .Contain(_ => _.ReceiverName == "receiverForAll");
        }

        [Test]
        public void TestMessageBWasRecievedTwice()
        {
            testMessageB
                .GetReceiverLog.Should()
                .HaveCount(2);
        }

        [Test]
        public void TestMessageCWasReceivedBySubscriberForAll()
        {
            testMessageC
                .GetReceiverLog.Should()
                .Contain(_ => _.ReceiverName == "receiverForAll");
        }

        [Test]
        public void TestMessageCWasOnlyReceivedOnce()
        {
            testMessageC
                .GetReceiverLog.Should()
                .HaveCount(1);
        }

        [Test]
        public void TestMessageBReceivedOnDifferentThread()
        {
            testMessageB
                .GetReceiverLog.First()
                .ReceiverThreadId.Should()
                .NotBe(Thread.CurrentThread.ManagedThreadId);
        }

        [Test]
        public void TestMessageBAndCReceivedOnDifferentThreads()
        {
            var threadB = testMessageB.GetReceiverLog.First();
            var threadC = testMessageC.GetReceiverLog.First();
            threadB.Should().NotBe(threadC);
        }

    }
}
