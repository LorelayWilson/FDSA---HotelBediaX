using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using backend.Data;
using backend.Models;
using backend.Services;

namespace backend.Tests.Services
{
    /// <summary>
    /// Tests unitarios para DataSeedService
    /// Prueba la funcionalidad de poblar la base de datos con datos de ejemplo
    /// </summary>
    public class DataSeedServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly DataSeedService _service;

        public DataSeedServiceTests()
        {
            // Configurar base de datos en memoria para tests
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new DataSeedService(_context);
        }

        [Fact]
        public async Task SeedDataAsync_WithEmptyDatabase_AddsDestinations()
        {
            // Arrange - Base de datos vacía

            // Act
            await _service.SeedDataAsync();

            // Assert
            var destinations = await _context.Destinations.ToListAsync();
            destinations.Should().HaveCount(10);
            
            // Verificar que se insertaron destinos de diferentes tipos
            destinations.Should().Contain(d => d.Type == DestinationType.Beach);
            destinations.Should().Contain(d => d.Type == DestinationType.Cultural);
            destinations.Should().Contain(d => d.Type == DestinationType.Adventure);
            destinations.Should().Contain(d => d.Type == DestinationType.City);
            destinations.Should().Contain(d => d.Type == DestinationType.Mountain);
            destinations.Should().Contain(d => d.Type == DestinationType.Relax);
        }

        [Fact]
        public async Task SeedDataAsync_WithExistingDestinations_DoesNotAddMore()
        {
            // Arrange - Agregar un destino existente
            var existingDestination = new Destination
            {
                Name = "Destino Existente",
                Description = "Descripción del destino existente",
                CountryCode = "MEX",
                Type = DestinationType.Beach,
                LastModif = DateTime.UtcNow
            };
            
            _context.Destinations.Add(existingDestination);
            await _context.SaveChangesAsync();

            // Act
            await _service.SeedDataAsync();

            // Assert
            var destinations = await _context.Destinations.ToListAsync();
            destinations.Should().HaveCount(1);
            destinations.First().Name.Should().Be("Destino Existente");
        }

        [Fact]
        public async Task SeedDataAsync_AddsCorrectDestinations()
        {
            // Act
            await _service.SeedDataAsync();

            // Assert - Verificar destinos específicos
            var destinations = await _context.Destinations.ToListAsync();
            
            // Verificar Playa del Carmen
            var playaDelCarmen = destinations.FirstOrDefault(d => d.Name == "Playa del Carmen");
            playaDelCarmen.Should().NotBeNull();
            playaDelCarmen!.CountryCode.Should().Be("MEX");
            playaDelCarmen.Type.Should().Be(DestinationType.Beach);
            playaDelCarmen.Description.Should().Contain("caribeña");

            // Verificar Santorini
            var santorini = destinations.FirstOrDefault(d => d.Name == "Santorini");
            santorini.Should().NotBeNull();
            santorini!.CountryCode.Should().Be("GRC");
            santorini.Type.Should().Be(DestinationType.Cultural);
            santorini.Description.Should().Contain("griega");

            // Verificar Kyoto
            var kyoto = destinations.FirstOrDefault(d => d.Name == "Kyoto");
            kyoto.Should().NotBeNull();
            kyoto!.CountryCode.Should().Be("JPN");
            kyoto.Type.Should().Be(DestinationType.Cultural);
            kyoto.Description.Should().Contain("Japón");

            // Verificar Machu Picchu
            var machuPicchu = destinations.FirstOrDefault(d => d.Name == "Machu Picchu");
            machuPicchu.Should().NotBeNull();
            machuPicchu!.CountryCode.Should().Be("PER");
            machuPicchu.Type.Should().Be(DestinationType.Adventure);
            machuPicchu.Description.Should().Contain("inca");

            // Verificar París
            var paris = destinations.FirstOrDefault(d => d.Name == "París");
            paris.Should().NotBeNull();
            paris!.CountryCode.Should().Be("FRA");
            paris.Type.Should().Be(DestinationType.City);
            paris.Description.Should().Contain("amor");

            // Verificar Nueva York
            var newYork = destinations.FirstOrDefault(d => d.Name == "Nueva York");
            newYork.Should().NotBeNull();
            newYork!.CountryCode.Should().Be("USA");
            newYork.Type.Should().Be(DestinationType.City);
            newYork.Description.Should().Contain("nunca duerme");

            // Verificar Barcelona
            var barcelona = destinations.FirstOrDefault(d => d.Name == "Barcelona");
            barcelona.Should().NotBeNull();
            barcelona!.CountryCode.Should().Be("ESP");
            barcelona.Type.Should().Be(DestinationType.Cultural);
            barcelona.Description.Should().Contain("Gaudí");

            // Verificar Río de Janeiro
            var rio = destinations.FirstOrDefault(d => d.Name == "Río de Janeiro");
            rio.Should().NotBeNull();
            rio!.CountryCode.Should().Be("BRA");
            rio.Type.Should().Be(DestinationType.City);
            rio.Description.Should().Contain("Cristo Redentor");

            // Verificar Alpes Suizos
            var alpes = destinations.FirstOrDefault(d => d.Name == "Alpes Suizos");
            alpes.Should().NotBeNull();
            alpes!.CountryCode.Should().Be("CHE");
            alpes.Type.Should().Be(DestinationType.Mountain);
            alpes.Description.Should().Contain("esquí");

            // Verificar Bali
            var bali = destinations.FirstOrDefault(d => d.Name == "Bali");
            bali.Should().NotBeNull();
            bali!.CountryCode.Should().Be("IDN");
            bali.Type.Should().Be(DestinationType.Relax);
            bali.Description.Should().Contain("indonesia");
        }

        [Fact]
        public async Task SeedDataAsync_SetsCorrectLastModifDates()
        {
            // Arrange
            var beforeSeed = DateTime.UtcNow;

            // Act
            await _service.SeedDataAsync();

            // Assert
            var afterSeed = DateTime.UtcNow;
            var destinations = await _context.Destinations.ToListAsync();
            
            foreach (var destination in destinations)
            {
                destination.LastModif.Should().BeOnOrAfter(beforeSeed);
                destination.LastModif.Should().BeOnOrBefore(afterSeed);
            }
        }

        [Fact]
        public async Task SeedDataAsync_AssignsUniqueIds()
        {
            // Act
            await _service.SeedDataAsync();

            // Assert
            var destinations = await _context.Destinations.ToListAsync();
            var ids = destinations.Select(d => d.ID).ToList();
            
            ids.Should().OnlyHaveUniqueItems();
            ids.Should().AllSatisfy(id => id.Should().BeGreaterThan(0));
        }

        [Fact]
        public async Task SeedDataAsync_CanBeCalledMultipleTimesSafely()
        {
            // Act - Llamar múltiples veces
            await _service.SeedDataAsync();
            await _service.SeedDataAsync();
            await _service.SeedDataAsync();

            // Assert - Solo debe haber 10 destinos (no duplicados)
            var destinations = await _context.Destinations.ToListAsync();
            destinations.Should().HaveCount(10);
        }

        [Fact]
        public async Task SeedDataAsync_CoversAllDestinationTypes()
        {
            // Act
            await _service.SeedDataAsync();

            // Assert
            var destinations = await _context.Destinations.ToListAsync();
            var types = destinations.Select(d => d.Type).Distinct().ToList();
            
            // Verificar que se cubren todos los tipos de destino
            types.Should().Contain(DestinationType.Beach);
            types.Should().Contain(DestinationType.Mountain);
            types.Should().Contain(DestinationType.City);
            types.Should().Contain(DestinationType.Cultural);
            types.Should().Contain(DestinationType.Adventure);
            types.Should().Contain(DestinationType.Relax);
            
            // Verificar que hay al menos un destino de cada tipo
            destinations.Count(d => d.Type == DestinationType.Beach).Should().BeGreaterOrEqualTo(1);
            destinations.Count(d => d.Type == DestinationType.Cultural).Should().BeGreaterOrEqualTo(1);
            destinations.Count(d => d.Type == DestinationType.City).Should().BeGreaterOrEqualTo(1);
            destinations.Count(d => d.Type == DestinationType.Adventure).Should().BeGreaterOrEqualTo(1);
            destinations.Count(d => d.Type == DestinationType.Mountain).Should().BeGreaterOrEqualTo(1);
            destinations.Count(d => d.Type == DestinationType.Relax).Should().BeGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task SeedDataAsync_UsesValidCountryCodes()
        {
            // Act
            await _service.SeedDataAsync();

            // Assert
            var destinations = await _context.Destinations.ToListAsync();
            
            foreach (var destination in destinations)
            {
                destination.CountryCode.Should().HaveLength(3);
                destination.CountryCode.Should().MatchRegex("^[A-Z]{3}$");
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
