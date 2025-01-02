using Lib.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Lib.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //Declare variable
            string message = "sorry, internal server error occured. Kindly try again";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";

            try
            {
                await next(context);

                //check if exceptionis too many requests
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many request made.";
                    statusCode = StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, title, message, statusCode);
                }

                //if response is UnAuthorized
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "You are not authorized";
                    await ModifyHeader(context, title, message, statusCode);
                }

                //if response is forbidden
                if(context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of Access";
                    message = "You are not allowd to access";
                    statusCode = StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, title, message, statusCode);
                }
            }catch(Exception ex)
            {
                //log original exceptions/filr, debugger,console
                LogException.LogExceptions(ex);

                //check if Exception is timeout
                if(ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of time";
                    message = "Request timeout,try again";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }

                //if none of the exceptions
                await ModifyHeader(context, title, message, statusCode);
            }
          
        }
        private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            //display scary-free message to client
            context.Response.ContentType = "Application.json/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails{
                Detail = message,
                Status = statusCode,
                Title =title
            }), CancellationToken.None);
            return;
        }
    }
}
