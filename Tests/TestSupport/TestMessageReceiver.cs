namespace Tests.TestSupport
{
    public class TestMessageReceiver
    {
        private readonly string name;

        public TestMessageReceiver(string name)
        {
            this.name = name;
        }

        public void Receive(TestMessageBase message)
        {
            message.Received(name);
        }
    }
}