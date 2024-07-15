
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DesingeryWeb.Middlewares
{

    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the Authorization header is present
            if (context.Request.Headers.ContainsKey("token"))
            {
                var token = context.Request.Headers["token"].ToString().Replace("Bearer ", "");
                // Do something with the token, for example, log it or validate it
                Console.WriteLine($"Token received: {token}");

                // You can add the token to the context items for later use in controllers
                context.Items["token"] = token;
            }

            await _next(context);
        }
    }
}
