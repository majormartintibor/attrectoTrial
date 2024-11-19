SW Architecture:

I am using a Hexagonal architecture with ports and adapters. At the center is the Core project which does contain the business logic and domain models.
All Interfaces must be defined in the core regarding of direction (inward, outward).
The Core may not have any project dependencies.

I am also using the mediator pattern, I use an in-process message bus and use messages to communicate between the API and Core project. 
This makes the application more decoupled and enables easy refactoring if a real message broker will be needed in the future. All this
without increasing the complexity of the system. 
The mssages also serve as contracts.

About the Nuget Packages / 3rd Party Libraries

Alba is basically a wrapper around ASP.NET Core TestServer and provides a pleasent to work with API. 
In combination with Xunit and docker it enables integration testing with simple setup.

Wolverine is a messaging and web development framework that has good integration with many other tools from RabbitMQ, Kafka to Amazon SQS as well
as a strong integration with the Marten libary and PostgreSQL. Wolverine is built with the mindset of reducing boilerplate code 
and reducing complexity, making it easier to reason about the code and the actual business use cases.

Wolverine, Marten and Alba are all part of the so called "Critter Stack". It has a very stable community and is open source, but also has a company
called JaperFX on top of it. It is here to stay for a long time and has very ambitous goals.
I like this stack, because while it does enable me to build classic CRUD style systems with a relational database it also opens up the
possibility to use PostgreSQL as a document store or even build full blown event sourced applications and turning PostgreSQL into an event store.
Further more I even have the option to use the same database as relational db, document db and event store at the same time!