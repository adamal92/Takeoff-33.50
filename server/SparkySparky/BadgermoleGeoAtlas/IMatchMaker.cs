namespace BadgermoleGeoAtlas
{
    /// <summary>
    /// Checks matches between geographies (Points, Polygons, Multipolygons, etc...) to see mutual overlapping
    /// </summary>
    public interface IMatchMaker
    {
        /// <summary>
        /// Checks geographical overlap between the given geographies
        /// </summary>
        /// <param name="geo1">The first geography.</param>
        /// <param name="geo2">The second geography.</param>
        /// <returns>true if the geographies match; otherwise, false.</returns>
        public bool IsMatch(IGeography geo1, IGeography geo2);
    }
}
