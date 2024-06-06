using Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;

namespace Polygon.FunctionalTests
{
    public class PolygonIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private const string Path = "https://localhost:7043/api/polygons";

        public PolygonIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Create_And_Get_Polygons_Intersected_By_Segment()
        {
            CreatePolygon createPolygon = new()
            {
                Coordinates = [
                    new Coordinate { X = 50.460716, Y = 30.483937 },
                    new Coordinate { X = 50.465201, Y = 30.513966 },
                    new Coordinate { X = 50.451843, Y = 30.515098 },
                    new Coordinate { X = 50.451032, Y = 30.474249 }]
            };

            var createPolygonContent = new StringContent(JsonConvert.SerializeObject(createPolygon), Encoding.UTF8, "application/json");

            var httpResponseMessage = await _client.PostAsync(Path, createPolygonContent);

            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new Exception("Polygon creation doesn't work");

            var createPolygonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

            int id = JsonConvert.DeserializeObject<int>(createPolygonResponse);

            httpResponseMessage = await _client.GetAsync($"{Path}/segment/50.453462/30.510630/50.468118/30.461706");

            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new Exception("Getting polygons by segment doesn't work");

            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var polygons = JsonConvert.DeserializeObject<Api.Models.Polygon[]>(response);

            polygons.Should().NotBeNullOrEmpty();

            polygons.Should().HaveCountGreaterThanOrEqualTo(1);

            httpResponseMessage = await _client.DeleteAsync($"{Path}/{id}");

            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new Exception("Deleting a polygon doesn't work");
        }
    }
}
