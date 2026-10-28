const panels = [
  {
    number: '02',
    title: 'Polygons',
    description: 'Draw and manage areas on the map.',
    action: 'Add polygon',
  },
  {
    number: '03',
    title: 'Map objects',
    description: 'Place markers and custom symbols.',
    action: 'Add object',
  },
]

function App() {
  return (
    <main className="workspace">
      <header className="topbar">
        <a className="brand" href="/" aria-label="Boom-Boom home">
          <span className="brand-mark" aria-hidden="true">
            b.
          </span>
          <span>boom-boom</span>
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
          <span className="coordinate-icon" aria-hidden="true">
            ◎
          </span>
          <span>WGS 84</span>
          <span className="coordinate-divider" />
          <span>EPSG:4326</span>
        </div>
      </section>

      <div className="dashboard">
        <section className="panel map-panel" aria-labelledby="map-title">
          <div className="panel-heading">
            <div className="panel-title">
              <span className="panel-number">01</span>
              <h2 id="map-title">Map</h2>
            </div>
            <span className="panel-tag">INTERACTIVE CANVAS</span>
          </div>
          <div className="map-placeholder">
            <div className="map-grid" aria-hidden="true" />
            <div className="map-crosshair" aria-hidden="true">
              <span />
            </div>
            <div className="map-message">
              <span className="map-pin" aria-hidden="true">
                +
              </span>
              <p>Map canvas</p>
              <span>Connect a map layer to get started</span>
            </div>
            <span className="map-coordinate map-coordinate-top">
              32°05&apos;N&nbsp; 34°46&apos;E
            </span>
            <span className="map-coordinate map-coordinate-bottom">
              ZOOM&nbsp; 01
            </span>
          </div>
          <div className="map-footer">
            <span><i className="legend-dot" /> No active layers</span>
            <span>0 FEATURES</span>
          </div>
        </section>

        <section className="panel tools-panel" aria-labelledby="tools-title">
          <div className="panel-heading">
            <div className="panel-title">
              <span className="panel-number">TOOLS</span>
              <h2 id="tools-title">Create features</h2>
            </div>
          </div>
          <div className="feature-list">
            {panels.map((panel) => (
              <article className="feature-card" key={panel.number}>
                <div className="feature-heading">
                  <span className="panel-number">{panel.number}</span>
                  <span className="feature-indicator" aria-hidden="true">
                    +
                  </span>
                </div>
                <h3>{panel.title}</h3>
                <p>{panel.description}</p>
                <button type="button" disabled>
                  {panel.action}
                  <span aria-hidden="true">↗</span>
                </button>
              </article>
            ))}
          </div>
          <p className="tools-note">Map tools will be available when the map is connected.</p>
        </section>

        <section className="panel data-panel" aria-labelledby="data-title">
          <div className="panel-heading">
            <div className="panel-title">
              <span className="panel-number">04</span>
              <h2 id="data-title">Map data</h2>
            </div>
            <span className="count-badge">0</span>
          </div>
          <div className="table-wrap">
            <table>
              <thead>
                <tr>
                  <th scope="col">OBJECT</th>
                  <th scope="col">LAT</th>
                  <th scope="col">LON</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td colSpan={3}>
                    <div className="empty-data">
                      <span aria-hidden="true">—</span>
                      <p>No map data yet</p>
                      <small>Saved objects will appear here.</small>
                    </div>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </section>
      </div>

      <footer className="page-footer">
        <span>BOOM-BOOM <span className="footer-separator">/</span> GIS CLIENT</span>
        <span>MAP DATA STAYS IN SYNC WITH YOUR WORKSPACE</span>
      </footer>
    </main>
  )
}

export default App
