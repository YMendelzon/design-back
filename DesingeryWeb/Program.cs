using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using DesingeryWeb.Middlewares;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration() // יצירת הגדרת ה-Logger של Serilog
    .MinimumLevel.Error()  // הגדרת רמת המינימום להיות שגיאה ומעלה
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) // הגדרת השימוש ב-File של Serilog לשמירת הלוגים, עם גיליון יומי
    .CreateLogger(); // יצירת ה-Logger בהתאם להגדרות שהוגדרו


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
///////////////
builder.Services.AddSingleton<IAdminService, AdminService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ICommonQuestionsService, CommonQuestionsService>();
builder.Services.AddSingleton<ICategoriesService, CategoriesService>();
//builder.Services.AddSingleton<IReviewService, ReviewService>();
builder.Services.AddSingleton<IProductService, ProductService>();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin",
//        builder =>
//        {
//            builder.WithOrigins("http://localhost:3000") // or the URL of your React app
//                   .AllowAnyHeader()
//                   .AllowAnyMethod();
//        });
//});

/////
//איפשור גישה מהלקוח
builder.Services.AddCors(p => p.AddPolicy("corspolicy", builder =>
{
    builder

    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));
var app = builder.Build();
app.UseCors("corspolicy");
app.UseStaticFiles();


///
///



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


//app.UseMiddleware<ExceptionHandleMiddleware>();

app.UseCors("AllowSpecificOrigin");

app.UseExceptionHandleMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

Log.CloseAndFlush();