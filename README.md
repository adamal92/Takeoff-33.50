# Takeoff-33.50

Takeoff-33.50 is a full-stack geospatial dashboard built as an interview project for IAI. It demonstrates a map-centric workflow inspired by C4I systems: users can create, view, select, and delete map objects and polygon areas. It is a demonstration project, not an operational IAI system.

## Project overview

- **Client:** React, TypeScript, and Leaflet provide an interactive map centered on Tel Aviv, tools for placing point objects and drawing polygon boundaries, and a table of saved features. React Query manages API data and mutations; Zustand holds the current tool, drafts, and selection.
- **Server:** An ASP.NET Core 8 Web API exposes endpoints for listing, creating, and deleting objects and polygons. It validates GeoJSON geometries and persists the features in MongoDB, with 2dsphere indexes on their geometries.
- **Spatial data:** Features use GeoJSON in WGS 84 (EPSG:4326), with coordinates ordered as longitude, latitude.
- **Deployment:** Docker Compose starts the client, API, and MongoDB together. MongoDB data is retained in a named volume.

## Run with Docker Compose

From the repository root, start the client, API, and MongoDB:

```sh
docker compose up --build
```

Open the client at <http://localhost>. The API is available at
<http://localhost:5260>. MongoDB data persists in the `mongodb-data` volume.
Set `MONGODB_CONNECTION_STRING` in the environment to use a different MongoDB
connection string. Stop the services with `docker compose down`.
