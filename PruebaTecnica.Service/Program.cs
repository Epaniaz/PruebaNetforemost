using Hangfire;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using PruebaTecnica.DAL;
using PruebaTecnica.DAL.Service;
using PruebaTecnica.DAL.Interface;

using PruebaTecnica.Service.Hubs;
using PruebaTecnica.Service.Service;
using PruebaTecnica.Service.ViewModel;
using PruebaTecnica.Service.Interface;
using PruebaTecnica.Service.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Prueba Service",
        Version = "v1",
        Description = "Servicio de Prueba",
        Contact = new OpenApiContact
        {
            Name = "Prueba Tecnica",
            Email = "pruebatecnica@mail.com"
        },
        License = new OpenApiLicense { Name = "Usuarios autorizado" }
    });
    opt.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese el token de autorización",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            new string[]{ "DemoSwaggerDifferentAuthScheme" }
        }
    });
});

/**/
builder.Services.AddDbContext<PruebaTecnicaDbContext>(
    options => options.UseInMemoryDatabase(databaseName: "PRUEBA")
);

//string cnnBD = ConfigurationExtensions.GetConnectionString(builder.Configuration, "cnnPrueba");
//builder.Services.AddDbContext<PruebaTecnicaDbContext>(
//    opt => opt.UseSqlServer(cnnBD)
//);
builder.Services.Configure<Configuraciones>(builder.Configuration.GetSection("Configuraciones"));

builder.Services.AddScoped<ICorreoService, CorreoService>();
builder.Services.AddScoped<ValidarFormularioActionFilter>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IJWTokenService, JWTokenService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var jwtOptions = builder.Configuration
    .GetSection("Configuraciones")
    .GetSection("JwtOptions")
    .Get<JwtOption>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.KeySecret))
    };
});
builder.Services.AddAuthorization();

/*Configuracion de hangfire*/

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("cnnPrueba"));
});
GlobalConfiguration.Configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("cnnPrueba"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;

    try
    {
        var context = service.GetRequiredService<PruebaTecnicaDbContext>();

        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = service.GetRequiredService<ILogger<Program>>();

        logger.LogError(ex, ex.Message);
    }
}

app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});

{
    app.UseMiddleware<JWTokenMiddleware>();
    app.UseRouting();
    app.UseCors("AllowAll");
    app.MapControllers();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHub<UsuarioHub>("/usuariohub");
    });
    app.UseHangfireDashboard();
    app.UseHangfireServer();
}

app.Run();
