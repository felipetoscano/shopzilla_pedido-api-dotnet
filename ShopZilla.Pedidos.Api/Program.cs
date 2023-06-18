using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopZilla.Pedidos.Api;
using ShopZilla.Pedidos.Api.Dal;
using ShopZilla.Pedidos.Api.Models;
using ShopZilla.Pedidos.Api.Services;
using ShopZilla.Pedidos.Api.Services.BackgroundServices;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

AdicionarSwaggerConfigurado();
AdicionarControllersConfigurado();

var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero //Define o tempo de delay para cálculo de expiração como 0
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

AdicionarInjecaoDeDependencias();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

void AdicionarSwaggerConfigurado()
{
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "API Pedidos ShopZilla",
            Description = "Responsável por gerenciar os pedidos do Shopzilla",
            Contact = new OpenApiContact
            {
                Name = "Felipe Toscano",
                Email = "felipetoscano02@gmail.com"
            }
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description =
            "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
            "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +
            "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                Array.Empty<string>()
            }
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });
}

void AdicionarControllersConfigurado()
{
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
}

void AdicionarInjecaoDeDependencias()
{
    builder.Services.AddDbContext<PedidosDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PedidosDb")));
    builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>());
    builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(KafkaSettings)).Get<KafkaSettings>());
    builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>());
    builder.Services.AddSingleton<TokenService>();
    builder.Services.AddScoped<PedidosDal>();
    builder.Services.AddScoped<UsuariosDal>();
    builder.Services.AddScoped<KafkaProducerService>();
    builder.Services.AddHostedService<KafkaConsumerService>();
}