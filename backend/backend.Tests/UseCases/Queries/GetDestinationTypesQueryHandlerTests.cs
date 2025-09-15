using Xunit;
using FluentAssertions;
using Moq;
using backend.Application.Queries;
using backend.Domain.Interfaces;

namespace backend.Tests.Application.Queries
{
    /// <summary>
    /// Tests unitarios para GetDestinationTypesQueryHandler
    /// Verifica la lógica de consulta de tipos de destino
    /// </summary>
    public class GetDestinationTypesQueryHandlerTests
    {
        private readonly GetDestinationTypesQueryHandler _handler;

        public GetDestinationTypesQueryHandlerTests()
        {
            _handler = new GetDestinationTypesQueryHandler();
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfDestinationTypes()
        {
            // Arrange
            var query = new GetDestinationTypesQuery();
            var expectedTypes = new List<string> 
            { 
                "Beach", "Mountain", "City", "Cultural", "Adventure", "Relax" 
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedTypes);
            result.Should().HaveCount(6);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllExpectedTypes()
        {
            // Arrange
            var query = new GetDestinationTypesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().Contain("Beach");
            result.Should().Contain("Mountain");
            result.Should().Contain("City");
            result.Should().Contain("Cultural");
            result.Should().Contain("Adventure");
            result.Should().Contain("Relax");
        }

        [Fact]
        public async Task Handle_ShouldReturnConsistentResults()
        {
            // Arrange
            var query = new GetDestinationTypesQuery();

            // Act
            var result1 = await _handler.Handle(query, CancellationToken.None);
            var result2 = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result1.Should().BeEquivalentTo(result2);
            result1.Should().HaveCount(6);
        }
    }
}
