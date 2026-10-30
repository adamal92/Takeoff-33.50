# Takeoff-33.50

[![CI](https://github.com/adamal92/Takeoff-33.50/actions/workflows/ci.yml/badge.svg)](https://github.com/adamal92/Takeoff-33.50/actions/workflows/ci.yml)

Takeoff-33.50 is a geospatial dashboard for creating, viewing, and deleting map
polygons and point objects. It is a demonstration project, not an operational
system.

The application consists of a React/TypeScript client, an ASP.NET Core Web API,
and MongoDB. Spatial data is exchanged as GeoJSON using WGS 84 (EPSG:4326), with
coordinates ordered as `[longitude, latitude]`.

## Features

- Draw and save polygon areas on an interactive Leaflet map.
- Place and save point objects with marker or jeep symbols.
- Select and inspect saved features; delete them from the map data table.
- Store polygon and point data in MongoDB with 2dsphere indexes.
- Check API liveness and MongoDB connectivity with health endpoints.

The API also exposes `PUT` endpoints for updating saved features. The current
client does not yet include an edit workflow.

## Use the map

- **Draw a polygon:** choose **Add polygon**, click the map to add at least
  three vertices, choose **Finish polygon**, name the draft, and save it.
- **Place an object:** choose a name and symbol, select **Place object**, click
  the map, then save the point.
- **Inspect or delete a feature:** select it on the map or in the data table.
  Use the row's **Delete** action to remove it.

## Run the full application

Requirements: Docker with the Compose plugin.

From the repository root:

```sh
docker compose up --build
```

Open the client at <http://localhost>, the API at <http://localhost:5260>, and
the Swagger UI at <http://localhost:5260/swagger>. Compose starts MongoDB and
waits for its health check before starting the API.

The default Compose configuration uses the MongoDB container and stores data
in the `mongodb-data` named volume. To connect the API to a different MongoDB
deployment, set `MONGODB_CONNECTION_STRING` in the environment before running
Compose. Never commit credentials or connection strings.

Stop the containers with `docker compose down`. This preserves the database
volume. Removing the volume (for example, with `docker compose down --volumes`)
permanently deletes the data stored in it.

## Run the client against the containerized API

This workflow is useful when developing the client while keeping the API and
MongoDB in Docker.

1. In a terminal at the repository root, start the database and API:

   ```sh
   docker compose up --build mongodb server
   ```

2. In a second terminal, start the client:

   ```sh
   cd client
   corepack pnpm install --frozen-lockfile
   corepack pnpm dev
   ```

3. Open <http://localhost:5173>. The client defaults to
   `http://localhost:5260` for its API. To override that URL, set
   `VITE_API_BASE_URL` in `client/.env.development` (see
   [`client/.env.example`](client/.env.example)).

Use HTTP for both local client and API unless you have explicitly configured
HTTPS certificates and endpoints. See the [development guide](docs/development.md)
for configuration and troubleshooting.

## Documentation

- [Architecture and data flow](docs/architecture.md)
- [HTTP API reference](docs/api.md)
- [Development, configuration, and tests](docs/development.md)
- [Continuous integration](.github/workflows/ci.yml)

## Technology

- **Client:** React 19, TypeScript, Vite, Leaflet / React Leaflet, TanStack
  Query, Zustand, and Vitest.
- **Server:** ASP.NET Core 8, MongoDB.Driver, and Swagger / Swashbuckle.
- **Persistence:** MongoDB with GeoJSON geometry fields and 2dsphere indexes.
- **Containers:** Docker Compose, Nginx for the built client, and MongoDB 7.
