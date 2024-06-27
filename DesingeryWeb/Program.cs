using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using DesingeryWeb.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
///////////////
builder.Services.AddSingleton<IAdminService, AdminService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ICommonQuestionsService, CommonQuestionsService>();
builder.Services.AddSingleton<IReviewService, ReviewService>();



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
app.UseExceptionHandleMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
