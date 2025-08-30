# HotelBediaX - Portal de Gestión de Destinos Turísticos

## Descripción del Proyecto

**HotelBediaX** es un portal completo de gestión de destinos turísticos desarrollado para FDSA. La aplicación implementa **Arquitectura Hexagonal** (Ports and Adapters) con patrones CQRS, Repository y Unit of Work, permitiendo a los usuarios gestionar destinos turísticos con operaciones CRUD completas, filtrado avanzado, documentación automática con Swagger y una arquitectura optimizada y mantenible.

### Objetivos de la Prueba Técnica

- ✅ **Backend**: API REST con .NET 9
- ✅ **Frontend**: SPA con Angular (en desarrollo)
- ✅ **Base de Datos**: Mock database para demostración
- ✅ **Funcionalidades**: CRUD completo + filtrado + paginación
- ✅ **Rendimiento**: Optimizado para manejar 200k+ registros
- ✅ **Documentación**: Swagger/OpenAPI integrado
- ✅ **Testing**: Suite completa de pruebas unitarias e integración
- ✅ **Logging**: Logging estructurado con Serilog
- ✅ **Arquitectura**: Arquitectura Hexagonal (Ports and Adapters)
- ✅ **Patrones**: CQRS + Repository Pattern + Unit of Work + MediatR
- ✅ **API Versioning**: Infraestructura preparada para versionado futuro

## Arquitectura del Proyecto

### Estructura General

```
HotelBediaX/
├── backend/                 # API REST con .NET 9
│   ├── backend/            # Proyecto principal con Arquitectura Hexagonal
│   │   ├── Domain/         # Lógica de negocio pura
│   │   │   ├── Entities/   # Entidades del dominio
│   │   │   └── Enums/      # Enumeraciones del dominio
│   │   ├── Application/    # Casos de uso y reglas de aplicación
│   │   │   ├── Commands/   # Comandos CQRS
│   │   │   ├── Queries/    # Consultas CQRS
│   │   │   ├── DTOs/       # Data Transfer Objects
│   │   │   └── Mapping/    # Configuración de AutoMapper
│   │   ├── Infrastructure/ # Persistencia y servicios externos
│   │   │   ├── Data/       # Contexto de Entity Framework
│   │   │   ├── Repositories/ # Implementaciones de repositorios
│   │   │   ├── Services/   # Servicios de infraestructura
│   │   │   └── UnitOfWork/ # Implementación de Unit of Work
│   │   ├── Presentation/   # Controladores y middleware
│   │   │   ├── Controllers/ # Controladores de API
│   │   │   └── Middleware/ # Middleware personalizado
│   │   └── Program.cs      # Punto de entrada de la aplicación
│   ├── backend.Tests/      # Proyecto de pruebas organizado por capas
│   │   ├── Domain/         # Tests de entidades y enums
│   │   ├── Application/    # Tests de comandos, queries y mapeos
│   │   ├── Infrastructure/ # Tests de repositorios y servicios
│   │   ├── Presentation/   # Tests de controladores
│   │   ├── Integration/    # Tests de integración
│   │   └── Helpers/        # Utilidades para tests
│   └── backend.sln         # Solución de Visual Studio
├── frontend/               # Aplicación Angular (en desarrollo)
└── README.md               # Este archivo
```

### Arquitectura Hexagonal (Ports and Adapters)

El backend implementa **Arquitectura Hexagonal** que separa el código en capas bien definidas:

- **Domain Layer**: Entidades, enums e interfaces del dominio (independiente de frameworks)
- **Application Layer**: Casos de uso, CQRS (Commands/Queries), DTOs y mapeos
- **Infrastructure Layer**: Repositorios, contexto de datos, servicios externos
- **Presentation Layer**: Controladores de API, middleware y DTOs de presentación

**Beneficios**: Testabilidad, independencia de frameworks, flexibilidad, mantenibilidad y escalabilidad.

## Funcionalidades Principales

### Gestión de Destinos Turísticos
- **Crear** nuevos destinos turísticos
- **Leer** y visualizar destinos existentes
- **Actualizar** información de destinos
- **Eliminar** destinos no deseados
- **Filtrar** por país, tipo de destino o texto
- **Paginación** para manejar grandes volúmenes de datos

### Tipos de Destinos Soportados
- **Beach** - Destinos de playa y costa
- **Mountain** - Destinos de montaña
- **City** - Destinos urbanos
- **Cultural** - Patrimonio cultural e histórico
- **Adventure** - Actividades de aventura
- **Relax** - Destinos de relajación

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

- **API REST v1.0**: `https://localhost:7170/api/v1/destinations`
- **Swagger UI**: `https://localhost:7170/swagger` (solo en desarrollo)
- **Frontend**: `http://localhost:4200` (cuando esté implementado)

## Testing

### Ejecutar las Pruebas

```bash
# Ejecutar todas las pruebas
cd backend/backend.Tests
dotnet test
```

### Cobertura de Testing

- **✅ 81 pruebas** implementadas (cobertura completa)
- **✅ 81 pruebas exitosas** (100% de éxito)
- **✅ 0 pruebas con errores** (todos los errores corregidos)
- **✅ Tests organizados por capas** de arquitectura hexagonal
- **✅ Cobertura completa** de todos los componentes

## Frontend - Angular (En Desarrollo)

### Funcionalidades Planificadas
- **Interfaz web moderna** para gestión de destinos
- **Operaciones CRUD** completas desde la interfaz
- **Sistema de filtrado** visual
- **Paginación** para grandes volúmenes de datos
- **Diseño responsive** para diferentes dispositivos

## Próximos Pasos

### Desarrollo del Frontend
1. **Crear proyecto Angular** con estructura modular
2. **Implementar componentes** para gestión de destinos
3. **Integrar con la API** del backend
4. **Implementar interfaz** basada en el wireframe
5. **Testing y optimización** de la interfaz

### Características Implementadas

#### **Arquitectura Hexagonal (Ports and Adapters)**
- ✅ **Domain Layer** - Lógica de negocio pura e independiente de frameworks
- ✅ **Application Layer** - Casos de uso con CQRS (Commands/Queries)
- ✅ **Infrastructure Layer** - Implementaciones concretas de repositorios y servicios
- ✅ **Presentation Layer** - Controladores de API y middleware
- ✅ **Repository Pattern** - Abstracción del acceso a datos
- ✅ **Unit of Work** - Coordinación de transacciones complejas
- ✅ **MediatR** - Desacoplamiento entre controladores y lógica de negocio
- ✅ **Handlers especializados** - Cada operación tiene su handler específico
- ✅ **API Versioning** - Infraestructura preparada para versionado futuro

#### **Estructura del Proyecto**
- ✅ **Arquitectura hexagonal** - Implementación completa de Ports and Adapters
- ✅ **Namespaces organizados** - Estructura clara por capas arquitectónicas
- ✅ **Tests organizados** - Tests estructurados por capas de arquitectura
- ✅ **Cobertura completa de tests** - 81 tests implementados con 100% de éxito
- ✅ **Tests organizados por capas** - Cobertura total de todas las capas arquitectónicas
- ✅ **Documentación actualizada** - READMEs actualizados con nueva arquitectura

#### **Logging Estructurado**
- ✅ **Serilog** configurado con múltiples sinks (Console, File)
- ✅ **Logging eficiente** - solo operaciones críticas
- ✅ **Logs estructurados** con propiedades JSON para búsqueda eficiente
- ✅ **Rotación automática** de archivos de log
- ✅ **Contexto completo** en errores con RequestId único
- ✅ **Configuración por ambiente** (Development vs Production)

## Documentación Detallada

Para información más específica sobre cada componente del proyecto, consulta los READMEs correspondientes:

- **[Backend API](./backend/backend/README.md)** - Documentación completa del backend con Arquitectura Hexagonal, endpoints, configuración y troubleshooting
- **[Testing](./backend/backend.Tests/README.md)** - Documentación completa de las pruebas unitarias e integración, cobertura y mejores prácticas

## Author

**Lorelay Pricop Florescu**  
Graduate in Interactive Technologies and Project Manager with experience in .NET, Python, Angular, Azure DevOps, AI, and Agile methodologies.

🔗 [LinkedIn](https://www.linkedin.com/in/lorelaypricop)  
📧 Contact: lorelaypricop@gmail.com

# Notes
> Some ideas regarding validation, style, and structure were reviewed with the support of artificial intelligence (AI) tools, used to help accelerate documentation and validate edge cases.