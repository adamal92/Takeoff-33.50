import { Effect } from 'effect'
import type {
  CreateMapObject,
  CreateMapPolygon,
  MapObject,
  MapPolygon,
} from '../types'

const apiBaseUrl = (import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5260')
  .replace(/\/+$/, '')

export class ApiError extends Error {
  constructor(
    message: string,
    readonly status?: number,
  ) {
    super(message)
    this.name = 'ApiError'
  }
}

function request<T>(
  path: string,
  init?: RequestInit,
): Promise<T> {
  return Effect.runPromise(
    Effect.tryPromise({
      try: async (signal) => {
        const response = await fetch(`${apiBaseUrl}${path}`, {
          ...init,
          signal,
          headers: {
            Accept: 'application/json',
            ...(init?.body ? { 'Content-Type': 'application/json' } : {}),
            ...init?.headers,
          },
        })

        if (!response.ok) {
          const details = await response.text()
          throw new ApiError(
            details || `Request failed with status ${response.status}`,
            response.status,
          )
        }

        if (response.status === 204) {
          return undefined as T
        }

        return (await response.json()) as T
      },
      catch: (error) =>
        error instanceof ApiError
          ? error
          : new ApiError(
              error instanceof Error ? error.message : 'Unable to reach the map service.',
            ),
    }),
  )
}

export const mapApi = {
  getPolygons: () => request<MapPolygon[]>('/api/polygons'),
  getObjects: () => request<MapObject[]>('/api/objects'),
  createPolygon: (polygon: CreateMapPolygon) =>
    request<MapPolygon>('/api/polygons', {
      method: 'POST',
      body: JSON.stringify(polygon),
    }),
  createObject: (mapObject: CreateMapObject) =>
    request<MapObject>('/api/objects', {
      method: 'POST',
      body: JSON.stringify(mapObject),
    }),
  deletePolygon: (id: string) =>
    request<void>(`/api/polygons/${encodeURIComponent(id)}`, {
      method: 'DELETE',
    }),
  deleteObject: (id: string) =>
    request<void>(`/api/objects/${encodeURIComponent(id)}`, {
      method: 'DELETE',
    }),
}
