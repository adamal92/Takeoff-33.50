using NetTopologySuite.Geometries;
using SharpKml.Engine;
using System.Runtime.InteropServices;

namespace BadgermoleGeoAtlas
{
    /// <summary>
    /// Represents generic geographic data.
    /// </summary>
    public interface IGeographyParser
    {
        /// <summary>
        /// Gets the KML geometry of the geography.
        /// </summary>
        /// <param name="geometry">The <c>NetTopologySuite.Geometries</c> geometry to parse.</param>
        /// <returns>The KML geometry.</returns>
        public KmlFile ParseToKmlGeometry(Geometry geometry);

        /// <summary>
        /// Parses the KML geometry to a <c>NetTopologySuite.Geometries</c> object.
        /// </summary>
        /// <param name="kmlGeometry">The KML geometry to parse.</param>
        /// <returns>The geometry as a <c>NetTopologySuite.Geometries</c> object.</returns>
        public Geometry DecodeKmlToGeometry(KmlFile kmlGeometry);

        /// <summary>
        /// Reads a geometry from a KML file handle.
        /// </summary>
        /// <param name="kmlFile">The handle reference to the KML file.</param>
        /// <returns>The geometry read from the KML file as a <c>NetTopologySuite.Geometries</c> object.</returns>
        public Geometry ReadFromKmlGeometry(HandleRef kmlFile);
    }
}