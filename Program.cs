using Microsoft.EntityFrameworkCore;
using ToDoApp_API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program)); // Register AutoMapper with dependency injection
builder.Services.AddControllers(); // Add support for controllers

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // Adds endpoints API explorer for minimal APIs
builder.Services.AddSwaggerGen(); // Register Swagger/OpenAPI services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint
    app.UseSwaggerUI(); // Enable middleware to serve Swagger UI
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS

app.UseRouting(); // Enables routing

app.UseAuthorization(); // Enables authorization middleware

app.MapControllers(); // Maps the controllers to endpoints

app.Run(); // Runs the application