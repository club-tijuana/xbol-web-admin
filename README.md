# XBOL Admin Portal

Blazor Server application for the XBOL admin panel.

## Development Setup

This refers to development using Visual Studio 2026 on Windows, or using the .NET 10 SDK through the command line.

### Requirements

- [Visual Studio 2026](https://visualstudio.microsoft.com/insiders/) (Windows)
- [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) (Linux)

### Quick Start

In Visual Studio, press `F5` or the play button. For the command-line interface:

```bash
dotnet watch --project Odasoft.XBOL.AdminPortal/Odasoft.XBOL.AdminPortal
```

### Build & Compilation

To build the entire solution:

```bash
dotnet build Odasoft.XBOL.AdminPortal/Odasoft.XBOL.AdminPortal.slnx
```

### Secrets

Configure secrets using .NET Secret Manager:

```bash
dotnet user-secrets set "SeatsIo:SecretKey" "YOUR_KEY" --project Odasoft.XBOL.AdminPortal/Odasoft.XBOL.AdminPortal
```

List configured secrets:

```bash
dotnet user-secrets list --project Odasoft.XBOL.AdminPortal/Odasoft.XBOL.AdminPortal
```

### Configuration

Edit `appsettings.Development.json` for local settings (API base address, authentication, etc.). Settings cascade: `appsettings.json` -> `appsettings.{Environment}.json` -> environment variables. All settings are validated at startup.

IDE autocomplete is provided by `appsettings.schema.json`, which regenerates automatically on Debug builds.

## Deployment

The container is production-ready with:

- **Security**: Non-root `app` user
- **Optimization**: Release build with ReadyToRun compilation
- **Health checks**: Automatic container health monitoring
- **Restart policy**: `unless-stopped` for high availability
- **Environment**: `ASPNETCORE_ENVIRONMENT=Production`

#### Requirements

- Make
- [Podman](https://podman.io/) (or [Docker](https://www.docker.com/))
- [Podman Compose](https://docs.podman.io/en/latest/markdown/podman-compose.1.html) (or [Docker Compose](https://docs.docker.com/compose/))

#### Usage

```bash
make build    # Create the Docker container
make run      # Run the Docker Compose environment
```

**Access the containerized services**

- **Base URL**: <http://localhost:8080>
- **Health Check**: <http://localhost:8080/healthz>

#### GCP Secrets

Runtime configuration is stored in GCP Secret Manager. Each environment has a dedicated secret:

| Secret                      | Contents                          |
| --------------------------- | --------------------------------- |
| `dev-xbol-web-admin-secret` | App configuration (API URL, keys) |

The app secret stores environment variables using ASP.NET's `__` (double underscore) convention for nested config:

```json
{
    "AdminApiClient__BaseAddress": "https://dev-api.admin.pwrticket.mx",
    "SeatsIo__SecretKey": "<seats.io secret key>",
    "Authentication__AllowedUsers__0__Email": "admin@xbol.com",
    "Authentication__AllowedUsers__0__Password": "P@ssw0rd1234"
}
```

To update:

```bash
gcloud secrets versions add dev-xbol-web-admin-secret --data-file=- <<'EOF'
{ ... }
EOF
```

QA secrets follow the same pattern with a `qa-` prefix.
