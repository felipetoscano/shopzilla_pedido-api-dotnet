using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShopZilla.Pedidos.Api;
using ShopZilla.Pedidos.Api.Dal;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

AdicionarSwaggerConfigurado();
AdicionarControllersConfigurado();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<PedidosDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PedidosDb")));
builder.Services.AddScoped<PedidosDal>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
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