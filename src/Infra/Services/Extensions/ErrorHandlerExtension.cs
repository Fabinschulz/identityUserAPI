using IdentityUser.src.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace IdentityUser.src.Infra.Services.Extensions
{
    public static class ErrorHandlerExtension
    {
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

            context.Response?.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response!.ContentType = "application/json";

            if (exception is BadRequestException badRequestException)
            {
                await HandleBadRequest(context, badRequestException);
            }
            else if (exception is NotFoundException notFoundException)
            {
                await HandleNotFound(context, notFoundException);
            }
            else
            {
                await HandleInternalError(context, exception);
            }
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

        private static async Task HandleInternalError(HttpContext context, System.Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new
            {
                Message = exception.Message,
                StatusCode = context.Response.StatusCode
            };

            await WriteJsonResponse(context, errorResponse);
        }

        private static async Task WriteJsonResponse(HttpContext context, object responseObject)
        {
            var jsonResponse = JsonSerializer.Serialize(responseObject);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
