# Chorus Chassis Framework for C# .NET Core Microservices
## Purpose
The purpose of this project is to provide an **opinionated** framework for building C# .NET Core microservices, treating **CQRS**, **Kafka**, and **events** as 1st class citizens. Using this framework will allow you to easily create microservices that fit into best practices for CQRS and event-driven systems built on Kafka.

## Design and Architecture
Chorus is designed using a **Hexagonal Architecture**. All infrastructure or technology-specific features are designed to be pluggable, and are abstracted by carefully crafted interfaces. The purpose is not to support as many possible technologies right away, but rather so that it is as easy as possible for anyone to add their own concrete implementations.

## Roadmap
Planned features (in no particular order):
- [ ] Saga builder w/ outbox pattern
- [ ] CQRS base classes and interfaces
- [ ] Kafka consumers and producers for events and commands, with OOTB support for OpenTracing
- [ ] Event base classes and interfaces
- [ ] API gateway
- [ ] Authentication / authorization
- [ ] Logging
- [ ] Configuration
- [ ] Distributed Tracing

## FAQ
- **Why the name Chorus?** 1) Try to pronounce CQRS if it weren't an acronym. See? 2) One of the definitions of "chorus" on Google is "a large organized group of singers, especially one that performs together with an orchestra or opera company." This is a nice analogy for microservices, which if done correctly can be an amazing showcase of orchestration and coordination.
- **Why is there no support for RabbitMQ?** We have noticed a significant gap in support for Kafka tooling when it comes to C#. For example, it's really difficult to find a Saga implementation on top of Kafka. It's also difficult to find libraries that support distributed tracing for Kafka. It's possible that RabbitMQ is supported in the future, but for now Kafka is the priority.
