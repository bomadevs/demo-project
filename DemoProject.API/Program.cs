using DemoProject.Application.DTOs;
using DemoProject.Application.Handlers;
using DemoProject.Infrastructure.MockDB;
using DemoProject.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using DemoProject.Application.Validators;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using DemoProject.API.SwaggerExamples;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// setup Swagger...
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations(); 
    options.ExampleFilters();

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Demo API",
        Version = "v1",
        Description = "A sample API demonstrating Swagger examples"
    });
});

// register swagger example providers...
builder.Services.AddSwaggerExamplesFromAssemblyOf<BackendServiceRequestExample>();

// add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(FreeThirdPartyHandler).Assembly));

// register in memory database for mocking a real database...
builder.Services.AddDbContext<AppDbContext>(options =>options.UseInMemoryDatabase("MockDatabase"));

// registed in memory cache to simulate read distributed cache...
builder.Services.AddMemoryCache();

// register Company Service using Dependency Injection
builder.Services.AddScoped<IDataService, DataService>();

// register AutoMapper. instead of using manual mapping, we can use AutoMapper to map objects...
builder.Services.AddAutoMapper(typeof(MappingProfile));

// register Failure Simulation Service...
builder.Services.AddSingleton<IFailureSimulationService, FailureSimulationService>();

// register input validators for endpoints...
builder.Services.AddValidatorsFromAssemblyContaining<BackendServiceRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// seed mock database with mock data on startup...
using (var scope = app.Services.CreateScope())
{
    var companyService = scope.ServiceProvider.GetRequiredService<IDataService>();
    await companyService.SeedDataAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
