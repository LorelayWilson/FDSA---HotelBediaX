# Tests Unitarios - HotelBediaX Backend

Este proyecto contiene los tests unitarios y de integración para el backend de HotelBediaX.

## Estructura del Proyecto

```
backend.Tests/
├── Controllers/           # Tests para controladores
│   └── DestinationsControllerTests.cs
├── Services/             # Tests para servicios
│   ├── DestinationServiceTests.cs
│   └── DataSeedServiceTests.cs
├── Mapping/              # Tests para AutoMapper
│   └── AutoMapperProfileTests.cs
├── Integration/          # Tests de integración
│   └── DestinationsControllerIntegrationTests.cs
├── Helpers/              # Clases helper para tests
│   └── TestDataHelper.cs
└── README.md
```

## Tecnologías Utilizadas

- **xUnit**: Framework de testing
- **Moq**: Framework de mocking
- **FluentAssertions**: Librería de aserciones más legibles
- **Microsoft.EntityFrameworkCore.InMemory**: Base de datos en memoria para tests
- **Microsoft.AspNetCore.Mvc.Testing**: Testing de integración para APIs

## Tipos de Tests

### 1. Tests Unitarios

#### DestinationServiceTests
- ✅ Obtener destinos con y sin filtros
- ✅ Obtener destino por ID (válido e inválido)
- ✅ Crear nuevo destino
- ✅ Actualizar destino existente
- ✅ Eliminar destino
- ✅ Obtener lista de países
- ✅ Paginación y ordenamiento

#### DestinationsControllerTests
- ✅ Todos los endpoints GET, POST, PUT, DELETE
- ✅ Manejo de errores y excepciones
- ✅ Validación de ModelState
- ✅ Respuestas HTTP correctas (200, 201, 404, 500)

#### DataSeedServiceTests
- ✅ Poblar base de datos vacía
- ✅ No duplicar datos existentes
- ✅ Verificar destinos específicos
- ✅ Validar fechas y IDs únicos
- ✅ Cobertura de todos los tipos de destino

#### AutoMapperProfileTests
- ✅ Mapeo de entidades a DTOs
- ✅ Mapeo de DTOs a entidades
- ✅ Mapeo de listas
- ✅ Mapeo con valores nulos y vacíos
- ✅ Validación de configuración

### 2. Tests de Integración

#### DestinationsControllerIntegrationTests
- ✅ Flujo completo de la API
- ✅ Base de datos real (en memoria)
- ✅ Filtros y paginación
- ✅ Validación de datos
- ✅ Respuestas HTTP completas

## Cómo Ejecutar los Tests

### Desde la línea de comandos

```bash
# Navegar al directorio del proyecto de tests
cd backend/backend.Tests

# Restaurar paquetes NuGet
dotnet restore

# Ejecutar todos los tests
dotnet test

# Ejecutar tests con cobertura de código
dotnet test --collect:"XPlat Code Coverage"

# Ejecutar tests con salida detallada
dotnet test --verbosity normal

# Ejecutar tests específicos
dotnet test --filter "ClassName=DestinationServiceTests"
```

### Desde Visual Studio

1. Abrir el proyecto en Visual Studio
2. Ir a **Test Explorer** (Test → Test Explorer)
3. Hacer clic en **Run All Tests** o ejecutar tests individuales

### Desde Visual Studio Code

1. Instalar la extensión **.NET Core Test Explorer**
2. Abrir el proyecto
3. Ejecutar tests desde el panel de Test Explorer

## Cobertura de Código

Los tests cubren:

- ✅ **100%** de los métodos públicos de `DestinationService`
- ✅ **100%** de los endpoints de `DestinationsController`
- ✅ **100%** de la funcionalidad de `DataSeedService`
- ✅ **100%** de los mapeos de `AutoMapperProfile`

## Patrones de Testing Utilizados

### 1. Arrange-Act-Assert (AAA)
```csharp
[Fact]
public async Task GetDestination_WithValidId_ReturnsDestination()
{
    // Arrange
    var destination = TestDataHelper.CreateTestDestination();
    _context.Destinations.Add(destination);
    await _context.SaveChangesAsync();

    // Act
    var result = await _service.GetDestinationByIdAsync(destination.ID);

    // Assert
    result.Should().NotBeNull();
    result!.ID.Should().Be(destination.ID);
}
```

### 2. Mocking con Moq
```csharp
_mockService.Setup(s => s.GetDestinationByIdAsync(destinationId))
           .ReturnsAsync(expectedDestination);
```

### 3. Base de Datos en Memoria
```csharp
var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    .Options;
```

### 4. Aserciones con FluentAssertions
```csharp
result.Should().NotBeNull();
result.Items.Should().HaveCount(3);
result.TotalCount.Should().Be(3);
```

## Datos de Prueba

La clase `TestDataHelper` proporciona métodos para crear datos de prueba consistentes:

- `CreateTestDestination()`: Crea un destino individual
- `CreateTestDestinations()`: Crea una lista de destinos
- `CreateTestCreateDestinationDto()`: Crea DTO de creación
- `CreateTestUpdateDestinationDto()`: Crea DTO de actualización
- `CreateTestDestinationFilter()`: Crea filtro de búsqueda

## Mejores Prácticas Implementadas

1. **Aislamiento**: Cada test es independiente
2. **Limpieza**: Uso de `IDisposable` para limpiar recursos
3. **Nombres descriptivos**: Los nombres de los tests explican qué se está probando
4. **Datos de prueba consistentes**: Uso de helpers para crear datos
5. **Cobertura completa**: Tests para casos exitosos y de error
6. **Tests de integración**: Verificación del flujo completo

## Troubleshooting

### Error: "Database already exists"
- Los tests usan nombres únicos de base de datos con `Guid.NewGuid()`
- Si persiste, limpiar la base de datos en memoria

### Error: "AutoMapper configuration invalid"
- Verificar que todos los mapeos estén definidos en `AutoMapperProfile`
- Ejecutar `config.AssertConfigurationIsValid()` en los tests

### Tests lentos
- Usar base de datos en memoria en lugar de SQL Server
- Evitar operaciones de red en tests unitarios
- Usar mocks para dependencias externas

## Contribución

Al agregar nuevos tests:

1. Seguir el patrón AAA (Arrange-Act-Assert)
2. Usar nombres descriptivos para los tests
3. Agregar tests para casos de error
4. Mantener la cobertura de código alta
5. Documentar casos especiales o complejos
