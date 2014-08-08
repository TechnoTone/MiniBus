using MiniBus;

namespace Tests.TestSupport
{
  public class MessageRepeater
  {
    private readonly MessageBus bus;
    private readonly int index;

    public MessageRepeater(MessageBus bus, int index)
    {
      this.bus = bus;
      this.index = index;
    }

    public void Receive(RepeatingMessage message)
    {
      if (message.Counter != index)
        return;

      message.Counter++;

      bus.SendMessage(message);
    }
  }
}