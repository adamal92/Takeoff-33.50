import { afterEach, describe, expect, it, vi } from 'vitest'
import { ApiError, mapApi } from './api'

describe('mapApi', () => {
  afterEach(() => {
    vi.unstubAllGlobals()
  })

  it('posts polygon geometry using the REST contract', async () => {
    const polygon = {
      id: 'polygon-1',
      name: 'Area A',
      geometry: {
        type: 'Polygon' as const,
        coordinates: [[[34, 32], [35, 33], [34, 32]]] as [number, number][][],
      },
      createdAt: '2026-10-28T12:00:00Z',
    }
    const fetchMock = vi.fn().mockResolvedValue(
      new Response(JSON.stringify(polygon), { status: 201 }),
    )
    vi.stubGlobal('fetch', fetchMock)

    await expect(
      mapApi.createPolygon({ name: 'Area A', geometry: polygon.geometry }),
    ).resolves.toEqual(polygon)
    expect(fetchMock).toHaveBeenCalledWith(
      'http://localhost:5260/api/polygons',
      expect.objectContaining({
        method: 'POST',
        body: JSON.stringify({ name: 'Area A', geometry: polygon.geometry }),
      }),
    )
  })

  it('surfaces server errors instead of hiding them', async () => {
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue(new Response('Invalid polygon', { status: 400 })),
    )

    await expect(
      mapApi.createPolygon({
        name: 'Bad',
        geometry: { type: 'Polygon', coordinates: [] },
      }),
    ).rejects.toMatchObject<ApiError>({
      name: 'ApiError',
      status: 400,
      message: 'Invalid polygon',
    })
  })

  it('supports successful no-content deletes', async () => {
    const fetchMock = vi.fn().mockResolvedValue(new Response(null, { status: 204 }))
    vi.stubGlobal('fetch', fetchMock)

    await expect(mapApi.deleteObject('a/b')).resolves.toBeUndefined()
    expect(fetchMock).toHaveBeenCalledWith(
      'http://localhost:5260/api/objects/a%2Fb',
      expect.objectContaining({ method: 'DELETE' }),
    )
  })
})
