using BadgermoleGeoAtlas;
using NetTopologySuite.Geometries;

namespace BadgermoleGeoAtlas.Tests
{
    public class MatchMakerTests
    {
        private readonly IMatchMaker _matchMaker;

        public MatchMakerTests()
        {
            _matchMaker = new MatchMaker();
        }

        [Fact]
        public void IsMatch_ReturnsTrue_WhenGeographiesMatch()
        {
            // Arrange
            IGeography geo1 = new XyzGeometry(36.1, -120.2);
            IGeography geo2 = new XyzGeometry(36.1, -120.2);

            // Act
            var result = _matchMaker.IsMatch(geo1, geo2);

            // Assert
            Assert.True(result);
        }
    }
}