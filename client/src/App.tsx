import { FeatureTools } from './components/FeatureTools'
import { MapCanvas } from './components/MapCanvas'
import { MapDataTable } from './components/MapDataTable'
import { useMapData } from './hooks/useMapData'
import { useMapStore } from './store/mapStore'
import type { CreateMapObject, CreateMapPolygon, FeatureSelection } from './types'

function App() {
  const {
    polygons,
    objects,
    isLoading,
    errors,
    reload,
    createPolygon,
    createObject,
    deleteFeature,
  } = useMapData()
  const selection = useMapStore((state) => state.selection)

  const errorsToShow = [
    ...errors,
    createPolygon.error,
    createObject.error,
    deleteFeature.error,
  ].filter((error): error is Error => error instanceof Error)

  function savePolygon(polygon: CreateMapPolygon) {
    createPolygon.mutate(polygon)
  }

  function saveObject(mapObject: CreateMapObject) {
    createObject.mutate(mapObject)
  }

  function deleteMapFeature(feature: FeatureSelection) {
    deleteFeature.mutate(feature)
  }

  return (
    <main className="workspace">
      <header className="topbar">
        <a className="brand" href="/" aria-label="Takeoff-33.50 home">
          <span className="brand-mark" aria-hidden="true">T.</span>
          <span>Takeoff-33.50</span>
        </a>
        <div className="workspace-label">
          <span className="status-dot" />
          GIS WORKSPACE
        </div>
      </header>

      <section className="intro" aria-labelledby="page-title">
        <div>
          <p className="eyebrow">MAP OPERATIONS / OVERVIEW</p>
          <h1 id="page-title">Your map, at a glance.</h1>
          <p className="intro-copy">
            Create map features and keep your spatial data in one place.
          </p>
        </div>
        <div className="coordinate-badge">
          <span className="coordinate-icon" aria-hidden="true">◎</span>
          <span>WGS 84</span>
          <span className="coordinate-divider" />
          <span>EPSG:4326</span>
        </div>
      </section>

      {errorsToShow.map((error, index) => (
        <div className="error-banner" role="alert" key={`${error.message}-${index}`}>
          <div>
            <strong>Map service error</strong>
            <span>{error.message}</span>
          </div>
          <button type="button" onClick={() => void reload()}>Retry loading data</button>
        </div>
      ))}

      <div className="dashboard">
        <section className="panel map-panel" aria-labelledby="map-title">
          <div className="panel-heading">
            <div className="panel-title">
              <span className="panel-number">01</span>
              <h2 id="map-title">Map</h2>
            </div>
            <span className="panel-tag">
              {isLoading ? 'LOADING MAP DATA' : 'INTERACTIVE CANVAS'}
            </span>
          </div>
          <div className="map-view" aria-label="Map panel">
            <MapCanvas objects={objects} polygons={polygons} />
            {isLoading && <div className="map-loading">Loading saved features…</div>}
          </div>
          <div className="map-footer">
            <span><i className="legend-dot" /> OpenStreetMap</span>
            <span>{objects.length + polygons.length} FEATURES</span>
          </div>
        </section>

        <FeatureTools
          objectCount={objects.length}
          polygonCount={polygons.length}
          onCreateObject={saveObject}
          onCreatePolygon={savePolygon}
          objectSaving={createObject.isPending}
          polygonSaving={createPolygon.isPending}
        />

        <MapDataTable
          objects={objects}
          polygons={polygons}
          selection={selection}
          onDelete={deleteMapFeature}
          deleting={deleteFeature.isPending}
        />
      </div>

      <footer className="page-footer">
        <span>BOOM-BOOM <span className="footer-separator">/</span> GIS CLIENT</span>
        <span>GEOJSON COORDINATES <span className="footer-separator">/</span> LONGITUDE, LATITUDE</span>
      </footer>
    </main>
  )
}

export default App
