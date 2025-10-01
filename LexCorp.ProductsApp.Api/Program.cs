using Asp.Versioning;
using LexCorp.Product.App.Extensions;
using LexCorp.Product.Data.Extensions;
using LexCorp.Products.Auth.App.Extensions;
using LexCorp.Products.Data;
using LexCorp.Products.Data.Seeder.Extensions;
using LexCorp.Products.Data.Seeder.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

//Registering application layers
builder.Services.AddProductsAuth(builder.Configuration);
builder.Services.AddProductsData(builder.Configuration);
builder.Services.AddProductsApp();
builder.Services.AddDataSeeder();

builder.Services.AddControllers();

var apiVersionBuilder = builder.Services.AddApiVersioning(options =>
{
  options.ReportApiVersions = true;
  options.AssumeDefaultVersionWhenUnspecified = true;
  options.DefaultApiVersion = new ApiVersion(1, 0);
});

apiVersionBuilder.AddApiExplorer(options =>
{
  options.GroupNameFormat = "'v'VVV";
  options.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SupportNonNullableReferenceTypes();
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products App", Version = "v1" });

  //https://github.com/domaindrivendev/Swashbuckle.AspNetCore#include-descriptions-from-xml-comments
  c.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, "LexCorp.ProductsApp.Api.xml"));
  var dtoDlls = Directory.GetFiles(AppContext.BaseDirectory, "*.Dto.dll")
                  .Select(x => System.IO.Path.GetFileNameWithoutExtension(x))
                  .Where(x => x.StartsWith("LexCorp."))
                  .ToList();
  foreach (var assembly in dtoDlls)
  {
    var xmlFileName = $"{assembly}.xml";
    if (!File.Exists(System.IO.Path.Combine(AppContext.BaseDirectory, xmlFileName)))
    {
      throw new Exception($"No documentation file found: {xmlFileName}");
    }
    c.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, xmlFileName));
  }

  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    In = ParameterLocation.Header,
    Description = "Please insert JWT with Bearer into field",
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey
  });

  c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
      new string[] { }
    }
  });
});

var app = builder.Build();


using var scope = app.Services.CreateScope();
//Database migrations
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
db.Database.Migrate();
//Seeding initial data if needed.
var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeederService>();
await seeder.SeedDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API v1");
  });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>
/// For integration tests purposes only.
/// </summary>
public partial class Program { }