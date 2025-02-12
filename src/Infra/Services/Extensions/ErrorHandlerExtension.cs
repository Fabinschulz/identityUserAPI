using IdentityUser.src.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace IdentityUser.src.Infra.Services.Extensions
{

    /// <summary>
    /// Provides extension methods for handling errors in the application.
    /// </summary>
    public static class ErrorHandlerExtension
    {
        /// <summary>
        /// Configures the application to use a custom error handler.
        /// </summary>
        /// <param name="app">The application builder.</param>

        [Obsolete("Use UseMiddleware<ExceptionHandlingMiddleware> instead.")]
        public static void UseErrorHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(HandleExceptions);
            });
        }
        private static async Task HandleExceptions(HttpContext context)
        {
            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionHandlerFeature?.Error;

            if (exception == null)
                return;

            context.Response?.Headers.Append("Access-Control-Allow-Origin", "*");
            context.Response!.ContentType = "application/json";

            if (exception is BadRequestException badRequestException)
            {
                await HandleBadRequest(context, badRequestException);
            }
            else if (exception is NotFoundException notFoundException)
            {
                await HandleNotFound(context, notFoundException);
            }
            else if (exception is InvalidOperationException invalidOperationException)
            {
                await HandleDbContextConcurrencyError(context, invalidOperationException);
            }
            else
            {
                await HandleInternalError(context, exception);
            }
        }

        private static async Task HandleDbContextConcurrencyError(HttpContext context, InvalidOperationException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;

            var errorResponse = new
            {
                Message = "Ocorreu um problema de concorrência ao acessar o banco de dados. Tente novamente.",
                context.Response.StatusCode
            };

            await WriteJsonResponse(context, errorResponse);
        }


        private static async Task HandleBadRequest(HttpContext context, BadRequestException badRequestException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Errors = badRequestException.Errors.ToArray()
            };

            await WriteJsonResponse(context, errorResponse);
        }

        private static async Task HandleNotFound(HttpContext context, NotFoundException notFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Errors = notFoundException.Message
            };

            await WriteJsonResponse(context, errorResponse);
        }

        private static async Task HandleInternalError(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            object errorResponse;

            if (exception is JsonException jsonException)
            {
                errorResponse = new
                {
                    Message = "Ocorreu um erro ao processar os dados fornecidos.Verifique os valores enviados.",
                    Details = jsonException.Message,
                    StatusCode = context.Response.StatusCode
                };
            }
            else
            {
                errorResponse = new
                {
                    Message = "Ocorreu um erro inesperado no servidor.Tente novamente mais tarde.",
                    Details = exception.Message,
                    StatusCode = context.Response.StatusCode
                };
            }

            await WriteJsonResponse(context, errorResponse);
        }

        private static async Task WriteJsonResponse(HttpContext context, object responseObject)
        {
            var jsonResponse = JsonSerializer.Serialize(responseObject);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
