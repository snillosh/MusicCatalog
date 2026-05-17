# Development Setup

This project uses ASP.NET Core User Secrets for local development configuration. These settings can be setup using appsettings.Development.json too.

## Initialize User Secrets

Run from the solution root:

```
dotnet user-secrets init --project src/MusicCatalog.Api
```

## Configure Database Connection
```
dotnet user-secrets set "ConnectionStrings:MusicCatalog" "Host=localhost;Port=5433;Database=musiccatalog;Username=music;Password=music" --project src/MusicCatalog.Api
```
## Configure JWT Authentication

Make sure that the JWT key is greater then 256 bits.

```
dotnet user-secrets set "Jwt:Issuer" "MusicCatalog.Api" --project src/MusicCatalog.Api

dotnet user-secrets set "Jwt:Audience" "MusicCatalog.Client" --project src/MusicCatalog.Api

dotnet user-secrets set "Jwt:Key" "this-is-a-local-development-jwt-signing-key-123456789" --project src/MusicCatalog.Api

dotnet user-secrets set "Jwt:ExpiryMinutes" "60" --project src/MusicCatalog.Api
```
## View Current User Secrets
```
dotnet user-secrets list --project src/MusicCatalog.Api
```
## Create a New Migration
```
dotnet ef migrations add <MigrationName> --project src/MusicCatalog.Infrastructure --startup-project src/MusicCatalog.Api
  ```

## Run Database Migrations
```
dotnet ef database update --project src/MusicCatalog.Infrastructure --startup-project src/MusicCatalog.Api
  ```
## Start PostgreSQL

To start a development database, spin up the docker container using docker compose using the command below from the docker folder in the repo root.

```
docker compose up -d
```
