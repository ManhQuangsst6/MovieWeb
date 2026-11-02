# MovieWeb

ASP.NET Core MVC movie application targeting .NET 6, with SQL Server and bundled static assets.

## Run

Install the .NET 6 SDK (validated with SDK 6.0.428/runtime 6.0.36) and SQL Server. No npm installation is needed for the bundled assets.

Create a development database named `MovieDB1`, then import `MovieWeb/ttcm.sql` using SQL Server Management Studio or `sqlcmd`. The script is UTF-16 and includes schema, sample data, views, and stored procedures; run it **once against an empty development database**, not an existing database. The database must exist before importing it. Full-text search is required for the search functionality.

Configure the connection in your process environment or .NET user secrets. All controllers, EF queries, and direct SQL commands use the same `ConnectionStrings:MovieDbContext` setting. There is no machine-specific fallback.

```sh
# Example for a local development SQL Server with a self-signed certificate.
# Replace the placeholders locally; do not commit a real password.
export ConnectionStrings__MovieDbContext='Server=127.0.0.1,1433;Database=MovieDB1;User Id=<database-user>;Password=<database-password>;Encrypt=True;TrustServerCertificate=True'

dotnet restore MovieWeb.sln
dotnet build MovieWeb.sln --no-restore
dotnet test MovieWeb.Tests/MovieWeb.Tests.csproj --no-restore
cd MovieWeb
ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://127.0.0.1:5080 \
  dotnet run --no-build --no-launch-profile
```

For a remote or production SQL Server, use a trusted server certificate and `TrustServerCertificate=False`. Windows development can still use an integrated-authentication connection string when the SQL Server and process identity support it. Linux development should use SQL authentication.

Without a connection string, startup fails with a configuration message. With an unreachable database, database-backed requests fail; the app does not silently substitute sample results. `/Home/Privacy`, `/Access/Login`, and `/Access/Register` can render without contacting the database. `/` and `/api/TheLoai/GetAll` exercise direct SQL and EF database access respectively.

The regression tests validate that EF preserves the configured SQL connection and that MVC pages render using the configured application. They do not require a database and do not validate catalog queries or login submission. For a database smoke check after importing the data, verify `/` renders the movie catalog and `/api/TheLoai/GetAll` returns the seeded genres.

## Cloud environment

The prepared SDK and NuGet cache are under `/workspace/movieweb-setup`. Activate them with:

```sh
source /workspace/movieweb-setup/activate.sh
```

Processes must be started again in a new task. Use the existing checkout; no extra Git worktree is needed. The official SQL Server container image is `mcr.microsoft.com/mssql/server:2022-latest`; its download may also require `centralus.data.mcr.microsoft.com` in the environment network settings. SQL Server must be running and the connection string configured before the catalog can be validated.
