# 🚀 Task and Team Management System

A **robust, modular, and scalable web application** built with **ASP.NET Core (.NET 8)**, following a **Clean Architecture** approach, and powered by **MediatR** for streamlined request handling.  

The system enables seamless **task and team management**, with **real-time collaboration**, **secure authentication (including Google login)**, and **enterprise-grade observability and performance**.  

---

## 🌟 Key Highlights

- **Real-time notifications**
- **Caching** for optimized performance  
- **gRPC** for high-performance service-to-service communication  
- **SMTP mail notifications** for alerts and reminders  
- **Advanced logging & monitoring** with **ELK integration**  
- **Event-driven messaging queues** for resilience and scalability  
- **Comprehensive testing** (unit & integration) to ensure reliability  
- **Automated CI/CD pipeline** for seamless deployment  

---

## 🔑 Features

- **User Authentication & Authorization**
  - ASP.NET Core Identity integration  
  - JWT-based authentication & refresh tokens  
  - Google OAuth login support
  - Role, policy, and attribute-based authorization

- **Task Management**
  - Full CRUD operations for tasks  
  - Task assignment and status tracking  

- **Team Management**
  - Create, update, and list teams  
  - Assign users to teams  

- **Event-Driven Architecture**
  - RabbitMQ integration for task-related events  

- **Real-time notification **
  - SignalR for instant updates

- **Robust Middleware**
  - Request/response logging  
  - Centralized exception handling  

- **Testing**
  - Unit and integration tests for core features  

---

## 📂 Project Structure

> The solution follows **Clean Architecture**, separating concerns into distinct layers:  
- **API Layer** (Controllers, Authentication, Swagger)  
- **Application Layer** (CQRS with MediatR, DTOs, Business Rules)  
- **Domain Layer** (Entities, Aggregates, Core Logic)  
- **Infrastructure Layer** (EF Core, Identity, RabbitMQ, SMTP, Caching, Logging)  
- **Tests Layer** (Unit & Integration tests)  

---

## 🛠️ Getting Started

### ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
- SQL Server or compatible database  
- Docker & Docker Compose


### ⚡ Setup

1. **Start dependencies**:  
   ```bash
   docker-compose up -d
   ```
   Spins up SQL Server, RabbitMQ, Redis, and ELK Stack.

2. **Configure database**:  
   Update `appsettings.json` connection string and apply migrations:  
   ```bash
   dotnet ef database update --project Api

3. **Run the app**:  
   ```bash
   dotnet run --project Api
   ```

4. **API Documentation**:  
   Swagger UI at `https://localhost:{port}/swagger`


### Key Endpoints
- **POST** `/api/auth/login` — Standard login
- **POST** `/api/auth/google-login` — Google OAuth login
- **POST** `/api/taskitem` — Create a new task
- **GET** `/api/taskitem/{id}` — Get task by ID
- **GET** `/api/taskitem` — List all tasks
- **POST** `/api/team` — Create a new team
- **GET** `/api/team` — List all teams

## 🛠️ Tech Stack
- ASP.NET Core 8
- MediatR (CQRS)
- Entity Framework Core
- ASP.NET Core Identity
- JWT Authentication & Google OAuth
- SignalR (real-time notifications)
- gRPC (service-to-service communication)
- RabbitMQ (event-driven messaging)
- Redis (caching)
- FluentValidation
- xUnit (testing)
- ELK Stack (Elasticsearch, Logstash, Kibana) for logging & search

## 🚀 Future Enhancements
- GraphQL API support for flexible queries
- ELK Index-based search for faster and richer task queries
- Grafana & Prometheus for real-time monitoring and metrics
- Distributed Tracing (Jaeger / OpenTelemetry) for debugging microservices
- Kubernetes Deployment with Helm charts for cloud-native scaling
- Service Mesh (Istio/Linkerd) for secure, observable service communication

## 🤝 Contributing
Contributions are welcome! 🎉  
Please open an issue or submit a pull request for improvements.
