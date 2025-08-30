using Xunit;
using FluentAssertions;
using Moq;
using backend.Application.Commands;
using backend.Domain.Entities;
using backend.Infrastructure.UnitOfWork;
using backend.Tests.Helpers;

namespace backend.Tests.Application.Commands
{
    /// <summary>
    /// Tests unitarios para DeleteDestinationCommandHandler
    /// Verifica la lógica de eliminación de destinos
    /// </summary>
    public class DeleteDestinationCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly DeleteDestinationCommandHandler _handler;

        public DeleteDestinationCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new DeleteDestinationCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldDeleteDestination()
        {
            // Arrange
            var command = new DeleteDestinationCommand { Id = 1 };
            var existingDestination = TestDataHelper.CreateTestDestination();

            _mockUnitOfWork.Setup(u => u.Destinations.GetByIdAsync(1))
                          .ReturnsAsync(existingDestination);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _mockUnitOfWork.Verify(u => u.Destinations.Remove(existingDestination), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentDestination_ShouldReturnFalse()
        {
            // Arrange
            var command = new DeleteDestinationCommand { Id = 999 };

            _mockUnitOfWork.Setup(u => u.Destinations.GetByIdAsync(999))
                          .ReturnsAsync((Destination?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            _mockUnitOfWork.Verify(u => u.Destinations.Remove(It.IsAny<Destination>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WithValidDestination_ShouldCallRepositoryMethods()
        {
            // Arrange
            var command = new DeleteDestinationCommand { Id = 1 };
            var existingDestination = TestDataHelper.CreateTestDestination();

            _mockUnitOfWork.Setup(u => u.Destinations.GetByIdAsync(1))
                          .ReturnsAsync(existingDestination);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockUnitOfWork.Verify(u => u.Destinations.GetByIdAsync(1), Times.Once);
            _mockUnitOfWork.Verify(u => u.Destinations.Remove(existingDestination), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
