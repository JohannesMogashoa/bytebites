using ByteBites;
using ByteBites.Application.Common.Interfaces;
using ByteBites.Infrastructure.Data;
using ByteBites.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("ByteBitesDb"));
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpoints(typeof(Program).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();
app.UseHttpsRedirection();
app.Run();