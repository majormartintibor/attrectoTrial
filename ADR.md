# SW Architecture:

I am using a Hexagonal architecture with ports and adapters. At the center is the Core project which does contain the business logic and domain models.
All Interfaces must be defined in the core regarding of direction (inward, outward).
The Core may not have any project dependencies.

I am also using the mediator pattern, I use an in-process message bus and use messages to communicate between the API and Core project. 
This makes the application more decoupled and enables easy refactoring if a real message broker will be needed in the future. All this
without increasing the complexity of the system. 
The mssages also serve as contracts.

## About the Nuget Packages / 3rd Party Libraries

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

# Testing

## Unit Tests
I am not a fan of bloated unit test projects. I think a lot of testing belongs to the integration test level.

## Integration Tests
I personally prefer to use integration tests with docker whenever possible and try to keep mocking to an absolute minimum.
While I understand that mocking is sometimes useful, I think it is oversed. I see no point in mocking EF's dbcontext for example 
to write unit tests in the persistence layer. Just spin up the db in docker and move these tests to the integration test level.

I might use the term integration test a bit differently than you might be used to. My entire applciation is bootstrapped and runs in
a TestContainer, I am able to test it end to end, from calling the HTTP endpoints and use a real database where data is actually written to.
You could argue this is end to end testing or blackbox testing or whatever. A discussion about the terminus/technicus doesn't really bring
any business value. I prefer to use the term integration tests for this kind of setup.

# Regarding REST (Representational State Transfer)

Unfortunately our industry doesn't really know what REST actually means and where the term originates from. REST is an architectural style
described by Roy Fielding. It can be found in Chapter 5 in his PhD dissertation 
"Architectural Styles and the Design of Network-based Software Architectures". 
Part of REST as described by Fielding is HATEAOS aka "Hypertext as the engine of application state" which is a fundamental concept of 
REST that ensures a client interacts with a RESTful service through dynamic hyperlinks provided in responses. I have implemented a very
simple example of it. I wish our industry would use HATEAOS more.