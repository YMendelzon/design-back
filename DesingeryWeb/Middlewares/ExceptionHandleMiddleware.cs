using Microsoft.AspNetCore.Http;

namespace DesingeryWeb.Middlewares
{
    public class ExceptionHandleMiddleware
    {
        // הגדרת שדה פרטי לשמירת הפונקציה הבאה בצינור הבקשות
        private readonly RequestDelegate _next;

        // בנאי של המחלקה שמקבל פונקציית RequestDelegate
        public ExceptionHandleMiddleware(RequestDelegate next)
        {
            _next = next;// שמירת הערך
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(ex, context); // קריאה לפונקציה לטיפול ב-exception
            }
        }

        private async Task HandleException(Exception ex, HttpContext context) // פונקציה לטיפול ב-exception
        {
            if (ex is InvalidOperationException)
            {
                context.Response.StatusCode = 400; // קביעת קוד תגובה ל-400 (Bad Request)
                await context.Response.WriteAsJsonAsync("Bad request"); // שליחת הודעת שגיאה "Bad request" בתגובה

            }
            else if (ex is UnauthorizedAccessException) // אם ה-exception הוא UnauthorizedAccessException
            {
                context.Response.StatusCode = 401; // קביעת קוד תגובה ל-401 (Unauthorized)
                await context.Response.WriteAsync("Unauthorized access"); // שליחת הודעת שגיאה "Unauthorized access" בתגובה
            }
            else if (ex is KeyNotFoundException) // אם ה-exception הוא KeyNotFoundException
            {
                context.Response.StatusCode = 404; // קביעת קוד תגובה ל-404 (Not Found)
                await context.Response.WriteAsync("Resource not found"); // שליחת הודעת שגיאה "Resource not found" בתגובה
            }
            else // לכל שאר סוגי ה-exception
            {
                context.Response.StatusCode = 500; // קביעת קוד תגובה ל-500 (Internal Server Error)
                await context.Response.WriteAsync("Internal server error"); // שליחת הודעת שגיאה "Internal server error" בתגובה
            }

        }
       
    }

    public static class ExceptionHandleMiddlewareExtensions
    {

        public static IApplicationBuilder UseExceptionHandleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandleMiddleware>();
        }

    }
}
