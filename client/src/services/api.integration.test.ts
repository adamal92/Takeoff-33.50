import { describe, expect, it } from 'vitest'
import { mapApi } from './api'

const integrationEnabled = import.meta.env.VITE_RUN_API_INTEGRATION_TESTS === 'true'
const apiBaseUrl = (import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5260')
  .replace(/\/+$/, '')

describe.skipIf(!integrationEnabled)('mapApi against the running API', () => {
  it('round-trips client features through the API and MongoDB', async () => {
    const suffix = `${Date.now()}-${Math.random().toString(36).slice(2)}`
    let polygonId: string | undefined
    let objectId: string | undefined

    try {
      const healthResponse = await fetch(
        `${apiBaseUrl}/api/health`,
      )
      expect(healthResponse.status).toBe(200)
      await expect(healthResponse.json()).resolves.toEqual({ status: 'healthy' })

      const polygonInput = {
        name: `Integration polygon ${suffix}`,
        geometry: {
          type: 'Polygon' as const,
          coordinates: [[[34, 32], [35, 32], [35, 33], [34, 32]]] as [number, number][][],
        },
      }
      const objectInput = {
        name: `Integration object ${suffix}`,
        symbolType: 'jeep',
        geometry: { type: 'Point' as const, coordinates: [34.7818, 32.0853] as [number, number] },
      }

      const createdPolygon = await mapApi.createPolygon(polygonInput)
      polygonId = createdPolygon.id
      const createdObject = await mapApi.createObject(objectInput)
      objectId = createdObject.id

      expect(createdPolygon).toMatchObject(polygonInput)
      expect(createdPolygon.id).toMatch(/^[a-f\d]{24}$/i)
      expect(Date.parse(createdPolygon.createdAt)).not.toBeNaN()
      expect(createdObject).toMatchObject(objectInput)
      expect(createdObject.id).toMatch(/^[a-f\d]{24}$/i)
      expect(Date.parse(createdObject.createdAt)).not.toBeNaN()

      const [polygons, objects] = await Promise.all([
        mapApi.getPolygons(),
        mapApi.getObjects(),
      ])
      expect(polygons).toContainEqual(expect.objectContaining({
        id: createdPolygon.id,
        name: polygonInput.name,
        geometry: polygonInput.geometry,
      }))
      expect(objects).toContainEqual(expect.objectContaining({
        id: createdObject.id,
        name: objectInput.name,
        symbolType: objectInput.symbolType,
        geometry: objectInput.geometry,
      }))
    } finally {
      if (polygonId) await mapApi.deletePolygon(polygonId)
      if (objectId) await mapApi.deleteObject(objectId)
    }
  })
})
