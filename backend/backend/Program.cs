using backend.Data;
using backend.Services;
using Microsoft.EntityFrameworkCore;

// Configuración y construcción de la aplicación web
var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// CONFIGURACIÓN DE SERVICIOS
// ============================================================================

// Agregar controladores MVC
builder.Services.AddControllers();

// Configurar CORS para permitir comunicación con el frontend Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            // Permitir solo el origen del frontend Angular
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()      // Permitir cualquier header HTTP
                  .AllowAnyMethod();     // Permitir cualquier método HTTP (GET, POST, PUT, DELETE)
        });
});

// Configurar Entity Framework Core con base de datos en memoria
// Esta es la implementación del "mock database" solicitado en la prueba técnica
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("HotelBediaXDb"));

// Configurar AutoMapper para mapeo automático entre entidades y DTOs
// Busca automáticamente todos los perfiles en el assembly
builder.Services.AddAutoMapper(typeof(Program));

// Registrar servicios de la aplicación
builder.Services.AddScoped<IDestinationService, DestinationService>();  // Servicio principal de destinos
builder.Services.AddScoped<DataSeedService>();                          // Servicio para poblar datos de ejemplo

// Configurar OpenAPI/Swagger para documentación de la API
builder.Services.AddOpenApi();

// ============================================================================
// CONSTRUCCIÓN Y CONFIGURACIÓN DE LA APLICACIÓN
// ============================================================================

var app = builder.Build();

// Ejecutar seed de datos al iniciar la aplicación
// Solo se ejecuta si la base de datos está vacía
using (var scope = app.Services.CreateScope())
{
    var seedService = scope.ServiceProvider.GetRequiredService<DataSeedService>();
    await seedService.SeedDataAsync();
}

// ============================================================================
// CONFIGURACIÓN DEL PIPELINE HTTP
// ============================================================================

// Configurar Swagger/OpenAPI solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Redirigir HTTP a HTTPS para mayor seguridad
app.UseHttpsRedirection();

// Aplicar política CORS configurada anteriormente
app.UseCors("AllowAngularApp");

// Agregar middleware de autorización (preparado para futuras implementaciones)
app.UseAuthorization();

// Mapear controladores de la API
app.MapControllers();

// ============================================================================
// INICIAR LA APLICACIÓN
// ============================================================================

app.Run();
