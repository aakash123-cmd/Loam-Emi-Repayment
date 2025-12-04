using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Text.Json;


namespace Loan___Emi_Repayment.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Exception Occurred. RequestId={RequestId}, UserId={UserId}, CorrelationId={CorrelationId}, Path={Path}, Timestamp={Timestamp}",
                 context.TraceIdentifier,        //TraceIdentifier = ASP.NET Core ka automatic request tracking number.
                 context.User?.Identity?.Name ?? "Anonymous",     // If user is not null give the identity and also the name ------ If user is not logged in give anonymlus
                 context.Request.Headers["Correlation-Id"].FirstOrDefault() ?? "None",// “Correlation ID ek unique ID hota hai jisse ek request ka poora journey track hota hai.”
                 context.Request.Path, //Yeh tumhare API ka URL path deta hai.
                 DateTime.UtcNow );   //Yeh tumhe current time deta hai but UTC format me. UTC = Universal Time Coordinated (Global standard time jo har jagah same hota hai)

                await HandleGlobalExceptionAsync(context, ex);
            }
        }

        private async Task HandleGlobalExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message,
                Detailed = _env.IsDevelopment() ? ex.StackTrace : "Internal Server Error"

            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}



