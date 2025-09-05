# HotelBediaX Backend - API REST

API REST desarrollada con .NET 9 para la gestión de destinos turísticos, implementando **Arquitectura Hexagonal** con patrones CQRS, Repository y Unit of Work.

## Inicio Rápido

### Prerrequisitos
- .NET 9 SDK
- Visual Studio 2022 o VS Code

### Ejecutar la Aplicación
```bash
cd backend/backend
dotnet restore
dotnet run
```

### Acceso
- **API**: `http://localhost:5259/api/v1/destinations`
- **Swagger**: `http://localhost:5259/swagger`

## Arquitectura Hexagonal

### Estructura del Proyecto

```
backend/
├── Domain/               # DOMINIO - Lógica de negocio pura
│   ├── Entities/         # Entidades del dominio
│   ├── Enums/           # Enumeraciones
│   └── Interfaces/      # Interfaces del dominio (Ports)
├── Application/          # APLICACIÓN - Casos de uso y reglas
│   ├── Commands/        # CQRS - Comandos para escritura
│   ├── Queries/         # CQRS - Queries para lectura
│   ├── DTOs/            # Objetos de transferencia
│   └── Mapping/         # Configuración AutoMapper
├── Infrastructure/       # INFRAESTRUCTURA - Persistencia
│   ├── Data/            # Contexto Entity Framework
│   ├── Repositories/    # Repository Pattern (Adapters)
│   ├── Services/        # Servicios de infraestructura
│   └── UnitOfWork/      # Unit of Work Pattern
└── Presentation/         # PRESENTACIÓN - API y middleware
    ├── Controllers/     # Controladores de la API
    └── Middleware/     # Middleware personalizado
```

### Flujo de Dependencias

```
Presentation → Application → Domain
     ↓              ↓
Infrastructure → Application → Domain
```

**Regla**: Las dependencias siempre apuntan hacia el centro (Domain).

### Decisiones Arquitectónicas

#### Arquitectura Optimizada

La estructura actual sigue **Clean Architecture** con **CQRS** usando **MediatR**:

- **DTOs centralizados** en `Application/DTOs` para reutilización entre Commands, Queries y Controllers
- **Interfaces en Domain** para mantener la inversión de dependencias
- **Handlers como servicios** en lugar de servicios tradicionales
- **Controladores limpios** que solo delegan a MediatR

### Beneficios

- **Testabilidad**: Domain y Application fáciles de testear
- **Independencia**: El dominio no depende de frameworks
- **Flexibilidad**: Fácil cambio de implementaciones
- **Mantenibilidad**: Separación clara de responsabilidades
- **Escalabilidad**: Cada capa evoluciona independientemente

## Modelo de Datos

### Entidad Principal: `Destination`

```csharp
public class Destination
{
    public int ID { get; set; }                    // Identificador único
    public string Name { get; set; }               // Nombre del destino
    public string Description { get; set; }        // Descripción detallada
    public string CountryCode { get; set; }        // Código ISO del país (3 chars)
    public DestinationType Type { get; set; }      // Tipo de destino
    public DateTime LastModif { get; set; }        // Última modificación
}
```

### Tipos de Destino

```csharp
public enum DestinationType
{
    Beach,      // Destinos de playa y costa
    Mountain,   // Destinos de montaña
    City,       // Destinos urbanos
    Cultural,   // Patrimonio cultural e histórico
    Adventure,  // Actividades de aventura
    Relax       // Destinos de relajación
}
```

## API Endpoints

### Operaciones CRUD

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/v1/destinations` | Lista paginada con filtros |
| `GET` | `/api/v1/destinations/{id}` | Obtener destino por ID |
| `POST` | `/api/v1/destinations` | Crear nuevo destino |
| `PUT` | `/api/v1/destinations/{id}` | Actualizar destino |
| `DELETE` | `/api/v1/destinations/{id}` | Eliminar destino |

### Endpoints de Soporte

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/v1/destinations/countries` | Lista de códigos de países |
| `GET` | `/api/v1/destinations/types` | Lista de tipos de destino |

### Filtros Disponibles

```http
# Búsqueda por texto
GET /api/v1/destinations?searchTerm=playa

# Filtro por país
GET /api/v1/destinations?countryCode=MEX

# Filtro por tipo
GET /api/v1/destinations?type=Beach

# Combinación de filtros
GET /api/v1/destinations?searchTerm=playa&countryCode=MEX&type=Beach&page=1&pageSize=10
```

### Versionado de API

La API soporta múltiples métodos de versionado:

1. **URL Path**: `/api/v1/destinations` (recomendado)
2. **Query String**: `?version=1.0`
3. **Header**: `api-version: 1.0`

## Tecnologías

### Core
- **.NET 9**: Framework de desarrollo
- **ASP.NET Core**: Framework web para APIs REST
- **Entity Framework Core**: ORM para acceso a datos
- **Base de Datos en Memoria**: Mock database para demostración

### Patrones y Librerías
- **MediatR**: Implementación de CQRS y patrón Mediator
- **AutoMapper**: Mapeo automático entre entidades y DTOs
- **Repository Pattern**: Abstracción del acceso a datos
- **Unit of Work**: Coordinación de transacciones

### Documentación y Logging
- **Swagger/OpenAPI**: Documentación automática de la API
- **Serilog**: Logging estructurado con múltiples sinks

## Datos de Ejemplo

El sistema incluye **10 destinos turísticos reales**:

- **Playa del Carmen** (MEX) - Beach
- **Santorini** (GRC) - Cultural  
- **Kyoto** (JPN) - Cultural
- **Machu Picchu** (PER) - Adventure
- **París** (FRA) - City
- **Nueva York** (USA) - City
- **Barcelona** (ESP) - Cultural
- **Río de Janeiro** (BRA) - City
- **Alpes Suizos** (CHE) - Mountain
- **Bali** (IDN) - Relax

## Testing

Para información detallada sobre testing, cobertura y ejecución de tests, consulta el [README del proyecto de tests](../backend.Tests/README.md).

## Configuración

### Paquetes NuGet Principales

**Core ASP.NET:**
- Microsoft.AspNetCore.OpenApi (9.0.8)
- Swashbuckle.AspNetCore (7.2.0)

**Entity Framework:**
- Microsoft.EntityFrameworkCore (9.0.8)
- Microsoft.EntityFrameworkCore.InMemory (9.0.8)

**CQRS and Mediator:**
- MediatR (11.1.0)
- MediatR.Extensions.Microsoft.DependencyInjection (11.1.0)

**AutoMapper:**
- AutoMapper.Extensions.Microsoft.DependencyInjection (12.0.1)

**Logging:**
- Serilog.AspNetCore (8.0.3)
- Serilog.Sinks.Console (6.0.0)
- Serilog.Sinks.File (6.0.0)

**API Versioning:**
- Microsoft.AspNetCore.Mvc.Versioning (5.1.0)
- Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer (5.1.0)

### Configuración de Servicios

- **CORS**: Configurado para Angular (puerto 4200)
- **Entity Framework**: Base de datos en memoria con índices optimizados
- **AutoMapper**: Mapeo automático entre entidades y DTOs
- **MediatR**: Configuración automática de handlers CQRS
- **API Versioning**: Configuración para versionado de API
- **Swagger**: Documentación automática de la API
- **Middleware**: Manejo global de excepciones

## Características de Rendimiento

### Optimizaciones Implementadas

- **Índices de base de datos** en campos de filtrado frecuente
- **Paginación eficiente** para grandes volúmenes de datos
- **Consultas optimizadas** con Entity Framework Core
- **Manejo asíncrono** de todas las operaciones
- **Middleware optimizado** para manejo de errores
- **Logging estructurado** para monitoreo

### Capacidad de Escalabilidad

- **Diseñado para 200k+ registros** como especifica la prueba técnica
- **Filtrado eficiente** por múltiples criterios
- **Paginación configurable** para diferentes tamaños de página
- **Arquitectura preparada** para migración a base de datos real
- **Manejo de errores robusto** para alta disponibilidad

## Troubleshooting

### Error: "Unable to bind to https://localhost:5259"
- Verificar que el puerto 5259 no esté en uso
- Cambiar el puerto en `launchSettings.json` si es necesario

### Error: "Database context disposed"
- Verificar que el contexto esté configurado correctamente en `Program.cs`
- Asegurar que se use `AddDbContext` con el scope correcto

### Error: "AutoMapper configuration invalid"
- Verificar que todos los mapeos estén definidos en `AutoMapperProfile`
- Ejecutar `config.AssertConfigurationIsValid()` en desarrollo

## Autor

**Lorelay Pricop Florescu**  
Graduada en Tecnologías Interactivas y Project Manager con experiencia en .NET, Python, Angular, Azure DevOps, IA y metodologías ágiles.

[LinkedIn](https://www.linkedin.com/in/lorelaypricop)  
Contacto: lorelaypricop@gmail.com

# Notas
> Algunas ideas relacionadas con validación, estilo y estructura se revisaron con el apoyo de herramientas de inteligencia artificial (IA), utilizadas para acelerar la documentación y validar casos límite.