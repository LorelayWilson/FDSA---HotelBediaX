using Xunit;
using FluentAssertions;
using Moq;
using backend.Application.Commands;
using backend.Domain.Entities;
using backend.Domain.Interfaces;
using backend.Tests.Helpers;

namespace backend.Tests.Application.Commands
{
    /// <summary>
    /// Tests unitarios para DeleteDestinationCommandHandler
    /// Verifica la lógica de eliminación de destinos
    /// </summary>
    public class DeleteDestinationCommandHandlerTests
    {
        private readonly Mock<IRepositoryManager> _mockRepositoryManager;
        private readonly DeleteDestinationCommandHandler _handler;

        public DeleteDestinationCommandHandlerTests()
        {
            _mockRepositoryManager = new Mock<IRepositoryManager>();
            _handler = new DeleteDestinationCommandHandler(_mockRepositoryManager.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldDeleteDestination()
        {
            // Arrange
            var command = new DeleteDestinationCommand { Id = 1 };
            var existingDestination = TestDataHelper.CreateTestDestination();

            _mockRepositoryManager.Setup(r => r.Destinations.GetByIdAsync(1))
                          .ReturnsAsync(existingDestination);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _mockRepositoryManager.Verify(r => r.Destinations.Remove(existingDestination), Times.Once);
            _mockRepositoryManager.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentDestination_ShouldReturnFalse()
        {
            // Arrange
            var command = new DeleteDestinationCommand { Id = 999 };

            _mockRepositoryManager.Setup(r => r.Destinations.GetByIdAsync(999))
                          .ReturnsAsync((Destination?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _mockRepositoryManager.Verify(r => r.Destinations.Remove(It.IsAny<Destination>()), Times.Never);
            _mockRepositoryManager.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WithValidDestination_ShouldCallRepositoryMethods()
        {
            // Arrange
            var command = new DeleteDestinationCommand { Id = 1 };
            var existingDestination = TestDataHelper.CreateTestDestination();

            _mockRepositoryManager.Setup(r => r.Destinations.GetByIdAsync(1))
                          .ReturnsAsync(existingDestination);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockRepositoryManager.Verify(r => r.Destinations.GetByIdAsync(1), Times.Once);
            _mockRepositoryManager.Verify(r => r.Destinations.Remove(existingDestination), Times.Once);
            _mockRepositoryManager.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
