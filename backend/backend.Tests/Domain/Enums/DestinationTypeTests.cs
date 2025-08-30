using Xunit;
using FluentAssertions;
using backend.Domain.Enums;

namespace backend.Tests.Domain.Enums
{
    /// <summary>
    /// Tests unitarios para el enum DestinationType
    /// Verifica solo la funcionalidad esencial del enum
    /// </summary>
    public class DestinationTypeTests
    {
        [Fact]
        public void DestinationType_ShouldHaveSixValues()
        {
            // Act
            var values = Enum.GetValues<DestinationType>();

            // Assert
            values.Should().HaveCount(6);
        }

        [Fact]
        public void DestinationType_ShouldHaveAllExpectedTypes()
        {
            // Act
            var values = Enum.GetValues<DestinationType>();

            // Assert
            values.Should().Contain(DestinationType.Beach);
            values.Should().Contain(DestinationType.Mountain);
            values.Should().Contain(DestinationType.City);
            values.Should().Contain(DestinationType.Cultural);
            values.Should().Contain(DestinationType.Adventure);
            values.Should().Contain(DestinationType.Relax);
        }
    }
}
