using Microsoft.AspNetCore.Http;
namespace Lib.Middleware
{
    public class ListenToOnlyAPIGateway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //Extract specific header from the request
            var signedHeader = context.Request.Headers["api-Gateway"];

            //NULL means, the request is not coming from the API gateway
            if(signedHeader.FirstOrDefault() is null)
             {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry service unavailable");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
