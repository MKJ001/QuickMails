namespace QuickMails.Middleware
{
    using Common.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Model;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public class AppExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public AppExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (AppNotFoundException e)
            {
                await this.GenerateErrorResponse(context, e, 404);
            }
            catch (AppBadRequestException e)
            {
                await this.GenerateErrorResponse(context, e, 400);
            }

        }

        private Task GenerateErrorResponse(HttpContext context, AppBaseException exception, int httpStatusCode)
        {
            var responseBody = new AppErrorResponse
            {
                Description = exception.Message, 
                ErrorCode = exception.Code.ToString()
            };

            var body = JsonConvert.SerializeObject(responseBody);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = httpStatusCode;
            return context.Response.WriteAsync(body);
        }

    }
}
