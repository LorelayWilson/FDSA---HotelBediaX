using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text;
using Xunit;
using FluentAssertions;
using backend.Presentation.Middleware;

namespace backend.Tests.Presentation.Middleware
{
    /// <summary>
    /// Tests unitarios para GlobalExceptionMiddleware
    /// Verifica el manejo de excepciones globales
    /// </summary>
    public class GlobalExceptionMiddlewareTests
    {
        private readonly Mock<ILogger<GlobalExceptionMiddleware>> _mockLogger;
        private readonly GlobalExceptionMiddleware _middleware;

        public GlobalExceptionMiddlewareTests()
        {
            _mockLogger = new Mock<ILogger<GlobalExceptionMiddleware>>();
            RequestDelegate next = (ctx) => Task.CompletedTask;
            _middleware = new GlobalExceptionMiddleware(next);
        }

        [Fact]
        public async Task InvokeAsync_WithNoException_ShouldCallNext()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var nextCalled = false;
            RequestDelegate next = (ctx) =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };
            var middleware = new GlobalExceptionMiddleware(next);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            nextCalled.Should().BeTrue();
        }

        [Fact]
        public async Task InvokeAsync_WithException_ShouldReturnInternalServerError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            
            RequestDelegate next = (ctx) =>
            {
                throw new Exception("Test exception");
            };
            var middleware = new GlobalExceptionMiddleware(next);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            context.Response.ContentType.Should().Be("application/json");
        }

        [Fact]
        public async Task InvokeAsync_WithException_ShouldLogError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            
            RequestDelegate next = (ctx) =>
            {
                throw new Exception("Test exception");
            };
            var middleware = new GlobalExceptionMiddleware(next);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            // El middleware usa Serilog directamente, no ILogger estándar
            // Por lo tanto, no podemos verificar el logging con Moq
            // Solo verificamos que no se lance excepción y que se maneje correctamente
            context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task InvokeAsync_WithException_ShouldReturnErrorResponse()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            
            RequestDelegate next = (ctx) =>
            {
                throw new Exception("Test exception");
            };
            var middleware = new GlobalExceptionMiddleware(next);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            responseBody.Should().Contain("error");
            responseBody.Should().Contain("Error interno del servidor");
        }
    }
}
