import type { LatLng } from 'leaflet'

export type Position = [longitude: number, latitude: number]

export interface PointGeometry {
  type: 'Point'
  coordinates: Position
}

export interface PolygonGeometry {
  type: 'Polygon'
  coordinates: Position[][]
}

export interface MapObject {
  id: string
  name: string
  symbolType: string
  geometry: PointGeometry
  createdAt: string
}

export interface MapPolygon {
  id: string
  name: string
  geometry: PolygonGeometry
  createdAt: string
}

export interface CreateMapObject {
  name: string
  symbolType: string
  geometry: PointGeometry
}

export interface CreateMapPolygon {
  name: string
  geometry: PolygonGeometry
}

export type FeatureType = 'object' | 'polygon'

export interface FeatureSelection {
  type: FeatureType
  id: string
}

export interface PolygonDraft {
  name: string
  positions: LatLng[]
  complete: boolean
}
