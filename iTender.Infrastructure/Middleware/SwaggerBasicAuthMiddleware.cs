using Microsoft.AspNetCore.Http;

namespace iTender.Infrastructure.Middleware
{
    public class SwaggerBasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public SwaggerBasicAuthMiddleware(RequestDelegate next) => _next = next;

        //For basic login
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    var header = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(authHeader);
                    var credentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(header.Parameter)).Split(':');
                    var user = credentials[0];
                    var password = credentials[1];
                    if (user == "cidbuser" && password == "Password1")
                    {
                        await _next(context);
                        return;
                    }
                } 

                context.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Response.StatusCode = 401;
            }
            else
            {
                await _next(context);
            }
        }
    }
}
