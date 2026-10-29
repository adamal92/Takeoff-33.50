# Takeoff-33.50
Takeoff-33.50 Interview clone of IAI Elta’s ELI-3350 MSISS C4I system, demonstrating geospacial data fusion and tactical dashboard architecture.

## Run with Docker Compose

From the repository root, start the client, API, and MongoDB:

```sh
docker compose up --build
```

Open the client at <http://localhost>. The API is available at
<http://localhost:5260>, and MongoDB data persists in the `mongodb-data` volume.
Set `MONGODB_CONNECTION_STRING` in the environment to use a different MongoDB
connection string. Stop the services with `docker compose down`.
