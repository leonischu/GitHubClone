using System.Net;
using System.Text.Json;

namespace GithubClone.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;  // next middleware in pipeline 
        private readonly ILogger<ExceptionMiddleware> _logger; // used to log error
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)  // This runs for every request 
        {
            try
            {
                await _next(context);  // pass request to next middleware/controller 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandle exception occured"); //Saves error in logs 
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //setting http response
                context.Response.ContentType = "application/json";  // response will  be JSON 
                var response = new    //create response object
                {
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));  
                //convert object to JSON and send to client

            }
         


        }

    }
}
