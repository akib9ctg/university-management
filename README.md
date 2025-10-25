# University Management

Layered ASP.NET Core 9 solution for managing students, courses, and class enrolments. The backend exposes secured endpoints for staff and students, persists data with Entity Framework Core, and ships with a Docker Compose environment for PostgreSQL and pgAdmin.

## Features
- JWT authentication with role-based policies (`StaffOnly`, `StudentOnly`).
- CQRS-style request handling via MediatR.
- Entity Framework Core migrations with automatic database updates and seed data.
- Serilog-based structured logging.
- Docker Compose orchestration for the API, PostgreSQL 16, and pgAdmin 4.
- xUnit test suite covering critical application flows.

## Solution Layout
- `src/UniversityManagement.API` - ASP.NET Core Web API, Swagger, Serilog setup.
- `src/UniversityManagement.Application` - MediatR commands/queries, DTOs, validation, and domain services.
- `src/UniversityManagement.Infrastructure` - EF Core context, migrations, repositories, identity services, JWT handling.
- `src/UniversityManagement.Domain` - Aggregate roots, value objects, enums, and shared abstractions.
- `src/UniversityManagement.UnitTests` - xUnit tests with Moq and EF Core InMemory provider.
- `docker-compose.yml` - Local stack for API + PostgreSQL + pgAdmin.

## Prerequisites
- [.NET SDK 9.0](https://dotnet.microsoft.com/download) (latest patch recommended).
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) 4.x or newer (for the compose workflow).
- Optional: [pgAdmin](https://www.pgadmin.org/) or any PostgreSQL client if you prefer connecting outside the bundled pgAdmin container.

## Configuration
The API reads configuration in this order: `appsettings.json` -> `appsettings.{Environment}.json` -> environment variables. Key settings you may want to override:

- `ConnectionStrings:DefaultConnection` - PostgreSQL connection string. In Docker it is supplied via the `ConnectionStrings__DefaultConnection` environment variable (`Host=postgres;Port=5432;Database=universitydb;Username=postgres;Password=postgres`).
- `Jwt:SecretKey`, `Jwt:Issuer`, `Jwt:Audience` - symmetric signing key and token metadata.
- `ASPNETCORE_ENVIRONMENT` - determines which appsettings file to load and whether developer middleware is enabled. Compose defaults this to `Development`.

> On startup the API runs `ApplicationDbContext.Database.Migrate()` and seeds demo data if the database is empty. No manual migration step is required for local usage.

## Run with Docker Compose (Recommended)
```bash
docker compose up --build
```
Make sure Docker Desktop (or your Docker engine) is running before executing the command; otherwise `docker compose` will fail to connect to the daemon.
Also ensure that `8080` (API) and `5050` (pgAdmin) are free on your host before starting the stack—if another service is already bound to either port, Docker will fail the run step with a port binding error.
This builds the API image, starts PostgreSQL 16, and pgAdmin 4. Service endpoints:
- API: http://localhost:8080 (Swagger UI at `http://localhost:8080/swagger`)
- PostgreSQL: _internal only_ (reachable from other containers through the `postgres` host name)
- pgAdmin: http://localhost:5050 (login `akib9ctg@gmail.com` / `admin`)

> To inspect the database from the host, exec into the postgres container (`docker compose exec postgres psql -U postgres`) or use the pgAdmin container that is exposed on port 5050.

To stop the stack, run `docker compose down`. Add `-v` if you want to remove the persisted database volume (`pgdata`).

## Run the API without Docker
1. Ensure PostgreSQL is running locally (or update the connection string to point at a remote instance).
2. Update `src/UniversityManagement.API/appsettings.Development.json` (or user secrets) with your database connection string.
3. Restore dependencies and build:
   ```bash
   dotnet restore
   dotnet build
   ```
4. Start the API:
   ```bash
   dotnet run --project src/UniversityManagement.API/UniversityManagement.API.csproj
   ```
   By default the app listens on `https://localhost:7003` and `http://localhost:5267` (see `Properties/launchSettings.json`).
If those ports are occupied (for example by another API or development server), either stop the conflicting process or update `Properties/launchSettings.json` (and any reverse-proxy config) to use unused ports; otherwise Kestrel will refuse to start.

## Database Migrations
Migrations live under `src/UniversityManagement.Infrastructure/Database/Migrations`. To add a new migration:
```bash
dotnet ef migrations add <MigrationName> --project src/UniversityManagement.Infrastructure --startup-project src/UniversityManagement.API
```
Apply migrations manually (if you disabled automatic migrations) with:
```bash
dotnet ef database update --project src/UniversityManagement.Infrastructure --startup-project src/UniversityManagement.API
```

## Database Diagram
An up-to-date ERD is available at `docs/UniversityManagement_ERD.png`.

## Postman Collection
Import `UniversityManagement.postman_collection.json` (root of the repo) into Postman to explore the API. The collection includes environment-agnostic requests organised by feature area. After importing, set the `{{baseUrl}}` variable to match your running API instance (for Docker use `http://localhost:8080`, for local `dotnet run` use `https://localhost:7003` or `http://localhost:5267`).

## Default Accounts
- Staff login: `admin@university.com`
- Staff password: `Admin123!`
- Student login: `alice@student.com`
- Student password: `Student123!`
