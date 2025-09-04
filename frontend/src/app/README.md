# Estructura del Frontend - HotelBediaX

## Arquitectura Modular Escalable

Este frontend está organizado siguiendo las mejores prácticas de Angular con una arquitectura modular que facilita el mantenimiento y la escalabilidad.

## Estructura de Carpetas

```
src/app/
├── core/                    # Servicios y funcionalidades core
│   ├── services/           # Servicios base y específicos
│   ├── guards/             # Guards de autenticación/autorización
│   ├── interceptors/       # Interceptors HTTP
│   └── models/             # Modelos de dominio
├── shared/                 # Componentes y utilidades reutilizables
│   ├── components/         # Componentes compartidos
├── features/               # Módulos de funcionalidades
│   ├── destinations/       # Gestión de destinos turísticos
├── layout/                # Componentes de layout
│   ├── header/            # Cabecera de la aplicación
│   └── main-layout/       # Layout principal
└── app.*                  # Archivos principales de la app
```

## Principios de Organización

### **Core Module**
- **Servicios base**: `BaseApiService` para operaciones CRUD genéricas
- **Interceptors**: Configuración de API y manejo de errores
- **Guards**: Autenticación y autorización
- **Modelos de dominio**: Entidades específicas del negocio

### **Shared Module**
- **Componentes reutilizables**: Botones, modales, formularios genéricos
- **Pipes**: Transformaciones de datos comunes
- **Directives**: Comportamientos reutilizables
- **Modelos compartidos**: Interfaces y tipos comunes

### **Features Module**
- **Módulos independientes**: Cada feature es autónoma
- **Lazy loading**: Carga bajo demanda para optimizar rendimiento
- **Componentes específicos**: Solo para esa funcionalidad
- **Servicios específicos**: Lógica de negocio de la feature

### **Layout Module**
- **Componentes de estructura**: Header, sidebar, footer
- **Navegación**: Menús y enlaces
- **Responsive design**: Adaptación a diferentes pantallas

## Beneficios de esta Estructura

### **Escalabilidad**
- Fácil agregar nuevas features sin afectar existentes
- Separación clara de responsabilidades
- Código organizado y mantenible

### **Reutilización**
- Componentes y servicios compartidos
- Patrones consistentes en toda la aplicación
- Reducción de duplicación de código

### **Testing**
- Tests organizados por módulos
- Mocks y utilidades compartidas
- Cobertura de código estructurada

### **Performance**
- Lazy loading de features
- Tree shaking optimizado
- Carga bajo demanda

## Convenciones de Naming

### **Carpetas**
- **kebab-case**: `destinations-page`, `main-layout`
- **Descriptivo**: Nombres que explican el propósito

### **Archivos**
- **Componentes**: `component-name.component.ts`
- **Servicios**: `service-name.service.ts`
- **Modelos**: `model-name.model.ts`
- **Tests**: `component-name.component.spec.ts`

### **Clases y Interfaces**
- **PascalCase**: `DestinationsPageComponent`
- **Descriptivo**: Nombres claros y específicos

## Futuras Mejoras

### **Próximos Módulos**
- **Hotels**: Gestión de hoteles y alojamientos
- **Bookings**: Sistema de reservas
- **Users**: Gestión de usuarios y perfiles
- **Reports**: Reportes y estadísticas

### **Mejoras Técnicas**
- **State Management**: NgRx para estado global
- **PWA**: Progressive Web App capabilities
- **Testing**: E2E con Cypress
- **CI/CD**: Pipeline de despliegue automatizado

## Cómo Agregar una Nueva Feature

1. **Crear carpeta** en `features/nueva-feature/`
2. **Implementar componentes** standalone
3. **Crear servicios** específicos
4. **Definir modelos** en `core/models/`
5. **Agregar rutas** con lazy loading
6. **Implementar tests** unitarios
7. **Documentar** la funcionalidad

Esta estructura asegura que el proyecto sea mantenible, escalable y siga las mejores prácticas de Angular.
