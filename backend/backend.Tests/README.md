# Tests Unitarios - HotelBediaX Backend

Este proyecto contiene los tests unitarios y de integración para el backend de HotelBediaX.

## Estructura del Proyecto - Tests para Arquitectura Hexagonal

```
backend.Tests/
├── Domain/               # Tests para capa de dominio
│   ├── Entities/         # Tests para entidades
│   │   └── DestinationTests.cs
│   └── Enums/           # Tests para enumeraciones
│       └── DestinationTypeTests.cs
├── Application/          # Tests para capa de aplicación
│   ├── Commands/        # Tests para command handlers
│   │   ├── CreateDestinationCommandHandlerTests.cs
│   │   ├── UpdateDestinationCommandHandlerTests.cs
│   │   └── DeleteDestinationCommandHandlerTests.cs
│   ├── Queries/         # Tests para query handlers
│   │   ├── GetDestinationsQueryHandlerTests.cs
│   │   ├── GetDestinationByIdQueryHandlerTests.cs
│   │   ├── GetCountriesQueryHandlerTests.cs
│   │   └── GetDestinationTypesQueryHandlerTests.cs
│   └── Mapping/         # Tests para AutoMapper
│       └── AutoMapperProfileTests.cs
├── Infrastructure/       # Tests para capa de infraestructura
│   ├── Repositories/    # Tests para repositorios
│   │   └── DestinationRepositoryTests.cs
│   ├── Services/        # Tests para servicios
│   │   └── DataSeedServiceTests.cs
│   └── UnitOfWork/      # Tests para Unit of Work
│       └── UnitOfWorkTests.cs
├── Presentation/         # Tests para capa de presentación
│   ├── Controllers/     # Tests para controladores
│   │   └── DestinationsControllerTests.cs
│   └── Middleware/     # Tests para middleware
│       └── GlobalExceptionMiddlewareTests.cs
├── Integration/          # Tests de integración (End-to-End)
│   └── DestinationsControllerIntegrationTests.cs
├── Helpers/              # Clases helper para tests
│   └── TestDataHelper.cs
└── README.md
```

### **Tests por Capa de Arquitectura Hexagonal**

#### **Domain Layer Tests** 
- **DestinationTests**: Tests para entidad del dominio
- **DestinationTypeTests**: Tests para enumeración de tipos

#### **Application Layer Tests** 
- **Command Handlers**: Tests para Create, Update, Delete destinations
- **Query Handlers**: Tests para GetDestinations, GetById, GetCountries, GetTypes
- **AutoMapperProfileTests**: Tests para mapeo de DTOs

#### **Infrastructure Layer Tests** 
- **DestinationRepositoryTests**: Tests para repositorio de destinos
- **DataSeedServiceTests**: Tests para servicio de datos iniciales
- **UnitOfWorkTests**: Tests para coordinación de transacciones

#### **Presentation Layer Tests** 
- **DestinationsControllerTests**: Tests unitarios para controladores
- **GlobalExceptionMiddlewareTests**: Tests para middleware de excepciones

#### **Integration Tests** 
- **DestinationsControllerIntegrationTests**: Tests end-to-end para API completa

## Tecnologías Utilizadas

- **xUnit**: Framework de testing
- **Moq**: Framework de mocking
- **FluentAssertions**: Librería de aserciones más legibles
- **Microsoft.EntityFrameworkCore.InMemory**: Base de datos en memoria para tests
- **Microsoft.AspNetCore.Mvc.Testing**: Testing de integración para APIs

## Tipos de Tests

### 1. Tests Unitarios

#### DestinationServiceTests
- Obtener destinos con y sin filtros
- Obtener destino por ID (válido e inválido)
- Crear nuevo destino
- Actualizar destino existente
- Eliminar destino
- Obtener lista de países
- Paginación y ordenamiento

#### DestinationsControllerTests
- Todos los endpoints GET, POST, PUT, DELETE
- Manejo de errores y excepciones
- Validación de ModelState
- Respuestas HTTP correctas (200, 201, 404, 500)

#### DataSeedServiceTests
- Poblar base de datos vacía
- No duplicar datos existentes
- Verificar destinos específicos
- Validar fechas y IDs únicos
- Cobertura de todos los tipos de destino

#### AutoMapperProfileTests
- Mapeo de entidades a DTOs
- Mapeo de DTOs a entidades
- Mapeo de listas
- Mapeo con valores nulos y vacíos
- Validación de configuración

### 2. Tests de Integración

#### DestinationsControllerIntegrationTests
- Flujo completo de la API
- Base de datos real (en memoria)
- Filtros y paginación
- Validación de datos
- Respuestas HTTP completas

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

// Mock del logger para servicios
_mockLogger = new Mock<ILogger<DestinationService>>();
_service = new DestinationService(_context, _mapper, _mockLogger.Object);
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
7. **Mocking de dependencias**: Uso de Moq para logger y servicios
8. **Logging en tests**: Verificación de que el logging funciona correctamente

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

## Author

**Lorelay Pricop Florescu**  
Graduate in Interactive Technologies and Project Manager with experience in .NET, Python, Angular, Azure DevOps, AI, and Agile methodologies.

[LinkedIn](https://www.linkedin.com/in/lorelaypricop)  
Contact: lorelaypricop@gmail.com

# Notes
> Some ideas regarding validation, style, and structure were reviewed with the support of artificial intelligence (AI) tools, used to help accelerate documentation and validate edge case