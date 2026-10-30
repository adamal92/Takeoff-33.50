# Development guide

## Prerequisites

- Docker with the Compose plugin for the full containerized application.
- Node.js 22 and Corepack for client development.
- .NET 8 SDK for server development and tests.
- MongoDB locally, or Docker for the containerized MongoDB service.

## Start the full stack

From the repository root:

```sh
docker compose up --build
```

This serves the browser client at `http://localhost` and the API at
`http://localhost:5260`. The client image receives its API base URL at build
time. To use a different reachable API URL, set the `VITE_API_BASE_URL` build
argument in `docker-compose.yml` before building.

MongoDB's default Compose connection is `mongodb://mongodb:27017`, and data is
stored in the `mongodb-data` named volume. Set `MONGODB_CONNECTION_STRING` in
the environment to override the Compose default. For example, in PowerShell:

```powershell
$env:MONGODB_CONNECTION_STRING = '<your MongoDB connection string>'
docker compose up --build
```

Do not store real credentials in source files or commit them.

## Develop the client against Docker services

Start only MongoDB and the API from the repository root:

```sh
docker compose up --build mongodb server
```

Then, in another terminal:

```sh
cd client
corepack pnpm install --frozen-lockfile
corepack pnpm dev
```

The Vite development server is normally available at `http://localhost:5173`.
The default API base URL is `http://localhost:5260`. To override it, copy
`client/.env.example` to `client/.env.development` and edit the value:

```env
VITE_API_BASE_URL=http://localhost:5260
```

Vite reads environment variables at startup; restart the dev server after
changing the file. The development API CORS policy allows loopback origins on
any port. Keep both services on HTTP for a simple local setup. Using an HTTPS
client with an HTTP API can be blocked as mixed content; HTTPS requires
certificates and HTTPS endpoints configured on both services.

## Run the server directly

For direct server development, provide a MongoDB instance reachable from the
host, then set the server connection string before starting the `http` launch
profile. The `Local` settings file has an empty connection string by default,
so supply a valid value. In PowerShell, from the repository root:

```powershell
$env:MongoDb__ConnectionString = 'mongodb://localhost:27017'
$env:MongoDb__DatabaseName = 'map-app'
dotnet run --project server\SparkySparky\SparkySparky\SparkySparky.csproj --launch-profile http
```

The `http` profile listens at `http://localhost:5260`. The `https` profile
listens at `https://localhost:7248` and also enables HTTP on port `5260`; use
the corresponding API URL in the client. If the .NET development certificate
is not trusted, configure it with `dotnet dev-certs https --trust`.

The Visual Studio `Container (Dockerfile)` profile uses HTTP (`useSSL: false`)
and may publish a dynamically assigned host port. Use the actual HTTP port
reported by Visual Studio/Docker; do not assume the random host port serves
HTTPS.

## Configuration

### Client

| Variable | Default | Description |
| --- | --- | --- |
| `VITE_API_BASE_URL` | `http://localhost:5260` | API base URL used by the browser client. |
| `VITE_SOURCEMAP` | `false` | When exactly `true`, Vite emits source maps during a build. |

Variables prefixed with `VITE_` are included in client-side code; never put
secrets in them.

### Server

ASP.NET Core configuration uses double underscores for environment-variable
hierarchy separators:

| Variable/key | Default | Description |
| --- | --- | --- |
| `MongoDb:ConnectionString` / `MongoDb__ConnectionString` | `mongodb://localhost:27017` in base settings; the `Local` launch profile overrides it with an empty value | MongoDB connection string. |
| `MongoDb:DatabaseName` / `MongoDb__DatabaseName` | `map-app` in base settings; `combustion` in `Local` settings | Database name. |
| `MongoDb:PolygonsCollectionName` / `MongoDb__PolygonsCollectionName` | `polygons` | Polygon collection. |
| `MongoDb:ObjectsCollectionName` / `MongoDb__ObjectsCollectionName` | `objects` | Point-object collection. |

The Compose file maps `MONGODB_CONNECTION_STRING` into the API's
`MongoDb__ConnectionString` setting.

## Tests and builds

Run client tests and a production build from `client/`:

```sh
corepack pnpm test
corepack pnpm build
```

Run the backend test suite from the repository root:

```sh
dotnet test server/SparkySparky/SparkySparky.sln
```

Backend CRUD integration tests start a temporary MongoDB instance with
Mongo2Go; they do not use the application's development database. The suite
also covers geometry parsing and health-controller behavior.

## Project layout

```text
client/
  src/components/       Map, feature tools, and data table
  src/hooks/            TanStack Query data/mutation hooks
  src/services/         HTTP API client
  src/store/            Zustand interaction state
  src/types.ts          Client GeoJSON and feature types
server/SparkySparky/
  SparkySparky/         ASP.NET Core API
    Controllers/        REST and health endpoints
    DTOs/               Create/update request models
    Models/             GeoJSON, entities, and BSON serializer
    Services/           MongoDB persistence and health checks
  SparkySparky.Tests/   xUnit unit and MongoDB integration tests
docs/                   API, architecture, and development documentation
```

## Troubleshooting

### Browser reports a CORS error

Confirm the API base URL points to the host-accessible API endpoint and that
the API is running. For Docker Compose, use `http://localhost:5260`; a random
published port from the Visual Studio container profile is not necessarily
`5260`. If you change Vite environment variables, restart Vite. CORS cannot
make an HTTP port speak HTTPS, and an HTTPS page cannot make ordinary HTTP API
requests because of mixed-content restrictions.

### Polygon or object save fails

Inspect the API response and server logs. Polygon rings must have at least four
positions and be closed, and all positions must use numeric
`[longitude, latitude]` coordinates. Point geometry must contain exactly two
numeric values. Confirm that MongoDB is reachable and that its connection
string is supplied to the API.

### MongoDB data is missing after container restart

The Compose volume persists across ordinary `docker compose down` and
`docker compose up`. `docker compose down --volumes` removes it and deletes the
stored data.
