# API CRUD de Productos — ASP.NET Core + SQL Server

**Alumno:** José Alejandro Ruiz Muro · **Matrícula:** UP230742 · **Grupo:** ISC09A
Universidad Politécnica de Aguascalientes

Backend que administra productos con operaciones CRUD sobre SQL Server usando **ADO.NET** (`Microsoft.Data.SqlClient`), implementando la interface `IProductoRepository`. Las pruebas se hacen desde **Swagger**.

## Tecnologías

- .NET 8 (ASP.NET Core Web API)
- SQL Server
- ADO.NET (`SqlConnection`, `SqlCommand`, `SqlDataReader`) con consultas parametrizadas
- Swagger (Swashbuckle)

## Estructura

```
CRUD-Productos-API/
├── Database/
│   └── script.sql                  # Crea la BD, la tabla y datos de prueba
├── ProductosApi/
│   ├── Controllers/
│   │   ├── ProductosController.cs  # Endpoints CRUD + buscar
│   │   └── ConexionController.cs   # Prueba de conexión
│   ├── Data/
│   │   └── ConexionDb.cs           # Conexión a SQL Server
│   ├── Models/
│   │   └── Producto.cs             # Entidad
│   ├── Repositories/
│   │   ├── IProductoRepository.cs  # Interface de la actividad
│   │   ├── IProductoBusqueda.cs    # Interface extra para "Buscar"
│   │   └── ProductoRepository.cs   # Implementación con ADO.NET
│   ├── Program.cs                  # DI, CORS y Swagger
│   └── appsettings.json            # Cadena de conexión
├── docker-compose.yml              # (Opcional) SQL Server en Docker
└── ProductosApi.sln
```

## Puntos resueltos

| Punto | Dónde | Método |
|---|---|---|
| Conexión | `Data/ConexionDb.cs` | `ObtenerConexion()` + `GET /api/conexion/probar` |
| Consultas | `ProductoRepository` | `ObtenerTodos()`, `ObtenerPorId(id)` |
| Insert | `ProductoRepository` | `Insertar(producto)` → devuelve el Id generado |
| Actualizar | `ProductoRepository` | `Actualizar(producto)` → `true` si modificó |
| Eliminar | `ProductoRepository` | `Eliminar(id)` → `true` si eliminó |
| Buscar | `ProductoRepository` | `Buscar(termino)` por nombre o categoría |

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/conexion/probar` | Verifica la conexión a SQL Server |
| GET | `/api/productos` | Lista todos los productos |
| GET | `/api/productos/{id}` | Obtiene un producto por Id |
| GET | `/api/productos/buscar?termino=mouse` | Busca por nombre o categoría |
| POST | `/api/productos` | Inserta un producto |
| PUT | `/api/productos/{id}` | Actualiza un producto |
| DELETE | `/api/productos/{id}` | Elimina un producto |

Ejemplo de cuerpo para POST / PUT:

```json
{
  "nombre": "Audífonos Bluetooth",
  "descripcion": "Con cancelación de ruido",
  "precio": 599.00,
  "stock": 20,
  "categoria": "Electrónica"
}
```

## Cómo ejecutarlo

1. **Crear la base de datos:** abre `Database/script.sql` en SQL Server Management Studio y ejecútalo completo.
2. **Revisar la cadena de conexión** en `ProductosApi/appsettings.json`:
   - SQL Server normal: `Server=localhost;...` (como viene)
   - SQL Server Express: cambia a `Server=localhost\\SQLEXPRESS;...` (en JSON la diagonal invertida va doble)
   - Docker (`docker compose up -d`): `Server=localhost,1433;Database=ProductosDB;User Id=sa;Password=Productos#2026;TrustServerCertificate=True;`
3. **Ejecutar la API:**
   ```bash
   cd ProductosApi
   dotnet run
   ```
   o abre `ProductosApi.sln` en Visual Studio y presiona F5.
4. Abre **http://localhost:5080/swagger** y prueba primero `GET /api/conexion/probar`.

> Si solo tienes instalado .NET 9, cambia `<TargetFramework>net8.0</TargetFramework>` por `net9.0` en `ProductosApi.csproj`.

## Evidencias

_(Agregar aquí capturas de Swagger: conexión, consulta, insert, actualizar, eliminar y buscar.)_
