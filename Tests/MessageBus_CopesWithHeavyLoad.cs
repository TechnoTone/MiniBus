using System;
using System.Linq;

using FluentAssertions;

using MiniBus;

using NUnit.Framework;

using Tests.TestSupport;

namespace Tests
{
  [TestFixture]
  public class MessageBus_CopesWithHeavyLoad
  {
    [Test]
    [Timeout(20000)]
    [Category("Slow")]
    public void HeavyLoadTest()
    {
      const int REPEATER_COUNT = 10;
      const int MESSAGE_COUNT = 200;

      // Arrange
      var bus = new MessageBus();

      var receivers = new MessageRepeater[REPEATER_COUNT];
      for (var i = 0; i < REPEATER_COUNT; i++)
      {
        receivers[i] = new MessageRepeater(bus, i);
        bus.SubscriberFor<RepeatingMessage>(receivers[i].Receive);
      }



      // Act
      var messages = new RepeatingMessage[MESSAGE_COUNT];
      for (var i = 0; i < MESSAGE_COUNT; i++)
      {
        messages[i] = new RepeatingMessage();
        bus.SendMessage(messages[i]);
      }

      // Assert
      var finished = false;
      while (!finished)
      {
        System.Threading.Thread.Sleep(500);
        var count = messages.Count(m => m.Counter == REPEATER_COUNT);
        Console.WriteLine(count);
        if (count == MESSAGE_COUNT)
          finished = true;
      }
    }
  }
}