using Xunit;
using FluentAssertions;
using Moq;
using AutoMapper;
using backend.Application.Queries;
using backend.Application.DTOs;
using backend.Domain.Entities;
using backend.Domain.Interfaces;
using backend.Domain.Interfaces;
using backend.Tests.Helpers;

namespace backend.Tests.Application.Queries
{
    /// <summary>
    /// Tests unitarios para GetDestinationsQueryHandler
    /// Verifica solo la lógica de negocio esencial
    /// </summary>
    public class GetDestinationsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetDestinationsQueryHandler _handler;

        public GetDestinationsQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetDestinationsQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_WithValidQuery_ShouldReturnPagedResults()
        {
            // Arrange
            var filter = TestDataHelper.CreateTestDestinationFilter();
            var query = new GetDestinationsQuery { Filter = filter };
            var destinations = TestDataHelper.CreateTestDestinations();
            var destinationDtos = destinations.Select(d => new DestinationDto 
            { 
                ID = d.ID, 
                Name = d.Name, 
                Type = d.Type 
            }).ToList();

            var pagedResult = new PagedResult<Destination>
            {
                Items = destinations,
                TotalCount = destinations.Count,
                Page = filter.Page,
                PageSize = filter.PageSize
            };

            _mockUnitOfWork.Setup(u => u.Destinations.GetDestinationsWithFiltersAsync(It.IsAny<IFilterCriteria>()))
                          .ReturnsAsync(pagedResult);
            _mockMapper.Setup(m => m.Map<List<DestinationDto>>(destinations))
                      .Returns(destinationDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(destinations.Count);
            result.TotalCount.Should().Be(destinations.Count);
            result.Page.Should().Be(filter.Page);
            result.PageSize.Should().Be(filter.PageSize);
        }

        [Fact]
        public async Task Handle_WithEmptyResults_ShouldReturnEmptyPagedResults()
        {
            // Arrange
            var filter = TestDataHelper.CreateTestDestinationFilter();
            var query = new GetDestinationsQuery { Filter = filter };
            var emptyPagedResult = new PagedResult<Destination>
            {
                Items = new List<Destination>(),
                TotalCount = 0,
                Page = filter.Page,
                PageSize = filter.PageSize
            };

            _mockUnitOfWork.Setup(u => u.Destinations.GetDestinationsWithFiltersAsync(It.IsAny<IFilterCriteria>()))
                          .ReturnsAsync(emptyPagedResult);
            _mockMapper.Setup(m => m.Map<List<DestinationDto>>(It.IsAny<List<Destination>>()))
                      .Returns(new List<DestinationDto>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }
    }
}
