# archii_assignment


Just went with the easiest way using EventAggregator in C# .

Why?

Because it allows implementing a basic pattern variation of the Observer design pattern ( in here, the Observer is the instance if EventAggregator ; receives the notes from publisher, multicast to subscribed subscribers ).
An other advantage is the loose coupling between pub & sub ( subscribers don't need to know publishers when subscribing ).
