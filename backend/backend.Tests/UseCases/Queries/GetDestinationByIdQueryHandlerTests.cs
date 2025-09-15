using Xunit;
using FluentAssertions;
using Moq;
using backend.Application.Queries;
using backend.Application.DTOs;
using backend.Domain.Entities;
using backend.Domain.Interfaces;
using backend.Tests.Helpers;

namespace backend.Tests.Application.Queries
{
    /// <summary>
    /// Tests unitarios para GetDestinationByIdQueryHandler
    /// Verifica la lógica de consulta de destino por ID
    /// </summary>
    public class GetDestinationByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly GetDestinationByIdQueryHandler _handler;

        public GetDestinationByIdQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new GetDestinationByIdQueryHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_WithValidId_ShouldReturnDestination()
        {
            // Arrange
            var query = new GetDestinationByIdQuery { Id = 1 };
            var destination = TestDataHelper.CreateTestDestination();
            var expectedDto = TestDataHelper.CreateTestDestinationDto();

            _mockUnitOfWork.Setup(u => u.Destinations.GetByIdAsync(1))
                          .ReturnsAsync(destination);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDto);
        }

        [Fact]
        public async Task Handle_WithNonExistentId_ShouldReturnNull()
        {
            // Arrange
            var query = new GetDestinationByIdQuery { Id = 999 };

            _mockUnitOfWork.Setup(u => u.Destinations.GetByIdAsync(999))
                          .ReturnsAsync((Destination?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            var query = new GetDestinationByIdQuery { Id = 1 };
            var destination = TestDataHelper.CreateTestDestination();
            var expectedDto = TestDataHelper.CreateTestDestinationDto();

            _mockUnitOfWork.Setup(u => u.Destinations.GetByIdAsync(1))
                          .ReturnsAsync(destination);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockUnitOfWork.Verify(u => u.Destinations.GetByIdAsync(1), Times.Once);
        }
    }
}
