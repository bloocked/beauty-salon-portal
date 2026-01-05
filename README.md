# Beauty Salon Portal

A web-based salon reservation system that allows clients to browse salons and book appointments, while providing administrators with tools to manage services and specialists.
The frontend is implemented using vanilla HTML, CSS, and JavaScript to focus on core web fundamentals.

## Features

- **User Management** - Registration and authentication with JWT
- **Salon Management** - Browse salons by city
- **Services & Specialists** - View available services and specialists
- **Reservations** - Book appointments with specialists
- **Admin Panel** - Manage salons, services, and specialists

## Tech Stack

- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core with SQLite
- HTML/CSS + JavaScript
- JWT Authentication
- NSwag (OpenAPI/Swagger)

### Prerequisites

- .NET 9.0 SDK

### Installation

1. Clone the repository
2. Navigate to the `api` folder
3. Restore dependencies:
   ```bash
   dotnet restore
   ```

### Running the Application

```bash
dotnet run
```

The API will be available at `https://localhost:7175` (or the port specified in [launchSettings.json](Properties/launchSettings.json)).

### Database

The application uses SQLite with automatic seeding. The database file `dbase.db` will be created automatically on first run.
