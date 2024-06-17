using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using RestfullWithAspNet.Business;
using RestfullWithAspNet.Business.Implementations;
using RestfullWithAspNet.Model.Context;
using RestfullWithAspNet.Repository;
using RestfullWithAspNet.Repository.Generic;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers(); // Adiciona serviços MVC ao container (Controllers, Views, TagHelpers, etc.)

var connection = builder.Configuration.GetConnectionString("MySQLConnection"); // Get the connection string from the appsettings.json file
builder.Services.AddDbContext<MySQLContext>(options =>
    options.UseMySql(connection, new MySqlServerVersion(new Version(5, 7, 44)))); // Add services to the container for Entity Framework Core, At version 3.2.0 we don't have to use MySqlServerVersion

builder.Services.AddApiVersioning(); // Add services to the container for API versioning

// Injection of dependencies
builder.Services.AddScoped<IPersonBusiness, PersonBusinessImplementation>(); // Register the Rules of business in the container
builder.Services.AddScoped<IBookBusiness, BookBusinessImplementation>(); // Register the Rules of business in the container
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>)); // Register the Rules of Repository (DataBase, Files, etc) in the container

// more about dependency injection: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0
// Add services to the container for API Explorer (used by Swashbuckle)
builder.Services.AddEndpointsApiExplorer();

// Add services to the container for Swagger
builder.Services.AddSwaggerGen();

// Add services to the container for MVC
builder.Services.AddMvc(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml"); // Add support for XML
    options.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json"); // Add support for JSON
})
.AddXmlSerializerFormatters(); // Add support for XML serialization in the MVC middleware

var app = builder.Build(); // Create the application instance.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Verify if the environment is development
{
    _ = connection ?? throw new ArgumentNullException(nameof(connection), "Connection string cannot be null.");
    MigrateDatabase(connection); // Migrate the database
    app.UseDeveloperExceptionPage(); // Adds a developer exception page to the pipeline
    app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint
    app.UseSwaggerUI(); // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint
}

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS

app.UseAuthorization(); // Enable authorization middleware

app.MapControllers(); // Add the MVC middleware to the pipeline

app.Run(); // Execute the application

void MigrateDatabase(string connection)
{
    try
    {
        using (var evolveConnection = new MySqlConnection(connection))
        {
            var evolve = new Evolve.Evolve(evolveConnection, Log.Information)
            {
                Locations = new List<string> { "db/migrations", "db/dataset" },
                IsEraseDisabled = true
                //ValidateChecksums = false // Disable checksum validation
            };
            evolve.Migrate();
        }
    }
    catch (Exception ex)
    {
        Log.Error("Database migration failed.", ex);
        throw;
    }
}
