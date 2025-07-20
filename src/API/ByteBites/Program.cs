using System.Security.Claims;
using ByteBites;
using ByteBites.Application.Common.Interfaces;
using ByteBites.Domain;
using ByteBites.Infrastructure;
using ByteBites.Infrastructure.Data;
using ByteBites.Infrastructure.Interceptors;
using ByteBites.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Add services to the container.
var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://kodestallion.au.auth0.com/";
    options.Audience = "https://bytebites-api.johannesmogashoa.co.za";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
builder.Services.AddSingleton<AuditingInterceptor>();
builder.Services.AddSingleton<SoftDeleteInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    options.UseInMemoryDatabase("ByteBitesDb");
    options.AddInterceptors(
        sp.GetRequiredService<AuditingInterceptor>(),
        sp.GetRequiredService<SoftDeleteInterceptor>());
});

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpoints(typeof(Program).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ByteBites API", Version = "v1" });

    // Add JWT Authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            //context.Database.Migrate(); // Apply pending migrations
            // Seed data might need to be adjusted if it implies a user ID for seeded recipes
            SeedData.Initialize(services);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating or seeding the DB.");
        }
    }
}

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();
app.UseHttpsRedirection();
app.Run();