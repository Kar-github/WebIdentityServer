using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace WebApiDemo.CustomMidlware
{
    public class CustomMidlwareTest
    {
        private readonly RequestDelegate _requestdelegate;
        public CustomMidlwareTest(RequestDelegate requestdelegate)
        {
            _requestdelegate = requestdelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await  context.Response.WriteAsync("This is custom midlware");
            await _requestdelegate.Invoke(context);
        }

    }
    public static class CustomMidlwareExtenssions
    {
        public static IApplicationBuilder UseCustomMidlware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMidlwareTest>();
        }
    }

}
