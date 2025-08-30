using backend.Data;
using backend.Services;
using backend.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Configuración y construcción de la aplicación web
var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithProcessId()
        .Enrich.WithThreadId());

try
{
    Log.Information("Iniciando HotelBediaX API...");

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

// Configurar Swagger/OpenAPI para documentación de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "HotelBediaX API",
        Version = "v1",
        Description = "API para la gestión de destinos turísticos de HotelBediaX",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Lorelay Pricop",
            Email = "lorelay.pricop@gmail.com"
        }
    });
    
    // Incluir comentarios XML para documentación
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

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

// Configurar middleware de manejo de excepciones globales
app.UseGlobalExceptionMiddleware();

// Configurar Swagger/OpenAPI solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelBediaX API v1");
        c.RoutePrefix = "swagger"; // Swagger UI estará disponible en /swagger
        c.DocumentTitle = "HotelBediaX API Documentation";
    });
}

// Redirigir HTTP a HTTPS para mayor seguridad
app.UseHttpsRedirection();

// Aplicar política CORS configurada anteriormente
app.UseCors("AllowAngularApp");

// Agregar middleware de autorización
app.UseAuthorization();

// Mapear controladores de la API
app.MapControllers();

// ============================================================================
// INICIAR LA APLICACIÓN
// ============================================================================

app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Error fatal al iniciar la aplicación HotelBediaX");
}
finally
{
    Log.CloseAndFlush();
}

// Hacer la clase Program pública para las pruebas de integración
public partial class Program { }
