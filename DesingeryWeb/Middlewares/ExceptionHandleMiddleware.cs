using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

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
                var userEmail = context.User?.FindFirst(ClaimTypes.Email)?.Value;

                // יצירת אובייקט של השגיאה עם המידע שאתה רוצה לשמור
                var errorLog = new // יצירת הירשום של השגיאה והאימייל של המשתמש
                {
                    Timestamp = DateTime.UtcNow, // קביעת השעה על פי התקנים של שעון
                    Message = ex.Message, // הודעה של השגיאה שנפלה
                    StackTrace = ex.StackTrace, // הקוד המרכיב של השגיאה שנפלה
                    UserEmail = userEmail // אימייל של המשתמש שנפל
                };

                // המרת האובייקט למחרוזת JSON
                var errorLogJson = JsonConvert.SerializeObject(errorLog);

                // כתוב את השגיאה ללוג
                Log.Error(errorLogJson+"\n new:  ");
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
