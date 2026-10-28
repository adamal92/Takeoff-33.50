import L from 'leaflet'
import { useEffect } from 'react'
import {
  MapContainer,
  Marker,
  Polygon,
  Polyline,
  TileLayer,
  Tooltip,
  useMap,
  useMapEvents,
} from 'react-leaflet'
import { useMapStore } from '../store/mapStore'
import type { MapObject, MapPolygon, Position } from '../types'

const objectIcons: Record<string, L.DivIcon> = {
  marker: L.divIcon({
    className: 'object-marker object-marker--marker',
    html: '<span>M</span>',
    iconSize: [30, 30],
    iconAnchor: [15, 15],
  }),
  jeep: L.divIcon({
    className: 'object-marker object-marker--jeep',
    html: '<span>J</span>',
    iconSize: [30, 30],
    iconAnchor: [15, 15],
  }),
}

const toLatLng = (position: Position): [number, number] => [position[1], position[0]]

function MapInteractions() {
  const activeTool = useMapStore((state) => state.activeTool)
  const addPolygonVertex = useMapStore((state) => state.addPolygonVertex)
  const placeObject = useMapStore((state) => state.placeObject)

  useMapEvents({
    click(event) {
      if (activeTool === 'polygon') addPolygonVertex(event.latlng)
      if (activeTool === 'object') placeObject(event.latlng)
    },
  })

  return null
}

function MapSelectionFocus({
  objects,
  polygons,
}: {
  objects: MapObject[]
  polygons: MapPolygon[]
}) {
  const map = useMap()
  const selection = useMapStore((state) => state.selection)

  useEffect(() => {
    if (!selection) return
    if (selection.type === 'object') {
      const selectedObject = objects.find((object) => object.id === selection.id)
      if (selectedObject) map.flyTo(toLatLng(selectedObject.geometry.coordinates), 13)
      return
    }

    const selectedPolygon = polygons.find((polygon) => polygon.id === selection.id)
    const firstRing = selectedPolygon?.geometry.coordinates[0]
    if (firstRing?.length) {
      map.fitBounds(firstRing.map(toLatLng), { padding: [32, 32], maxZoom: 14 })
    }
  }, [map, objects, polygons, selection])

  return null
}

interface MapCanvasProps {
  objects: MapObject[]
  polygons: MapPolygon[]
}

export function MapCanvas({ objects, polygons }: MapCanvasProps) {
  const activeTool = useMapStore((state) => state.activeTool)
  const polygonDraft = useMapStore((state) => state.polygonDraft)
  const objectDraft = useMapStore((state) => state.objectDraft)
  const selection = useMapStore((state) => state.selection)
  const selectFeature = useMapStore((state) => state.selectFeature)

  const draftPositions = polygonDraft?.positions.map((point) => [point.lat, point.lng] as [number, number]) ?? []
  const cursorClass =
    activeTool === 'polygon' ? 'map-canvas map-canvas--drawing' :
      activeTool === 'object' ? 'map-canvas map-canvas--placing' : 'map-canvas'

  return (
    <MapContainer
      center={[32.0853, 34.7818]}
      zoom={11}
      className={cursorClass}
      scrollWheelZoom
      doubleClickZoom={activeTool !== 'polygon'}
      aria-label="Interactive map"
    >
      <TileLayer
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />
      <MapInteractions />
      <MapSelectionFocus objects={objects} polygons={polygons} />
      {polygons.map((polygon) => (
        <Polygon
          key={polygon.id}
          positions={polygon.geometry.coordinates.map((ring) => ring.map(toLatLng))}
          pathOptions={{
            color: selection?.id === polygon.id ? '#c6f36a' : '#8dbd70',
            fillColor: '#8dbd70',
            fillOpacity: 0.18,
            weight: selection?.id === polygon.id ? 3 : 2,
          }}
          eventHandlers={{
            click: () => selectFeature({ type: 'polygon', id: polygon.id }),
          }}
        >
          <Tooltip>{polygon.name}</Tooltip>
        </Polygon>
      ))}
      {objects.map((object) => (
        <Marker
          key={object.id}
          position={toLatLng(object.geometry.coordinates)}
          icon={objectIcons[object.symbolType] ?? objectIcons.marker}
          eventHandlers={{
            click: () => selectFeature({ type: 'object', id: object.id }),
          }}
        >
          <Tooltip>{object.name}</Tooltip>
        </Marker>
      ))}
      {draftPositions.length > 1 && <Polyline positions={draftPositions} pathOptions={{ color: '#c6f36a', dashArray: '6 6' }} />}
      {draftPositions.length > 2 && (
        <Polygon
          positions={draftPositions}
          pathOptions={{ color: '#c6f36a', fillColor: '#c6f36a', fillOpacity: 0.12, dashArray: '6 6' }}
        />
      )}
      {objectDraft && (
        <Marker
          position={objectDraft.position}
          icon={objectIcons[objectDraft.symbolType] ?? objectIcons.marker}
          opacity={0.65}
        >
          <Tooltip>{objectDraft.name} (unsaved)</Tooltip>
        </Marker>
      )}
    </MapContainer>
  )
}
