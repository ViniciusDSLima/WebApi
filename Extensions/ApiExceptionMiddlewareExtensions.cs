using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApplication1.Extensions;

public static class ApiExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async Context =>
            {
                Context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Context.Response.ContentType = "application/json";

                var contextFeature = Context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    await Context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = Context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                        Trace = contextFeature.Error.StackTrace
                    }.ToString());
                }
            });
        });
    }
}