using NetTopologySuite.Geometries;
using SharpKml.Engine;

namespace BadgermoleGeoAtlas
{
    /// <summary>
    /// Represents generic geographic data.
    /// </summary>
    public interface IGeographyHandler
    {
        /// <summary>
        /// Gets the bounding box of the geography.
        /// </summary>
        /// <returns>The bounding box as a <c>SharpKml.Engine.BoundingBox</c> object.</returns>
        public BoundingBox GetBoundingBox();

        /// <summary>
        /// Gets the geometry of the geography.
        /// </summary>
        /// <returns>The geometry.</returns>
        public IGeography GetGeometry();

        /// <summary>
        /// Gets the KML geometry of the geography.
        /// </summary>
        /// <param name="geometry">The <c>NetTopologySuite.Geometries</c> geometry to parse.</param>
        /// <returns>The KML geometry.</returns>
        public KmlFile ParseToKmlGeometry(IGeography geometry);
    }
}