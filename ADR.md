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

## Additional notes
Wolverine has strong EF core integration as well, I could have used Wolverine in the persistence layer as well. I purposely decided to not do so.
Instead I wanted to show the Hexagonal aka Ports and Adapters approach, where the interface of outgoing ports (from the Cores perspective) is 
defined in the core project, but implemented in the Persitence project. This approach has several benefits: there is no project dependency for the core,
all dependencies are still pointing inwards, any dependency's implementation can be swapped out as long as the new implementation implements the
interface. This makes it (somewhat) technology agnostic for the Core. It also is supposed to make testing simpler as Intefaces can me mocked.
I won't write tests using mocking, because I believe these functionalities are not part of the Unit Tests scope, but belong to the Integration Tests level.
Generally I am not a fan of wrapping DbContext with the repository pattern, but this is something folks do, so I wanted to showcase it.

# Testing

## Unit Tests
This application doesn't have any invariants that you want to encapsulate in the Core layer and would be good candidates 
for classic unit testing. This application is more about checking if the simple processes can run end to end and 
I rather moved testing to the Integration Test project than going down the mocking path which I am not a fan of.

## Integration Tests
I personally prefer to use integration tests with docker whenever possible and try to keep mocking to an absolute minimum.
While I understand that mocking is sometimes useful, I think it is oversed. I see no point in mocking EF's dbcontext for example 
to write unit tests in the persistence layer. Just spin up the db in docker and move these tests to the integration test level.

I might use the term integration test a bit differently than you might be used to. My entire applciation is bootstrapped and runs in
a TestContainer, I am able to test it end to end, from calling the HTTP endpoints and use a real database where data is actually written to.
You could argue this is end to end testing or blackbox testing or whatever. A discussion about the terminus/technicus doesn't really bring
any business value. I prefer to use the term integration tests for this kind of setup.

I have came up with some validation rules and written a few tests for them. It is not meant to be complete, just a sample.

## API Testing
I think it is fair to say, that you could consider my Intgration Test setup as aslo being API and end to end test.
My personal bias is that many applications would be fine with only this kind of testing.

# API design
I plan to include API versioning with URL versioning.

For the soft delete I have decided to use the HTTP verb PATCH and not DELETE. As it is partial update (setting the isDeleted boealan) and
should be an idempotent operation PATCH seems a better fit. DELETE implies that the resource is permantently removed, which is not true here.

Validation example can be found in the CreateTests class. 

## Regarding REST (Representational State Transfer)

REST is an architectural style described by Roy Fielding. It can be found in Chapter 5 in his PhD dissertation 
"Architectural Styles and the Design of Network-based Software Architectures". 
Part of REST as described by Fielding is HATEAOS aka "Hypertext as the engine of application state" which is a fundamental concept of 
REST that ensures a client interacts with a RESTful service through dynamic hyperlinks provided in responses. I have implemented a very
simple example of it. I wish our industry would use HATEAOS more.

# Docker, Github actions

I've made a github action that runs whenever I push changes to the main branch. It builds the application and runs docker-compose up
then runs all Integration tests and publishes the test results.
I've created another docker-compose file for local testing with docker and two appsettings files to show that I can fully support
staging.

# Additional Notes

## Primitive Obsession

Depending on the project I would consider to use explicit types for cocnepts like Title, Description, ImageUrl, VideoUrl, etc.
instead of using c# primitives like string. This is known as "primitive obsession". I am aware of the concept, but considered
it overengineering for this simple assignment project.

## Auto mappers

In this project I have to do some mappings between the Core Domain Model, the DTO's used in the API layer and the Entities in the
persistence layer. I have worked with stuff like AutoMapper before, but I am not fan of it. I prefer to write my mappings manually
for the main reason that is forces me to actively think it through what and why I am mapping. Also while I am generally in favor
of using OSS, there is a certain risk associated with it.

## Options pattern

I know about it, probably did not have enough time.

# Regarding missing features

## UI
I simply do not have the time. However a few notes on how I would do it:
I would use Blazor WASM to build the UI using the MudBlazor component library. First of all I do have experience with it, I have built
a website with this stack before. MudBlazor is simple to use OSS solution which enables me to build a decently looking UI. I would
choose Blazor, because I can relly on my C# knowledge instead of having to bother with javascript.
Some things I would consider when also building a UI:
- Add cancelation tokens (on the back end as well)
- Use something like Polly for resiliency (retry mechanism)

## Authentication / Authorization
Again, lack of time. I have implemented such feature with Microsoft EntraID, Azure AD B2C and AuthO by Okta.
I have a public github repo where you can take a look at AuthO by Okta implementation:
github.com/MajorMartinDev/CamabrS