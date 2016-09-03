# SimpleEventManager
Simple event manager that I'm using for a small tower defense game in C#.

## Console App Print Example

Create a new "PrintMsgEvent". This event derives from BaseEvent and contains a simple string, which is a message that must be printed.

```
class PrintMsgEvent : BaseEvent
{
    public string Message;

    public PrintMsgEvent(string message)
    {
        Message = message;
    }
}
```

Now create a simple test program that listens for this event being fired and handles it appropriately.

```
// Create an event manager
var eventManager = new EventManager();

// Create a listener that prints messages on a PrintMsgEvent being fired
var listener = eventManager.Subscribe<PrintMsgEvent>(
    msgEvent => Console.WriteLine(msgEvent.Message));
```

Now fire an event so you can see it being handled.

```
// Invoke a print message event immediately
eventManager.Invoke(new PrintMsgEvent("Did you print this message?"));
```

This is the basics of using SimpleEvents. 

You can unsubscribe from the event by using the same object that was returned from the Subscribe method call.

```
// Now remove the listener & invoke again
eventManager.Unsubscribe(listener);
```

You can also use the QueueInvoke and InvokeQueuedEvents method. You can use this for delaying event invokation. The best practice would be to call InvokeQueuedEvents every frame. 

For example:

```
// Queue a few print message events
eventManager.QueueInvoke(new PrintMsgEvent("1st queued message!"));
eventManager.QueueInvoke(new PrintMsgEvent("2nd queued message!"));
eventManager.QueueInvoke(new PrintMsgEvent("3rd queued message!"));
// Invoke all of the queued events
eventManager.InvokeQueuedEvents();
```
