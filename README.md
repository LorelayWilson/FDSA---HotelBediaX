# HotelBediaX - Portal de Gestión de Destinos Turísticos

## Descripción del Proyecto

**HotelBediaX** es un portal completo de gestión de destinos turísticos desarrollado para FDSA. La aplicación permite a los usuarios gestionar destinos turísticos con operaciones CRUD completas, filtrado avanzado y una interfaz moderna y responsive.

### Objetivos de la Prueba Técnica

- ✅ **Backend**: API REST con .NET 9
- ✅ **Frontend**: SPA con Angular (en desarrollo)
- ✅ **Base de Datos**: Mock database para demostración
- ✅ **Funcionalidades**: CRUD completo + filtrado + paginación
- ✅ **Rendimiento**: Optimizado para manejar 200k+ registros

## Arquitectura del Proyecto

```
HotelBediaX/
├── backend/                 # API REST con .NET 9
│   ├── backend/            # Proyecto principal
│   │   ├── Controllers/    # Controladores de la API
│   │   ├── Data/           # Contexto de Entity Framework
│   │   ├── DTOs/           # Objetos de transferencia de datos
│   │   ├── Mapping/        # Configuración de AutoMapper
│   │   ├── Models/         # Entidades del dominio
│   │   ├── Services/       # Lógica de negocio
│   │   └── Program.cs      # Configuración de la aplicación
│   └── backend.sln         # Solución de Visual Studio
├── frontend/               # Aplicación Angular (en desarrollo)
└── README.md               # Este archivo
```

## Backend - API REST (.NET 9)

### Tecnologías Utilizadas

- **.NET 9** - Framework de desarrollo
- **Entity Framework Core** - ORM para acceso a datos
- **AutoMapper** - Mapeo automático entre entidades y DTOs
- **Swagger/OpenAPI** - Documentación automática de la API
- **Base de Datos en Memoria** - Mock database para demostración

### Modelo de Datos

#### Entidad Principal: `Destination`

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

#### Tipos de Destino Disponibles

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

### Endpoints de la API

#### Operaciones CRUD Principales

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/destinations` | Lista paginada con filtros |
| `GET` | `/api/destinations/{id}` | Obtener destino por ID |
| `POST` | `/api/destinations` | Crear nuevo destino |
| `PUT` | `/api/destinations/{id}` | Actualizar destino existente |
| `DELETE` | `/api/destinations/{id}` | Eliminar destino |

#### Endpoints de Soporte

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/destinations/countries` | Lista de códigos de países |
| `GET` | `/api/destinations/types` | Lista de tipos de destino |

### Funcionalidades de Filtrado

#### Parámetros de Filtrado

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

#### Ejemplos de Uso

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

### Datos de Ejemplo Incluidos

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

### Arquitectura del Backend

#### Patrón de Diseño Implementado

```
Controllers → Services → Data Layer
     ↓           ↓         ↓
   API REST   Business   Entity Framework
   Endpoints   Logic      + In-Memory DB
```

#### Componentes Principales

- **`DestinationsController`**: Maneja las peticiones HTTP
- **`IDestinationService`**: Interfaz de la lógica de negocio
- **`DestinationService`**: Implementación de la lógica de negocio
- **`ApplicationDbContext`**: Contexto de Entity Framework
- **`DataSeedService`**: Población automática de datos de ejemplo

### Configuración y Dependencias

#### Paquetes NuGet Utilizados

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="9.0.8" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="Microsoft.AspNetCore.Cors" Version="2.2.0" />
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.8" />
```

#### Configuración de Servicios

- **CORS**: Configurado para Angular (puerto 4200)
- **Entity Framework**: Base de datos en memoria
- **AutoMapper**: Mapeo automático entre entidades y DTOs
- **Swagger**: Documentación automática de la API

## Frontend - Angular (En Desarrollo)

### Estado Actual
- ❌ **No implementado aún**
- 🔄 **Pendiente de desarrollo**

### Funcionalidades Planificadas
- **SPA (Single Page Application)** con Angular
- **Interfaz moderna y responsive** basada en el wireframe proporcionado
- **Componentes para operaciones CRUD** de destinos
- **Sistema de filtrado y búsqueda** avanzado
- **Paginación** para grandes volúmenes de datos
- **Navegación entre módulos** (preparado para expansión futura)

### Diseño de la Interfaz
Basado en el wireframe proporcionado, la interfaz incluirá:
- **Tabla principal** con columnas: ID, Name, Description, CountryCode, Type, LastModif
- **Botones de acción**: Create, Modify, Remove
- **Panel de filtros** lateral con búsqueda y selección de datos
- **Diseño responsive** para diferentes dispositivos

## Cómo Ejecutar el Proyecto

### Prerrequisitos

- **.NET 9 SDK** instalado
- **Visual Studio 2022** o **VS Code** (recomendado)
- **Node.js** (para el frontend futuro)

### Ejecutar el Backend

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

- **API REST**: `https://localhost:7000/api/destinations`
- **Swagger UI**: `https://localhost:7000/swagger` (solo en desarrollo)
- **Frontend**: `http://localhost:4200` (cuando esté implementado)

## Testing de la API

### Probar Endpoints con Swagger

1. Ejecutar el backend
2. Abrir `https://localhost:7000/swagger`
3. Probar los endpoints directamente desde la interfaz

### Probar con Postman/Insomnia

```http
# Obtener todos los destinos
GET https://localhost:7000/api/destinations

# Crear un nuevo destino
POST https://localhost:7000/api/destinations
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

### Capacidad de Escalabilidad

- **Diseñado para 200k+ registros** como especifica la prueba técnica
- **Filtrado eficiente** por múltiples criterios
- **Paginación configurable** para diferentes tamaños de página
- **Arquitectura preparada** para migración a base de datos real

## Próximos Pasos

### Desarrollo del Frontend
1. **Crear proyecto Angular** con estructura modular
2. **Implementar componentes** para gestión de destinos
3. **Integrar con la API** del backend
4. **Implementar interfaz** basada en el wireframe
5. **Testing y optimización** de la interfaz

### Mejoras Futuras del Backend
- **Autenticación y autorización** (JWT)
- **Logging y monitoreo** avanzado
- **Caching** para mejorar rendimiento
- **Testing unitario** completo
- **Migración a base de datos real** (SQL Server, PostgreSQL)

## Notas de Desarrollo

### Decisiones de Arquitectura

- **Base de datos en memoria**: Cumple el requisito de "mock the database"
- **Patrón Repository**: Separación clara de responsabilidades
- **DTOs**: Transferencia segura de datos entre capas
- **Validaciones**: Data Annotations para validación de entrada

### Buenas Prácticas Implementadas

- **Inyección de dependencias** para servicios
- **Manejo de errores** robusto y consistente
- **Documentación XML** para IntelliSense
- **Código limpio** y mantenible
- **Separación de responsabilidades** clara

## Contacto y Soporte

### Desarrollador
- **Proyecto**: Prueba técnica para FDSA
- **Tecnologías**: .NET 9, Entity Framework Core, Angular
- **Arquitectura**: API REST, SPA, Clean Architecture

---

**HotelBediaX** - Portal de gestión de destinos turísticos desarrollado con las mejores prácticas y tecnologías modernas.
