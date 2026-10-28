import { useMapStore } from '../store/mapStore'
import type { FeatureSelection, MapObject, MapPolygon } from '../types'

interface MapDataTableProps {
  objects: MapObject[]
  polygons: MapPolygon[]
  selection: FeatureSelection | null
  onDelete: (feature: FeatureSelection) => void
  deleting: boolean
}

export function MapDataTable({
  objects,
  polygons,
  selection,
  onDelete,
  deleting,
}: MapDataTableProps) {
  const selectFeature = useMapStore((state) => state.selectFeature)
  const count = objects.length + polygons.length

  return (
    <section className="panel data-panel" aria-labelledby="data-title">
      <div className="panel-heading">
        <div className="panel-title">
          <span className="panel-number">04</span>
          <h2 id="data-title">Map data</h2>
        </div>
        <span className="count-badge">{count}</span>
      </div>
      <div className="table-wrap">
        <table>
          <thead>
            <tr>
              <th scope="col">OBJECT</th>
              <th scope="col">LAT</th>
              <th scope="col">LON</th>
              <th scope="col">TYPE</th>
              <th scope="col"><span className="visually-hidden">Actions</span></th>
            </tr>
          </thead>
          <tbody>
            {objects.map((object) => {
              const rowSelection = { type: 'object' as const, id: object.id }
              return (
                <tr
                  key={`object-${object.id}`}
                  className={selection?.type === 'object' && selection.id === object.id ? 'data-row data-row--selected' : 'data-row'}
                  onClick={() => selectFeature(rowSelection)}
                >
                  <td>{object.name}</td>
                  <td>{object.geometry.coordinates[1].toFixed(5)}</td>
                  <td>{object.geometry.coordinates[0].toFixed(5)}</td>
                  <td><span className="type-tag">{object.symbolType}</span></td>
                  <td>
                    <button
                      type="button"
                      className="delete-button"
                      aria-label={`Delete ${object.name}`}
                      disabled={deleting}
                      onClick={(event) => {
                        event.stopPropagation()
                        onDelete(rowSelection)
                      }}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              )
            })}
            {polygons.map((polygon) => {
              const rowSelection = { type: 'polygon' as const, id: polygon.id }
              const firstPosition = polygon.geometry.coordinates[0]?.[0]
              return (
                <tr
                  key={`polygon-${polygon.id}`}
                  className={selection?.type === 'polygon' && selection.id === polygon.id ? 'data-row data-row--selected' : 'data-row'}
                  onClick={() => selectFeature(rowSelection)}
                >
                  <td>{polygon.name}</td>
                  <td>{firstPosition ? firstPosition[1].toFixed(5) : '—'}</td>
                  <td>{firstPosition ? firstPosition[0].toFixed(5) : '—'}</td>
                  <td><span className="type-tag">polygon</span></td>
                  <td>
                    <button
                      type="button"
                      className="delete-button"
                      aria-label={`Delete ${polygon.name}`}
                      disabled={deleting}
                      onClick={(event) => {
                        event.stopPropagation()
                        onDelete(rowSelection)
                      }}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              )
            })}
            {count === 0 && (
              <tr>
                <td colSpan={5}>
                  <div className="empty-data">
                    <span aria-hidden="true">—</span>
                    <p>No map data yet</p>
                    <small>Saved objects and polygons will appear here.</small>
                  </div>
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </section>
  )
}
