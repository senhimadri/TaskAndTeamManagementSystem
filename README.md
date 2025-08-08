# Task and Team Management System

A robust, modular web application for managing tasks and teams, built with ASP.NET Core (.NET 8), MediatR, and a clean architecture approach. The system supports user authentication (including Google login), task and team management, and integrates with message queues for event-driven features.

## Features

- **User Authentication**
  - Standard login and Google OAuth support
  - JWT-based authentication and refresh tokens

- **Task Management**
  - CRUD operations for tasks
  - Task assignment and status tracking

- **Team Management**
  - Create, update, and list teams
  - Assign users to teams

- **Event-Driven Architecture**
  - Message queue integration for task events

- **Robust Middleware**
  - Request/response logging
  - Global exception handling

- **Testing**
  - Unit and integration tests for core features

## Project Structure

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or compatible database
- (Optional) RabbitMQ or other message queue for event-driven features

### Setup

1. **Clone the repository:**

2. **Configure the database:**
   - Update the connection string in `appsettings.json` under the `Api` project.

3. **Apply migrations:**
   - Run the following command in the `Api` project directory:
	 ```bash
	 dotnet ef database update
	 ```

5. **API Documentation:**
   - Swagger UI is available at `https://localhost:{port}/swagger` when running locally.

## Key Endpoints

- `POST /api/auth/login` — User login
- `POST /api/auth/google-login` — Google OAuth login
- `POST /api/taskitem` — Create a new task
- `GET /api/taskitem/{id}` — Get task by ID
- `GET /api/taskitem` — List all tasks
- `POST /api/team` — Create a new team
- `GET /api/team` — List all teams

## Technologies Used

- ASP.NET Core 8
- MediatR (CQRS)
- Entity Framework Core
- JWT Authentication
- FluentValidation
- RabbitMQ (for message queue)
- xUnit (testing)

## Contributing

Contributions are welcome! Please open issues or submit pull requests for improvements.

## License

This project is licensed under the MIT License.