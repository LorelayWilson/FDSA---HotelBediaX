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
    /// Tests unitarios para CreateDestinationCommandHandler
    /// Verifica la lógica de creación de destinos
    /// </summary>
    public class CreateDestinationCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateDestinationCommandHandler _handler;

        public CreateDestinationCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreateDestinationCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreateDestination()
        {
            // Arrange
            var createDto = TestDataHelper.CreateTestCreateDestinationDto();
            var command = new CreateDestinationCommand { CreateDestinationDto = createDto };
            var destination = TestDataHelper.CreateTestDestination();
            var expectedDto = TestDataHelper.CreateTestDestinationDto();

            _mockMapper.Setup(m => m.Map<Destination>(createDto))
                      .Returns(destination);
            _mockMapper.Setup(m => m.Map<DestinationDto>(destination))
                      .Returns(expectedDto);
            
            // Setup mock para el repositorio
            var mockRepository = new Mock<backend.Domain.Interfaces.IDestinationRepository>();
            _mockUnitOfWork.Setup(u => u.Destinations).Returns(mockRepository.Object);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedDto);
            mockRepository.Verify(r => r.Add(destination), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldSetLastModifToCurrentTime()
        {
            // Arrange
            var createDto = TestDataHelper.CreateTestCreateDestinationDto();
            var command = new CreateDestinationCommand { CreateDestinationDto = createDto };
            var destination = new Destination();
            var expectedDto = TestDataHelper.CreateTestDestinationDto();

            _mockMapper.Setup(m => m.Map<Destination>(createDto))
                      .Returns(destination);
            _mockMapper.Setup(m => m.Map<DestinationDto>(It.IsAny<Destination>()))
                      .Returns(expectedDto);
            
            // Setup mock para el repositorio
            var mockRepository = new Mock<backend.Domain.Interfaces.IDestinationRepository>();
            _mockUnitOfWork.Setup(u => u.Destinations).Returns(mockRepository.Object);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            destination.LastModif.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
