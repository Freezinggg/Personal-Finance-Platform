# Personal Finance Platform
A production-oriented backend application built with ASP.NET Core following Clean Architecture, Domain-Driven Design (DDD) principles, and modern backend engineering practices.
This project is part of my journey from Backend Developer to Backend Engineer, focusing on designing business-driven systems rather than simply implementing CRUD APIs.

## Overview
Managing personal finances is more than storing transactions and creating virtual wallet.

The goal of this project is to build a backend product that imitate real financial workflows while demonstrating software engineering practices used in production systems.

The project emphasizes:

- Business-driven domain modeling
- Clean Architecture
- Domain-Driven Design (DDD) principles
- Testable application design

Rather than building everything upfront, features are developed sprint by sprint as in a real software team.

## Goals

## Current Status

## Tech Stack
**Backend**
- ASP.NET Core
- .NET 10
- C#
**Database**
- PostgreSQL
- Entity Framework Core
  
**Architecture**
- Clean Architecture
- Domain-Driven Design (DDD)
- Repository Pattern
- Unit of Work
- Monolith
  
**Tooling**
- Visual Studio
- Git
- GitHub
- Docker (planned)

## Project Structure
**Presentation (API) > Application > Domain > Infrastructure > PostgreSQL**

The Domain layer contains the business rules and remains independent of frameworks and infrastructure.
The Application layer coordinates use cases.
The Infrastructure layer provides technical implementations such as Entity Framework Core repositories.

src/ 
  ├── PersonalFinancePlatform.API 
  ├── PersonalFinancePlatform.Application 
  ├── PersonalFinancePlatform.Domain 
  └── PersonalFinancePlatform.Infrastructure 
tests/ 
  └── PersonalFinancePlatform.Tests

## Features

## Roadmap
**Sprint 0**
- Engineering Bootstrap
- Solution Structure
- Clean Architecture
- Repository Setup
**Sprint 1**
- Authentication
- User Registration
- Wallet Domain
- Transaction Domain
**Future Sprints**
- Categories
- Financial Reports
- Budgeting
- Recurring Transactions
- Background Workers
- Notifications
- Performance Improvements

## Learning Objectives
This repository intentionally focuses on learning production-oriented backend engineering, including:

- Domain-Driven Design
- Clean Architecture
- Aggregate Design
- Value Objects
- Entity Modeling
- Repository Pattern
- Transaction Management
- Database Design
- Software Architecture

Future iterations will introduce topics such as:

- CQRS
- Outbox Pattern
- Background Processing
- Redis / Distributed Caching
- Message Brokers
- Observability
- Event-Driven Architecture

## License
This project is created for educational and portfolio purposes. Expect some minor frequent changes and bugs.
