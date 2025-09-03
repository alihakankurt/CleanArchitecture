using System.Text;
using Application;
using Core.Infrastructure.Services;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.Configure<JwtTokenService.JwtTokenOptions>(builder.Configuration.GetSection(nameof(JwtTokenService.JwtTokenOptions)));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer((options) =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtTokenOptions:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtTokenOptions:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtTokenOptions:SecretKey"]!)),
            ValidateLifetime = true,
            LifetimeValidator = (notBefore, expires, _, _) => notBefore <= DateTime.UtcNow && DateTime.UtcNow < expires,
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(
    (options) => options.AddDefaultPolicy(
        (policy) => policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
            .AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler(_ => { });
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
