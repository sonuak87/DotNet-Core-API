using GAMBULL_GAMC.UTILITY.Logger;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using VeggiFoodAPI.Helpers;
using VeggiFoodAPI.Models.ViewModels;

namespace VeggiFoodAPI.Logger
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly LogHandler _LogHandler;
        public ExceptionHandler(RequestDelegate next,
            IHostEnvironment env)
        {
            _env = env;
            _next = next;
            _LogHandler = new LogHandler();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var routeData = context.Request.RouteValues;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var exceptionDetails = _env.IsDevelopment()
                      ? new ExceptionModel(ex.Message, ex.Data.ToString(), ex.InnerException?.ToString(), ex.StackTrace, routeData["controller"].ToString(), routeData["action"].ToString())
                      : new ExceptionModel(ex.Message, ex.Data.ToString(), ex.InnerException?.ToString(), ex.StackTrace, routeData["controller"].ToString(), routeData["action"].ToString());

                _LogHandler.WriteError(exceptionDetails);


                CustomResponse _customResponse = new CustomResponse();

                _customResponse.GetResponseModel(new string[] { ex.Message }, _env.IsDevelopment() ? ex.StackTrace?.ToString() : null);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy =
                    JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(_customResponse.GetResponseModel(new string[] { ex.Message }, _env.IsDevelopment() ? ex.StackTrace?.ToString() : null), options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
