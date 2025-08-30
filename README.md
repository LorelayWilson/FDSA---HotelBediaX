# HotelBediaX - Portal de Gestión de Destinos Turísticos

## Descripción del Proyecto

**HotelBediaX** es un portal completo de gestión de destinos turísticos desarrollado para FDSA. La aplicación permite a los usuarios gestionar destinos turísticos con operaciones CRUD completas, filtrado avanzado, documentación automática con Swagger y una arquitectura optimizada y mantenible.

### Objetivos de la Prueba Técnica

- ✅ **Backend**: API REST con .NET 9
- ✅ **Frontend**: SPA con Angular (en desarrollo)
- ✅ **Base de Datos**: Mock database para demostración
- ✅ **Funcionalidades**: CRUD completo + filtrado + paginación
- ✅ **Rendimiento**: Optimizado para manejar 200k+ registros
- ✅ **Documentación**: Swagger/OpenAPI integrado
- ✅ **Testing**: Suite completa de pruebas unitarias e integración
- ✅ **Logging**: Logging estructurado con Serilog
- ✅ **Arquitectura**: CQRS + Repository Pattern + Unit of Work
- ✅ **Patrones**: MediatR para desacoplamiento y mantenibilidad

## Arquitectura básica del Proyecto

```
HotelBediaX/
├── backend/                 # API REST con .NET 9
│   ├── backend/            # Proyecto principal
│   ├── backend.Tests/      # Proyecto de pruebas
│   └── backend.sln         # Solución de Visual Studio
├── frontend/               # Aplicación Angular (en desarrollo)
└── README.md               # Este archivo
```

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

- **API REST**: `https://localhost:7170/api/destinations`
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

- **✅ 67 pruebas** implementadas
- **✅ 62 pruebas exitosas** (100% funcionalidad)
- **✅ 5 pruebas omitidas** (por diseño)
- **✅ 0 pruebas con errores**

## Frontend - Angular (En Desarrollo)

### Estado Actual
- ❌ **No implementado aún**
- 🔄 **Pendiente de desarrollo**

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

### Mejoras Implementadas

#### **Arquitectura Avanzada**
- ✅ **CQRS** - Separación de Commands y Queries para mejor escalabilidad
- ✅ **Repository Pattern** - Abstracción del acceso a datos
- ✅ **Unit of Work** - Coordinación de transacciones complejas
- ✅ **MediatR** - Desacoplamiento entre controladores y lógica de negocio
- ✅ **Handlers especializados** - Cada operación tiene su handler específico

#### **Logging Estructurado**
- ✅ **Serilog** configurado con múltiples sinks (Console, File)
- ✅ **Logging eficiente** - solo operaciones críticas
- ✅ **Logs estructurados** con propiedades JSON para búsqueda eficiente
- ✅ **Rotación automática** de archivos de log
- ✅ **Contexto completo** en errores con RequestId único
- ✅ **Configuración por ambiente** (Development vs Production)

### Mejoras Futuras
- **Autenticación y autorización** de usuarios
- **Caching** para mejorar rendimiento
- **Migración a base de datos real** (SQL Server, PostgreSQL)
- **Métricas y monitoreo** avanzado con Application Insights
- **Protección de la API** con rate limiting
- **Logging centralizado** con ELK Stack o Azure Monitor

## Documentación Detallada

Para información más específica sobre cada componente del proyecto, consulta los READMEs correspondientes:

- **📁 [Backend API](./backend/backend/README.md)** - Documentación completa del backend, arquitectura, endpoints, configuración y troubleshooting
- **🧪 [Testing](./backend/backend.Tests/README.md)** - Documentación completa de las pruebas unitarias e integración, cobertura y mejores prácticas

## Author

**Lorelay Pricop Florescu**  
Graduate in Interactive Technologies and Project Manager with experience in .NET, Python, Angular, Azure DevOps, AI, and Agile methodologies.

🔗 [LinkedIn](https://www.linkedin.com/in/lorelaypricop)  
📧 Contact: lorelaypricop@gmail.com

# Notes
> Some ideas regarding validation, style, and structure were reviewed with the support of artificial intelligence (AI) tools, used to help accelerate documentation and validate edge cases.