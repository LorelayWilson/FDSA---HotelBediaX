using AutoMapper;
using backend.DTOs;
using backend.Mapping;
using backend.Models;
using backend.Tests.Helpers;

namespace backend.Tests.Mapping
{
    /// <summary>
    /// Tests unitarios para AutoMapperProfile
    /// Verifica que todos los mapeos entre entidades y DTOs funcionen correctamente
    /// </summary>
    public class AutoMapperProfileTests
    {
        private readonly IMapper _mapper;

        public AutoMapperProfileTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void AutoMapperProfile_ShouldBeValid()
        {
            // Arrange & Act
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());

            // Assert
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_DestinationToDestinationDto_ShouldMapCorrectly()
        {
            // Arrange
            var destination = TestDataHelper.CreateTestDestination();

            // Act
            var destinationDto = _mapper.Map<DestinationDto>(destination);

            // Assert
            destinationDto.Should().NotBeNull();
            destinationDto.ID.Should().Be(destination.ID);
            destinationDto.Name.Should().Be(destination.Name);
            destinationDto.Description.Should().Be(destination.Description);
            destinationDto.CountryCode.Should().Be(destination.CountryCode);
            destinationDto.Type.Should().Be(destination.Type);
            destinationDto.LastModif.Should().Be(destination.LastModif);
        }

        [Fact]
        public void Map_CreateDestinationDtoToDestination_ShouldMapCorrectly()
        {
            // Arrange
            var createDto = TestDataHelper.CreateTestCreateDestinationDto();

            // Act
            var destination = _mapper.Map<Destination>(createDto);

            // Assert
            destination.Should().NotBeNull();
            destination.ID.Should().Be(0); // ID no se mapea desde CreateDto
            destination.Name.Should().Be(createDto.Name);
            destination.Description.Should().Be(createDto.Description);
            destination.CountryCode.Should().Be(createDto.CountryCode);
            destination.Type.Should().Be(createDto.Type);
            destination.LastModif.Should().Be(default(DateTime)); // LastModif no se mapea desde CreateDto
        }

        [Fact]
        public void Map_UpdateDestinationDtoToDestination_ShouldMapCorrectly()
        {
            // Arrange
            var updateDto = TestDataHelper.CreateTestUpdateDestinationDto();

            // Act
            var destination = _mapper.Map<Destination>(updateDto);

            // Assert
            destination.Should().NotBeNull();
            destination.ID.Should().Be(0); // ID no se mapea desde UpdateDto
            destination.Name.Should().Be(updateDto.Name);
            destination.Description.Should().Be(updateDto.Description);
            destination.CountryCode.Should().Be(updateDto.CountryCode);
            destination.Type.Should().Be(updateDto.Type);
            destination.LastModif.Should().Be(default(DateTime)); // LastModif no se mapea desde UpdateDto
        }

        [Fact]
        public void Map_UpdateDestinationDtoToExistingDestination_ShouldUpdateCorrectly()
        {
            // Arrange
            var existingDestination = TestDataHelper.CreateTestDestination();
            var updateDto = TestDataHelper.CreateTestUpdateDestinationDto();
            var originalId = existingDestination.ID;
            var originalLastModif = existingDestination.LastModif;

            // Act
            _mapper.Map(updateDto, existingDestination);

            // Assert
            existingDestination.ID.Should().Be(originalId); // ID no debe cambiar
            existingDestination.Name.Should().Be(updateDto.Name);
            existingDestination.Description.Should().Be(updateDto.Description);
            existingDestination.CountryCode.Should().Be(updateDto.CountryCode);
            existingDestination.Type.Should().Be(updateDto.Type);
            existingDestination.LastModif.Should().Be(originalLastModif); // LastModif no debe cambiar automáticamente
        }

        [Fact]
        public void Map_ListOfDestinationsToDestinationDtos_ShouldMapCorrectly()
        {
            // Arrange
            var destinations = TestDataHelper.CreateTestDestinations();

            // Act
            var destinationDtos = _mapper.Map<List<DestinationDto>>(destinations);

            // Assert
            destinationDtos.Should().NotBeNull();
            destinationDtos.Should().HaveCount(3);
            
            for (int i = 0; i < destinations.Count; i++)
            {
                destinationDtos[i].ID.Should().Be(destinations[i].ID);
                destinationDtos[i].Name.Should().Be(destinations[i].Name);
                destinationDtos[i].Description.Should().Be(destinations[i].Description);
                destinationDtos[i].CountryCode.Should().Be(destinations[i].CountryCode);
                destinationDtos[i].Type.Should().Be(destinations[i].Type);
                destinationDtos[i].LastModif.Should().Be(destinations[i].LastModif);
            }
        }

        [Fact]
        public void Map_DestinationWithAllTypes_ShouldMapCorrectly()
        {
            // Arrange
            var destinationTypes = Enum.GetValues<DestinationType>();
            
            foreach (var type in destinationTypes)
            {
                var destination = new Destination
                {
                    ID = 1,
                    Name = $"Test {type}",
                    Description = $"Description for {type}",
                    CountryCode = "TST",
                    Type = type,
                    LastModif = DateTime.UtcNow
                };

                // Act
                var destinationDto = _mapper.Map<DestinationDto>(destination);

                // Assert
                destinationDto.Should().NotBeNull();
                destinationDto.Type.Should().Be(type);
                destinationDto.Name.Should().Be($"Test {type}");
            }
        }

        [Fact]
        public void Map_DestinationWithEmptyStrings_ShouldMapCorrectly()
        {
            // Arrange
            var destination = new Destination
            {
                ID = 1,
                Name = "",
                Description = "",
                CountryCode = "",
                Type = DestinationType.Beach,
                LastModif = DateTime.UtcNow
            };

            // Act
            var destinationDto = _mapper.Map<DestinationDto>(destination);

            // Assert
            destinationDto.Should().NotBeNull();
            destinationDto.Name.Should().Be("");
            destinationDto.Description.Should().Be("");
            destinationDto.CountryCode.Should().Be("");
        }

        [Fact]
        public void Map_DestinationWithNullValues_ShouldMapCorrectly()
        {
            // Arrange
            var destination = new Destination
            {
                ID = 1,
                Name = null!,
                Description = null!,
                CountryCode = null!,
                Type = DestinationType.Beach,
                LastModif = DateTime.UtcNow
            };

            // Act
            var destinationDto = _mapper.Map<DestinationDto>(destination);

            // Assert
            destinationDto.Should().NotBeNull();
            destinationDto.Name.Should().BeNull();
            destinationDto.Description.Should().BeNull();
            destinationDto.CountryCode.Should().BeNull();
        }

        [Fact]
        public void Map_CreateDestinationDtoWithAllTypes_ShouldMapCorrectly()
        {
            // Arrange
            var destinationTypes = Enum.GetValues<DestinationType>();
            
            foreach (var type in destinationTypes)
            {
                var createDto = new CreateDestinationDto
                {
                    Name = $"Test {type}",
                    Description = $"Description for {type}",
                    CountryCode = "TST",
                    Type = type
                };

                // Act
                var destination = _mapper.Map<Destination>(createDto);

                // Assert
                destination.Should().NotBeNull();
                destination.Type.Should().Be(type);
                destination.Name.Should().Be($"Test {type}");
            }
        }

        [Fact]
        public void Map_UpdateDestinationDtoWithAllTypes_ShouldMapCorrectly()
        {
            // Arrange
            var destinationTypes = Enum.GetValues<DestinationType>();
            
            foreach (var type in destinationTypes)
            {
                var updateDto = new UpdateDestinationDto
                {
                    Name = $"Updated {type}",
                    Description = $"Updated description for {type}",
                    CountryCode = "UPD",
                    Type = type
                };

                // Act
                var destination = _mapper.Map<Destination>(updateDto);

                // Assert
                destination.Should().NotBeNull();
                destination.Type.Should().Be(type);
                destination.Name.Should().Be($"Updated {type}");
            }
        }
    }
}
