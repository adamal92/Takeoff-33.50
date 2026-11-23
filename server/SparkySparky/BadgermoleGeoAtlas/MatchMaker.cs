namespace BadgermoleGeoAtlas
{
    public class MatchMaker : IMatchMaker
    {
        public bool IsMatch(IGeography geo1, IGeography geo2)
        {
            // Implement the logic to check if the two geographies match
            // This is a placeholder implementation and should be replaced with actual logic
            return geo1.FormatName == geo2.FormatName;
            throw new NotImplementedException("IsMatch method is not implemented yet.");
        }
    }
}
