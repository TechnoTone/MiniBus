#MiniBus
*The micro message bus for intra-app communications.*


##Purpose
Enables components of the same application to send/receive
messages to/from other components without any knowledge of
the recipients/originators.

##Basic Usage
First, create an instance of a MessageBus class:

```csharp
var bus = new MessageBus();
```

Subscribe for an implementation of the Message interface:

```csharp
var subscriber = bus.SubscriberFor<MessageTypeA>(m => {
  Console.WriteLine("Message received: " + m.ToString());
});
```

Send a message through the message bus:

```csharp
bus.SendMessage(new MessageTypeA());
```

That's it! This example demonstrates the basic principle
of the MiniBus library and shows how easy it is to use.

All you need to do is share the MessageBus with all the
different parts of your app, either yourself or via your
favourite Dependancy Injection framework, and they will
be able to send and receive messages to each other.

See the unit tests for further examples such as derived
message types, etc.
