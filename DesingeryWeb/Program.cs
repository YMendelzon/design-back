using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using DesingeryWeb.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Serilog.Events;



var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();


// הוסף את השירותים של Authentication ו-JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "yourdomain.com", // הגדר את ה-Issuer שלך
        ValidAudience = "yourdomain.com", // הגדר את ה-Audience שלך
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key")) // הגדר את המפתח הסודי שלך
    };
});


builder.Services.AddAuthorization();





// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAdminService, AdminService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ICommonQuestionsService, CommonQuestionsService>();
builder.Services.AddSingleton<IOrderItemService, OrderItemService>();

builder.Services.AddSingleton<ICategoriesService, CategoriesService>();

//builder.Services.AddSingleton<IReviewService, ReviewService>();
builder.Services.AddSingleton<IReviewService, ReviewService>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IOrderService, OrdersService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddHttpClient<MailchimpService>();


//builder.Services.AddSingleton<IGmailSmtpClientService, GmailSmtpClientService>();


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

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles(); // This line is important

app.UseRouting();
app.UseAuthorization();
app.UseExceptionHandleMiddleware();
//app.UseMiddleware<ExceptionHandleMiddleware>();

//
app.UseAuthentication();
app.UseAuthorization();
//

app.MapControllers();

app.Run();

Log.CloseAndFlush();
