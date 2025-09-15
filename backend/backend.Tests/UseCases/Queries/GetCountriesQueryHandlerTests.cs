using Xunit;
using FluentAssertions;
using Moq;
using backend.Application.Queries;
using backend.Domain.Interfaces;

namespace backend.Tests.Application.Queries
{
    /// <summary>
    /// Tests unitarios para GetCountriesQueryHandler
    /// Verifica la lógica de consulta de países
    /// </summary>
    public class GetCountriesQueryHandlerTests
    {
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly GetCountriesQueryHandler _handler;

        public GetCountriesQueryHandlerTests()
        {
            _mockRepositoryManager = new Mock<IRepositoryManager>();
            _handler = new GetCountriesQueryHandler(_mockRepositoryManager.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfCountries()
        {
            // Arrange
            var query = new GetCountriesQuery();
            var expectedCountries = new List<string> { "MEX", "FRA", "JPN", "ESP", "USA" };

            _mockRepositoryManager.Setup(r => r.Destinations.GetUniqueCountryCodesAsync())
                          .ReturnsAsync(expectedCountries);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedCountries);
            result.Should().HaveCount(5);
        }

        [Fact]
        public async Task Handle_WithEmptyDatabase_ShouldReturnEmptyList()
        {
            // Arrange
            var query = new GetCountriesQuery();
            var emptyCountries = new List<string>();

            _mockRepositoryManager.Setup(r => r.Destinations.GetUniqueCountryCodesAsync())
                          .ReturnsAsync(emptyCountries);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryMethod()
        {
            // Arrange
            var query = new GetCountriesQuery();
            var countries = new List<string> { "MEX", "FRA" };

            _mockRepositoryManager.Setup(r => r.Destinations.GetUniqueCountryCodesAsync())
                          .ReturnsAsync(countries);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepositoryManager.Verify(r => r.Destinations.GetUniqueCountryCodesAsync(), Times.Once);
        }
    }
}
