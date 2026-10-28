import { useMapStore } from '../store/mapStore'
import type { CreateMapObject, CreateMapPolygon } from '../types'

interface FeatureToolsProps {
  objectCount: number
  polygonCount: number
  onCreateObject: (object: CreateMapObject) => void
  onCreatePolygon: (polygon: CreateMapPolygon) => void
  objectSaving: boolean
  polygonSaving: boolean
}

export function FeatureTools({
  objectCount,
  polygonCount,
  onCreateObject,
  onCreatePolygon,
  objectSaving,
  polygonSaving,
}: FeatureToolsProps) {
  const activeTool = useMapStore((state) => state.activeTool)
  const setTool = useMapStore((state) => state.setTool)
  const polygonDraft = useMapStore((state) => state.polygonDraft)
  const setPolygonName = useMapStore((state) => state.setPolygonName)
  const finishPolygon = useMapStore((state) => state.finishPolygon)
  const discardPolygon = useMapStore((state) => state.discardPolygon)
  const objectDraft = useMapStore((state) => state.objectDraft)
  const objectName = useMapStore((state) => state.objectName)
  const objectSymbolType = useMapStore((state) => state.objectSymbolType)
  const setObjectName = useMapStore((state) => state.setObjectName)
  const setObjectSymbolType = useMapStore((state) => state.setObjectSymbolType)
  const discardObject = useMapStore((state) => state.discardObject)

  function savePolygon() {
    if (!polygonDraft?.complete || polygonDraft.positions.length < 3) return
    const ring = polygonDraft.positions.map((point) => [point.lng, point.lat] as [number, number])
    ring.push(ring[0]!)
    onCreatePolygon({
      name: polygonDraft.name.trim(),
      geometry: { type: 'Polygon', coordinates: [ring] },
    })
  }

  function saveObject() {
    if (!objectDraft) return
    onCreateObject({
      name: objectDraft.name.trim(),
      symbolType: objectDraft.symbolType,
      geometry: {
        type: 'Point',
        coordinates: [objectDraft.position.lng, objectDraft.position.lat],
      },
    })
  }

  return (
    <section className="panel tools-panel" aria-labelledby="tools-title">
      <div className="panel-heading">
        <div className="panel-title">
          <span className="panel-number">TOOLS</span>
          <h2 id="tools-title">Create features</h2>
        </div>
      </div>
      <article className="feature-card">
        <div className="feature-heading">
          <span className="panel-number">02 / AREA</span>
          <span className="count-badge">{polygonCount}</span>
        </div>
        <h3>Polygons</h3>
        <p>Click the map to add boundary points.</p>
        <button
          type="button"
          className={activeTool === 'polygon' ? 'tool-button tool-button--active' : 'tool-button'}
          onClick={() => setTool(activeTool === 'polygon' ? 'none' : 'polygon')}
        >
          {activeTool === 'polygon' ? 'Stop drawing' : 'Add polygon'}
          <span aria-hidden="true">⌖</span>
        </button>
        {polygonDraft && (
          <div className="draft-controls">
            <label>
              Polygon name
              <input
                value={polygonDraft.name}
                onChange={(event) => setPolygonName(event.target.value)}
                maxLength={100}
              />
            </label>
            <p className="draft-hint">
              {polygonDraft.complete
                ? `${polygonDraft.positions.length} vertices ready to save`
                : `${polygonDraft.positions.length} points · ${polygonDraft.positions.length < 3 ? 'add at least 3' : 'finish when ready'}`}
            </p>
            {!polygonDraft.complete ? (
              <button
                type="button"
                className="secondary-button"
                disabled={polygonDraft.positions.length < 3}
                onClick={finishPolygon}
              >
                Finish polygon
              </button>
            ) : (
              <button
                type="button"
                className="primary-button"
                disabled={polygonSaving || !polygonDraft.name.trim()}
                onClick={savePolygon}
              >
                {polygonSaving ? 'Saving…' : 'Save polygon'}
              </button>
            )}
            <button type="button" className="text-button" onClick={discardPolygon}>
              Discard draft
            </button>
          </div>
        )}
      </article>
      <article className="feature-card">
        <div className="feature-heading">
          <span className="panel-number">03 / POINT</span>
          <span className="count-badge">{objectCount}</span>
        </div>
        <h3>Map objects</h3>
        <p>Choose a symbol, then place it on the map.</p>
        <label className="field-label">
          Object name
          <input value={objectName} onChange={(event) => setObjectName(event.target.value)} maxLength={100} />
        </label>
        <label className="field-label">
          Symbol
          <select value={objectSymbolType} onChange={(event) => setObjectSymbolType(event.target.value)}>
            <option value="marker">Marker</option>
            <option value="jeep">Jeep</option>
          </select>
        </label>
        <button
          type="button"
          className={activeTool === 'object' ? 'tool-button tool-button--active' : 'tool-button'}
          onClick={() => setTool(activeTool === 'object' ? 'none' : 'object')}
        >
          {activeTool === 'object' ? 'Cancel placement' : 'Place object'}
          <span aria-hidden="true">⌖</span>
        </button>
        {objectDraft && (
          <div className="draft-controls">
            <p className="draft-hint">
              Placed at {objectDraft.position.lat.toFixed(5)}, {objectDraft.position.lng.toFixed(5)}
            </p>
            <button
              type="button"
              className="primary-button"
              disabled={objectSaving || !objectDraft.name.trim()}
              onClick={saveObject}
            >
              {objectSaving ? 'Saving…' : 'Save object'}
            </button>
            <button type="button" className="text-button" onClick={discardObject}>
              Discard draft
            </button>
          </div>
        )}
      </article>
    </section>
  )
}
