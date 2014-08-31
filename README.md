#MiniBus
*The micro message bus for inter-app communications.*


##Purpose
So that independent components of the same application are able to send/receive messages to/from other components without any knowledge of the recipients/originators.

##Usage
First, create an instance of a MessageBus class:

```csharp
var bus = new MessageBus();
```

Subscribe for an implementation of the Message interface:

```csharp
var subscriber = bus.SubscriberFor<TestMessageTypeA>(m => {
  Console.WriteLine("Message received: " + m.ToString());
});
```

Send a message through the message bus:

```csharp
bus.SendMessage(new TestMessageTypeA());
```

That's it! This example demonstrates the basic principle of the MiniBus library shows how simple it is to use.

All you need to do is share the MessageBus with all the different parts of your app using your favourite DI framework and they will be able to send and receive messages to each other.

See the unit tests for further examples such as derived message types, etc.
