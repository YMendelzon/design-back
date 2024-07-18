using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using DesingeryWeb.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Serilog.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;



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
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero,


        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true


    };
});

builder.Services.AddAuthorization();


var securityScheme = new OpenApiSecurityScheme
  {
      Name = "JWT Authentication",
      Description = "Enter your JWT token in this field",
      In = ParameterLocation.Header,
      Type = SecuritySchemeType.Http,
      Scheme = "bearer",
      BearerFormat = "JWT"
  };
var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    };

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(securityRequirement);   
 }   
    );
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

app.UseExceptionHandleMiddleware();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
//
//// יצירת תפקידים בתחילת האפליקציה
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//    string[] roleNames = { "Admin", "User" };
//    foreach (var roleName in roleNames)
//    {
//        if (!await roleManager.RoleExistsAsync(roleName))
//        {
//            await roleManager.CreateAsync(new IdentityRole(roleName));
//        }
//    }
//}


app.Run();

Log.CloseAndFlush();
