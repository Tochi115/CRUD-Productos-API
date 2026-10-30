using System.Reflection;
using Microsoft.OpenApi.Models;
using ProductosApi.Data;
using ProductosApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controladores
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Productos",
        Version = "v1",
        Description = "CRUD de productos con ASP.NET Core, ADO.NET y SQL Server."
    });

    // Muestra en Swagger los comentarios /// de controladores y modelos
    var xml = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    if (File.Exists(xml))
        c.IncludeXmlComments(xml);
});

// Conexión y repositorio (inyección de dependencias)
builder.Services.AddSingleton<ConexionDb>();
builder.Services.AddScoped<ProductoRepository>();
builder.Services.AddScoped<IProductoRepository>(sp => sp.GetRequiredService<ProductoRepository>());
builder.Services.AddScoped<IProductoBusqueda>(sp => sp.GetRequiredService<ProductoRepository>());

// CORS abierto para poder consumir la API desde un front (React Native, etc.)
builder.Services.AddCors(o => o.AddPolicy("PermitirTodo", p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Swagger habilitado siempre para facilitar las pruebas de la actividad
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Productos v1");
    c.DocumentTitle = "API de Productos";
});

app.UseCors("PermitirTodo");
app.MapControllers();

// Abrir la raíz redirige a Swagger
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.Run();
