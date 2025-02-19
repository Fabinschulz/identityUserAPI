using IdentityUser.src.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace IdentityUser.src.Infra.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions that occur during the request processing pipeline.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // 500 por padrão
            var message = "Ocorreu um erro interno no servidor.";
            var errors = new string[] { };

            // Tratar exceções específicas
            switch (exception)
            {
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest; // 400
                    message = "Requisição inválida.";
                    errors = badRequestException.Errors;
                    break;

                case NotFoundException _:
                    statusCode = HttpStatusCode.NotFound; // 404
                    message = "Recurso não encontrado.";
                    break;

                case InvalidOperationException _:
                    statusCode = HttpStatusCode.Conflict; // 409
                    message = "Ocorreu um problema de concorrência ao acessar o banco de dados. Tente novamente.";
                    break;
            }

            // Configurar a resposta
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = (int)statusCode,
                Message = message,
                Errors = errors
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}