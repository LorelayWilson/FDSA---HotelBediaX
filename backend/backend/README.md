# HotelBediaX Backend - API REST

Este es el proyecto principal del backend de HotelBediaX, una API REST desarrollada con .NET 9 para la gestión de destinos turísticos.

## Estructura del Proyecto

```
backend/
├── Controllers/           # Controladores de la API
│   └── DestinationsController.cs
├── Data/                 # Contexto de Entity Framework
│   └── ApplicationDbContext.cs
├── DTOs/                 # Objetos de transferencia de datos
│   └── DestinationDto.cs
├── Mapping/              # Configuración de AutoMapper
│   └── AutoMapperProfile.cs
├── Middleware/           # Middleware personalizado
│   └── GlobalExceptionMiddleware.cs
├── Models/               # Entidades del dominio
│   └── Destination.cs
├── Services/             # Lógica de negocio
│   ├── IDestinationService.cs
│   ├── DestinationService.cs
│   └── DataSeedService.cs
├── Properties/           # Configuración de la aplicación
│   └── launchSettings.json
├── appsettings.json      # Configuración de la aplicación
├── appsettings.Development.json
├── Program.cs            # Punto de entrada de la aplicación
├── backend.csproj        # Archivo de proyecto
└── README.md
```

## Tecnologías Utilizadas

- **.NET 9**: Framework de desarrollo
- **Entity Framework Core**: ORM para acceso a datos
- **AutoMapper**: Mapeo automático entre entidades y DTOs
- **Swagger/OpenAPI**: Documentación automática de la API
- **Serilog**: Logging estructurado con múltiples sinks
- **Base de Datos en Memoria**: Mock database para demostración
- **ASP.NET Core**: Framework web para APIs REST

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
    public DateTime LastModif { get; set; }        // Última modificación (auto-actualizada)
}
```

### Tipos de Destino Disponibles

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

## Endpoints de la API

### Operaciones CRUD Principales

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/destinations` | Lista paginada con filtros |
| `GET` | `/api/destinations/{id}` | Obtener destino por ID |
| `POST` | `/api/destinations` | Crear nuevo destino |
| `PUT` | `/api/destinations/{id}` | Actualizar destino existente |
| `DELETE` | `/api/destinations/{id}` | Eliminar destino |

### Endpoints de Soporte

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/destinations/countries` | Lista de códigos de países |
| `GET` | `/api/destinations/types` | Lista de tipos de destino |

## Logging Estructurado con Serilog

### Configuración de Logging

La aplicación utiliza **Serilog** para logging estructurado con las siguientes características:

#### Sinks Configurados
- **Console**: Salida en consola con formato estructurado
- **File**: Archivos de log con rotación diaria
  - Producción: `logs/hotelbediax-{date}.log` (30 días de retención)
  - Desarrollo: `logs/hotelbediax-dev-{date}.log` (7 días de retención)

#### Enrichers Aplicados
- **FromLogContext**: Contexto de la solicitud HTTP
- **WithMachineName**: Nombre de la máquina
- **WithProcessId**: ID del proceso
- **WithThreadId**: ID del hilo

#### Niveles de Logging
- **Desarrollo**: Debug (máximo detalle)
- **Producción**: Information (información relevante)
- **Microsoft/System**: Warning (solo advertencias importantes)

### Logging en la Aplicación

#### Controladores
```csharp
// Solo logs de advertencias y errores importantes
Log.Warning("Destino no encontrado con ID: {DestinationId}", id);
Log.Warning("Datos de entrada inválidos para crear destino: {@ModelState}", ModelState);
```

#### Servicios
```csharp
// Solo logs de operaciones críticas (Create, Update, Delete)
Log.Information("Destino creado: {DestinationName} (ID: {DestinationId}) en {CountryCode}", 
    destination.Name, destination.ID, destination.CountryCode);
Log.Warning("Destino no encontrado para actualizar con ID: {DestinationId}", id);
```

#### Middleware de Excepciones
```csharp
// Solo errores no manejados con contexto completo
Log.Error(ex, "Error no manejado en la aplicación para {Method} {Path}{QueryString} con RequestId {RequestId}", 
    method, path, queryString, requestId);
```

### Estructura de Logs

Los logs incluyen información estructurada como:
- **Timestamp**: Fecha y hora exacta
- **Level**: Nivel de logging (Debug, Information, Warning, Error, Fatal)
- **Message**: Mensaje descriptivo
- **Properties**: Datos estructurados (filtros, IDs, etc.)
- **Exception**: Stack trace completo para errores
- **RequestId**: Identificador único de la solicitud HTTP

### Ejemplo de Log de Producción
```
[2024-01-15 14:30:25.123 +00:00 INF] Destino creado: París (ID: 5) en FRA
{"DestinationName": "París", "DestinationId": 5, "CountryCode": "FRA", "RequestId": "0HMQ8VQKJQJQJ"}

[2024-01-15 14:31:10.456 +00:00 WRN] Destino no encontrado con ID: 999
{"DestinationId": 999, "RequestId": "0HMQ8VQKJQJQK"}

[2024-01-15 14:32:05.789 +00:00 ERR] Error no manejado en la aplicación para POST /api/destinations con RequestId 0HMQ8VQKJQJQL
{"Method": "POST", "Path": "/api/destinations", "QueryString": "", "RequestId": "0HMQ8VQKJQJQL", "Exception": "..."}
```

### Estrategia de Logging

#### **Logs Mínimos y Efectivos**
- **Operaciones CRUD**: Solo logs de éxito para Create/Update/Delete
- **Consultas**: Sin logs (operaciones frecuentes y no críticas)
- **Errores**: Siempre loggeados con contexto completo
- **Advertencias**: Solo para situaciones que requieren atención

#### **Beneficios del Diseño**
- **Logs limpios** sin información innecesaria
- **Información relevante** y accionable
- **Rendimiento optimizado** (menos operaciones de I/O)
- **Debugging eficiente** con logs enfocados

### Monitoreo y Debugging

Los logs estructurados permiten:
- **Búsqueda eficiente** por propiedades específicas
- **Análisis de operaciones críticas** (creaciones, actualizaciones, eliminaciones)
- **Seguimiento de errores** con RequestId y contexto completo
- **Monitoreo de advertencias** para problemas potenciales
- **Debugging** con información relevante y concisa

## Funcionalidades de Filtrado

### Parámetros de Filtrado

```csharp
public class DestinationFilterDto
{
    public string? SearchTerm { get; set; }        // Búsqueda por texto
    public string? CountryCode { get; set; }       // Filtro por país
    public DestinationType? Type { get; set; }     // Filtro por tipo
    public int Page { get; set; } = 1;             // Número de página
    public int PageSize { get; set; } = 20;        // Elementos por página
}
```

### Ejemplos de Uso

```http
# Búsqueda por texto
GET /api/destinations?searchTerm=playa

# Filtro por país
GET /api/destinations?countryCode=MEX

# Filtro por tipo
GET /api/destinations?type=Beach

# Combinación de filtros
GET /api/destinations?searchTerm=playa&countryCode=MEX&type=Beach&page=1&pageSize=10
```

## Datos de Ejemplo Incluidos

El sistema incluye **10 destinos turísticos reales** con datos completos:

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

## Arquitectura del Backend

### Patrón de Diseño Implementado

```
Controllers → Services → Data Layer
     ↓           ↓         ↓
   API REST   Business   Entity Framework
   Endpoints   Logic      + In-Memory DB
```

### Componentes Principales

- **`DestinationsController`**: Maneja las peticiones HTTP
- **`IDestinationService`**: Interfaz de la lógica de negocio
- **`DestinationService`**: Implementación de la lógica de negocio
- **`ApplicationDbContext`**: Contexto de Entity Framework
- **`DataSeedService`**: Población automática de datos de ejemplo
- **`GlobalExceptionMiddleware`**: Manejo centralizado de errores

## Configuración y Dependencias

### Paquetes NuGet Utilizados

```xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.8" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="7.2.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="9.0.8" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
```

### Configuración de Servicios

- **CORS**: Configurado para Angular (puerto 4200)
- **Entity Framework**: Base de datos en memoria con índices optimizados
- **AutoMapper**: Mapeo automático entre entidades y DTOs
- **Swagger**: Documentación automática de la API
- **Middleware**: Manejo global de excepciones

## Cómo Ejecutar el Proyecto

### Prerrequisitos

- **.NET 9 SDK** instalado
- **Visual Studio 2022** o **VS Code** (recomendado)

### Ejecutar la Aplicación

```bash
# Navegar al directorio del backend
cd backend/backend

# Restaurar dependencias
dotnet restore

# Compilar el proyecto
dotnet build

# Ejecutar la aplicación
dotnet run
```

### Acceso a la Aplicación

- **API REST**: `https://localhost:7170/api/destinations`
- **Swagger UI**: `https://localhost:7170/swagger` (solo en desarrollo)

### Información de la API

```yaml
Title: HotelBediaX API
Version: v1
Description: API para la gestión de destinos turísticos de HotelBediaX
Contact: Lorelay Pricop (lorelay.pricop@gmail.com)
```

La API incluye documentación automática completa con Swagger/OpenAPI:
- **Documentación interactiva** en `/swagger`
- **Pruebas en vivo** de todos los endpoints
- **Esquemas de datos** detallados
- **Ejemplos de uso** para cada endpoint

## Testing de la API

### Probar con Swagger (Recomendado)

1. Ejecutar el backend
2. Abrir `https://localhost:7170/swagger`
3. Probar los endpoints directamente desde la interfaz

### Probar con Postman/Insomnia

```http
# Obtener todos los destinos
GET https://localhost:7170/api/destinations

# Crear un nuevo destino
POST https://localhost:7170/api/destinations
Content-Type: application/json

{
  "name": "Nuevo Destino",
  "description": "Descripción del destino",
  "countryCode": "ARG",
  "type": "Adventure"
}
```

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

## Mejoras de Arquitectura Implementadas

### 🚀 **Refactoring y Optimizaciones**

- **✅ Middleware de Manejo de Errores Global**: Eliminación de código repetitivo en controladores
- **✅ Validaciones Unificadas**: Solo en DTOs para evitar duplicación
- **✅ Configuración EF Simplificada**: Solo índices esenciales
- **✅ Logging Estructurado**: Configuración específica por namespace
- **✅ Dependencias Actualizadas**: Eliminación de paquetes obsoletos

### 🛡️ **Manejo de Errores Robusto**

```csharp
// Middleware global que captura todas las excepciones
public class GlobalExceptionMiddleware
{
    // Manejo centralizado con códigos HTTP apropiados
    // Logging estructurado de errores
    // Respuestas JSON consistentes
}
```

### 📊 **Logging Mejorado**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning",
      "backend": "Debug"
    }
  }
}
```

## Próximos Pasos

### Mejoras Futuras del Backend
- **Autenticación y autorización** (JWT)
- **Caching** para mejorar rendimiento
- **Migración a base de datos real** (SQL Server, PostgreSQL)
- **Métricas y monitoreo** avanzado
- **Rate limiting** para protección de la API

## Notas de Desarrollo

### Decisiones de Arquitectura

- **Base de datos en memoria**: Cumple el requisito de "mock the database"
- **Patrón Service**: Separación clara de responsabilidades
- **DTOs**: Transferencia segura de datos entre capas
- **Validaciones**: Data Annotations para validación de entrada
- **Middleware**: Manejo centralizado de cross-cutting concerns

### Buenas Prácticas Implementadas

- **Inyección de dependencias** para servicios
- **Manejo de errores** robusto y consistente
- **Documentación XML** para IntelliSense
- **Código limpio** y mantenible
- **Separación de responsabilidades** clara
- **Logging estructurado** para debugging
- **Refactoring continuo** para mantener calidad

## Troubleshooting

### Error: "Unable to bind to https://localhost:7170"
- Verificar que el puerto 7170 no esté en uso
- Cambiar el puerto en `launchSettings.json` si es necesario

### Error: "Database context disposed"
- Verificar que el contexto esté configurado correctamente en `Program.cs`
- Asegurar que se use `AddDbContext` con el scope correcto

### Error: "AutoMapper configuration invalid"
- Verificar que todos los mapeos estén definidos en `AutoMapperProfile`
- Ejecutar `config.AssertConfigurationIsValid()` en desarrollo

## Author

**Lorelay Pricop Florescu**  
Graduate in Interactive Technologies and Project Manager with experience in .NET, Python, Angular, Azure DevOps, AI, and Agile methodologies.

🔗 [LinkedIn](https://www.linkedin.com/in/lorelaypricop)  
📧 Contact: lorelaypricop@gmail.com

# Notes
> Some ideas regarding validation, style, and structure were reviewed with the support of artificial intelligence (AI) tools, used to help accelerate documentation and validate edge cases.
