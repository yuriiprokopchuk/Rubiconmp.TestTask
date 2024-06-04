using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Services.DataContext;
using Services.Models;

namespace Services.Tests
{
    [TestClass]
    public class PolygonServiceTest : IDisposable
    {
        private readonly SpatialDbContext _spatialDbContext;
        private readonly PolygonService _polygonService;

        public PolygonServiceTest()
        {
            _spatialDbContext = new SpatialDbContext(new DbContextOptionsBuilder<SpatialDbContext>().UseInMemoryDatabase("PolygonServiceTest").Options);
            _polygonService = new PolygonService(_spatialDbContext);

            _spatialDbContext.Database.EnsureDeleted();
            _spatialDbContext.Database.EnsureCreated();

            SetupPolygons();
        }

        [TestMethod]
        public void Get_All_Polygons_ReturnsTwo()
        {
            var polygonDtos = _polygonService.GetPolygons();
            polygonDtos.Should().NotBeNullOrEmpty();
            polygonDtos.Should().HaveCount(2);
        }


        [TestMethod]
        public void Get_Polygons_Not_Intersected_By_Segment_ReturnsZero()
        {
            CoordinateDto[] coordinateDtos = [
                new CoordinateDto { X = 50.461321, Y = 30.419749 },
                new CoordinateDto { X = 50.433031, Y = 30.410565 }];

            var polygonDtos = _polygonService.GetPolygons(coordinateDtos);

            polygonDtos.Should().BeEmpty();
            polygonDtos.Should().HaveCount(0);
        }


        [TestMethod]
        public void Get_Polygons_Intersected_By_Segment_ReturnsOne()
        {
            CoordinateDto[] coordinateDtos = [
                new CoordinateDto { X = 50.461321, Y = 30.419749 },
                new CoordinateDto { X = 50.468118, Y = 30.461706 }];

            var polygonDtos = _polygonService.GetPolygons(coordinateDtos);

            polygonDtos.Should().NotBeNullOrEmpty();
            polygonDtos.Should().HaveCount(1);
        }

        [TestMethod]
        public void Get_Polygons_Intersected_By_Segment_ReturnsTwo()
        {
            CoordinateDto[] coordinateDtos = [
                new CoordinateDto { X = 50.453462, Y = 30.510630 },
                new CoordinateDto { X = 50.468118, Y = 30.461706 }];

            var polygonDtos = _polygonService.GetPolygons(coordinateDtos);

            polygonDtos.Should().NotBeNullOrEmpty();
            polygonDtos.Should().HaveCount(2);
        }

        private void SetupPolygons()
        {
            var createPolygonDto1 = new CreatePolygonDto
            {
                CoordinateDtos = [
            new CoordinateDto { X = 50.472392, Y = 30.448401 },
                    new CoordinateDto { X = 50.473302, Y = 30.503887 },
                    new CoordinateDto { X = 50.448970, Y = 30.481978 },
                    new CoordinateDto { X = 50.453671, Y = 30.446377 }]
            };

            var createPolygonDto2 = new CreatePolygonDto
            {
                CoordinateDtos = [
                    new CoordinateDto { X = 50.460716, Y = 30.483937 },
                    new CoordinateDto { X = 50.465201, Y = 30.513966 },
                    new CoordinateDto { X = 50.451843, Y = 30.515098 },
                    new CoordinateDto { X = 50.451032, Y = 30.474249 }]
            };

            _polygonService.CreatePolygon(createPolygonDto1);
            _polygonService.CreatePolygon(createPolygonDto2);
        }

        public void Dispose()
        {
            _spatialDbContext?.Dispose();
        }
    }
}