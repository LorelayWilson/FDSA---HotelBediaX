using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using backend.Infrastructure.Data;
using backend.Infrastructure.UnitOfWork;
using backend.Domain.Entities;
using backend.Domain.Interfaces;
using backend.Tests.Helpers;

namespace backend.Tests.Ports.Services
{
    /// <summary>
    /// Tests de contrato para IUnitOfWork
    /// Verifica que cualquier implementación del puerto cumpla con el contrato
    /// </summary>
    public abstract class IUnitOfWorkContractTests
    {
        protected abstract IUnitOfWork CreateUnitOfWork(ApplicationDbContext context);

        [Fact]
        public async Task SaveChangesAsync_ShouldReturnNumberOfAffectedRecords()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);
            using var unitOfWork = CreateUnitOfWork(context);

            var destination = TestDataHelper.CreateTestDestination();

            // Act
            unitOfWork.Destinations.Add(destination);
            var result = await unitOfWork.SaveChangesAsync();

            // Assert
            result.Should().Be(1);
            context.Destinations.Should().HaveCount(1);
        }

        [Fact]
        public async Task BeginTransactionAsync_ShouldReturnTransaction()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);
            using var unitOfWork = CreateUnitOfWork(context);

            // Act
            var transaction = await unitOfWork.BeginTransactionAsync();

            // Assert
            transaction.Should().NotBeNull();
            transaction.Should().BeAssignableTo<IDbContextTransaction>();
        }

        [Fact]
        public async Task CommitTransactionAsync_ShouldCommitChanges()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);
            using var unitOfWork = CreateUnitOfWork(context);

            var destination = TestDataHelper.CreateTestDestination();

            // Act
            var transaction = await unitOfWork.BeginTransactionAsync();
            unitOfWork.Destinations.Add(destination);
            await unitOfWork.CommitTransactionAsync();

            // Assert
            context.Destinations.Should().HaveCount(1);
        }

        [Fact]
        public async Task RollbackTransactionAsync_ShouldRollbackChanges()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);
            using var unitOfWork = CreateUnitOfWork(context);

            var destination = TestDataHelper.CreateTestDestination();

            // Act
            var transaction = await unitOfWork.BeginTransactionAsync();
            unitOfWork.Destinations.Add(destination);
            await unitOfWork.RollbackTransactionAsync();

            // Assert
            context.Destinations.Should().BeEmpty();
        }

        [Fact]
        public void Destinations_ShouldReturnRepository()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);
            using var unitOfWork = CreateUnitOfWork(context);

            // Act
            var repository = unitOfWork.Destinations;

            // Assert
            repository.Should().NotBeNull();
            repository.Should().BeAssignableTo<IDestinationRepository>();
        }
    }

    /// <summary>
    /// Tests de contrato para IUnitOfWork usando implementación concreta
    /// </summary>
    public class UnitOfWorkContractTests : IUnitOfWorkContractTests
    {
        protected override IUnitOfWork CreateUnitOfWork(ApplicationDbContext context)
        {
            return new UnitOfWork(context);
        }
    }
}
