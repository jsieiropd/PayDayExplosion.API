using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PayDayExplosion.Application;
using PayDayExplosion.Infrastructure;
using PayDayExplosion.Infrastructure.Persistence;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => //Aquí se configuran los parámetros que se usarán para validar el token JWT que llega en cada petición.
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,//Le estás indicando que debe validar el issuer del token (quién emitió el token). Esto ayuda a asegurar de que el token fue emitido por este backend y no por alguien más.
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"], //Aquí se define cuál es el issuer válido. Se saca del archivo appsettings.json, típicamente algo como "https://tuservidor.com".

            ValidateAudience = true,//Indica que debe validarse el audience, es decir, para quién fue emitido el token.
            ValidAudience = builder.Configuration["JwtSettings:Audience"],//Se define el audience válido, también desde configuración. Por ejemplo: "https://tuapi.com".

            ValidateLifetime = true,//validar que el token siga vigente

            IssuerSigningKey = new SymmetricSecurityKey(//Se está cargando la clave secreta que (el servidor) usó para firmar el token. Esa clave es lo que se usa internamente para validar la firma del JWT recibido.
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Token"]!)),//Es decir, el backend reconstruye la firma y verifica que coincida con la del token recibido.

            ValidateIssuerSigningKey = true//Le indicas que debe validar que la firma del token es válida utilizando la clave configurada arriba (IssuerSigningKey).
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"? Error autenticando: {context.Exception.Message}");
                return Task.CompletedTask;
            }
        };

        //la parte de "app.UseAuthentication();" y "app.UseAuthorization();" es lo que activa el middleware para que con cada request
        //revise si el token es válido antes de permitir el acceso
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();

        var seedDb = services.GetRequiredService<SeedDb>();
        await seedDb.SeedAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al aplicar migraciones o sembrar la base de datos.");
        throw;
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
