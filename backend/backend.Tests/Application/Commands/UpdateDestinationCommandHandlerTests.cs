using Xunit;
using FluentAssertions;
using Moq;
using AutoMapper;
using backend.Application.Commands;
using backend.Application.DTOs;
using backend.Domain.Entities;
using backend.Domain.Interfaces;
using backend.Tests.Helpers;

namespace backend.Tests.Application.Commands
{
    /// <summary>
    /// Tests unitarios para UpdateDestinationCommandHandler
    /// Verifica la lógica de actualización de destinos
    /// </summary>
    public class UpdateDestinationCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateDestinationCommandHandler _handler;

        public UpdateDestinationCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateDestinationCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldUpdateDestination()
        {
            // Arrange
            var updateDto = TestDataHelper.CreateTestUpdateDestinationDto();
            var command = new UpdateDestinationCommand { Id = 1, UpdateDestinationDto = updateDto };
            var existingDestination = TestDataHelper.CreateTestDestination();
            var expectedDto = TestDataHelper.CreateTestDestinationDto();

            _mockUnitOfWork.Setup(u => u.Destinations.GetByIdAsync(1))
                          .ReturnsAsync(existingDestination);
            _mockMapper.Setup(m => m.Map<DestinationDto>(existingDestination))
                      .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDto);
            _mockUnitOfWork.Verify(u => u.Destinations.Update(existingDestination), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentDestination_ShouldReturnNull()
        {
            // Arrange
            var updateDto = TestDataHelper.CreateTestUpdateDestinationDto();
            var command = new UpdateDestinationCommand { Id = 999, UpdateDestinationDto = updateDto };

            _mockUnitOfWork.Setup(u => u.Destinations.GetByIdAsync(999))
                          .ReturnsAsync((Destination?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _mockUnitOfWork.Verify(u => u.Destinations.Update(It.IsAny<Destination>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldSetLastModifToCurrentTime()
        {
            // Arrange
            var updateDto = TestDataHelper.CreateTestUpdateDestinationDto();
            var command = new UpdateDestinationCommand { Id = 1, UpdateDestinationDto = updateDto };
            var existingDestination = TestDataHelper.CreateTestDestination();
            var expectedDto = TestDataHelper.CreateTestDestinationDto();

            _mockUnitOfWork.Setup(u => u.Destinations.GetByIdAsync(1))
                          .ReturnsAsync(existingDestination);
            _mockMapper.Setup(m => m.Map<DestinationDto>(It.IsAny<Destination>()))
                      .Returns(expectedDto);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingDestination.LastModif.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
