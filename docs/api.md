# HTTP API reference

The local Compose API base URL is `http://localhost:5260`. Swagger UI is
available at `/swagger`.

All request and response bodies use JSON. API JSON property names are
camel-cased. IDs are MongoDB ObjectId strings, and `createdAt` values are UTC
timestamps.

## Polygons

### List polygons

```http
GET /api/polygons
```

Returns `200 OK` with polygons ordered newest first:

```json
[
  {
    "id": "65a000000000000000000001",
    "name": "Area A",
    "geometry": {
      "type": "Polygon",
      "coordinates": [
        [
          [34.78, 32.08],
          [34.79, 32.08],
          [34.79, 32.09],
          [34.78, 32.08]
        ]
      ]
    },
    "createdAt": "2026-10-28T12:00:00Z"
  }
]
```

### Create a polygon

```http
POST /api/polygons
Content-Type: application/json
```

Request:

```json
{
  "name": "Area A",
  "geometry": {
    "type": "Polygon",
    "coordinates": [
      [
        [34.78, 32.08],
        [34.79, 32.08],
        [34.79, 32.09],
        [34.78, 32.08]
      ]
    ]
  }
}
```

Returns `201 Created` with the created record, including its generated `id`
and `createdAt`. A polygon must have type `Polygon`, at least one ring, and
closed rings with at least four positions. Every position must have exactly two
numeric coordinates.

### Update a polygon

```http
PUT /api/polygons/{id}
Content-Type: application/json
```

The request body has the same shape as polygon creation. Returns `204 No
Content` when updated, or `404 Not Found` when the ID is invalid or does not
exist. Updating a polygon changes its name and geometry; its original
`createdAt` is retained.

### Delete a polygon

```http
DELETE /api/polygons/{id}
```

Returns `204 No Content` when deleted, or `404 Not Found` when the ID is invalid
or does not exist.

## Point objects

### List objects

```http
GET /api/objects
```

Returns `200 OK` with objects ordered newest first. Each object includes `id`,
`name`, `symbolType`, `geometry`, and `createdAt`.

### Create an object

```http
POST /api/objects
Content-Type: application/json
```

Request:

```json
{
  "name": "Jeep Alpha",
  "symbolType": "jeep",
  "geometry": {
    "type": "Point",
    "coordinates": [34.7818, 32.0853]
  }
}
```

Returns `201 Created` with the created record. Geometry must have type `Point`
and exactly two numeric coordinates. The current client offers `marker` and
`jeep` symbol choices; the API accepts a required `symbolType` string.

### Update an object

```http
PUT /api/objects/{id}
Content-Type: application/json
```

The request body has the same shape as object creation. Returns `204 No
Content` when updated, or `404 Not Found` when the ID is invalid or does not
exist. Updating an object changes its name, symbol type, and geometry; its
original `createdAt` is retained.

### Delete an object

```http
DELETE /api/objects/{id}
```

Returns `204 No Content` when deleted, or `404 Not Found` when the ID is invalid
or does not exist.

## Health

| Method | Path | Purpose | Success |
| --- | --- | --- | --- |
| `GET` | `/api/health/is-alive` or `/health/is-alive` | Process liveness; does not check MongoDB | `200` with `{"status":"alive"}` |
| `GET` | `/api/health` or `/health` | MongoDB readiness check | `200` with `{"status":"healthy"}` or `503` with `{"status":"unhealthy"}` |

## Errors and validation

- Invalid geometry returns `400 Bad Request`.
- Invalid or missing IDs on update/delete return `404 Not Found`.
- ASP.NET Core model-binding/required-field errors are also `400 Bad Request`.
- Database and unexpected server errors are not converted into successful
  responses; inspect API logs for details.

## GeoJSON rules

Coordinates are longitude-first in WGS 84 / EPSG:4326. Polygon rings are
closed: their first and last positions must match and each ring must contain at
least four positions. The API currently checks structural validity, not
longitude/latitude ranges or winding order.
