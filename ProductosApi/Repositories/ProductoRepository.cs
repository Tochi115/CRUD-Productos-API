using System.Data;
using Microsoft.Data.SqlClient;
using ProductosApi.Data;
using ProductosApi.Models;

namespace ProductosApi.Repositories;

/// <summary>
/// Implementación de IProductoRepository con ADO.NET (SqlConnection / SqlCommand).
/// Todas las consultas usan parámetros (@...) para evitar inyección SQL.
/// </summary>
public class ProductoRepository : IProductoRepository, IProductoBusqueda
{
    private const string Columnas = "Id, Nombre, Descripcion, Precio, Stock, Categoria, FechaRegistro";

    private readonly ConexionDb _conexion;

    public ProductoRepository(ConexionDb conexion)
    {
        _conexion = conexion;
    }

    // ---------------------------------------------------------------
    // Punto 2: CONSULTAS
    // ---------------------------------------------------------------
    public List<Producto> ObtenerTodos()
    {
        var productos = new List<Producto>();

        using var cn = _conexion.ObtenerConexion();
        using var cmd = new SqlCommand($"SELECT {Columnas} FROM dbo.Productos ORDER BY Id;", cn);

        cn.Open();
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            productos.Add(Mapear(dr));
        }

        return productos;
    }

    public Producto? ObtenerPorId(int id)
    {
        using var cn = _conexion.ObtenerConexion();
        using var cmd = new SqlCommand($"SELECT {Columnas} FROM dbo.Productos WHERE Id = @Id;", cn);
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        cn.Open();
        using var dr = cmd.ExecuteReader();
        return dr.Read() ? Mapear(dr) : null;
    }

    // ---------------------------------------------------------------
    // Punto 3: INSERT
    // Devuelve el Id generado por la base de datos (OUTPUT INSERTED.Id).
    // ---------------------------------------------------------------
    public int Insertar(Producto producto)
    {
        const string sql = @"
            INSERT INTO dbo.Productos (Nombre, Descripcion, Precio, Stock, Categoria)
            OUTPUT INSERTED.Id
            VALUES (@Nombre, @Descripcion, @Precio, @Stock, @Categoria);";

        using var cn = _conexion.ObtenerConexion();
        using var cmd = new SqlCommand(sql, cn);
        AgregarParametros(cmd, producto);

        cn.Open();
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    // ---------------------------------------------------------------
    // Punto 4: ACTUALIZAR
    // Devuelve true si se modificó al menos un registro.
    // ---------------------------------------------------------------
    public bool Actualizar(Producto producto)
    {
        const string sql = @"
            UPDATE dbo.Productos
               SET Nombre      = @Nombre,
                   Descripcion = @Descripcion,
                   Precio      = @Precio,
                   Stock       = @Stock,
                   Categoria   = @Categoria
             WHERE Id = @Id;";

        using var cn = _conexion.ObtenerConexion();
        using var cmd = new SqlCommand(sql, cn);
        AgregarParametros(cmd, producto);
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = producto.Id;

        cn.Open();
        return cmd.ExecuteNonQuery() > 0;
    }

    // ---------------------------------------------------------------
    // Punto 5: ELIMINAR
    // Devuelve true si se eliminó el registro.
    // ---------------------------------------------------------------
    public bool Eliminar(int id)
    {
        using var cn = _conexion.ObtenerConexion();
        using var cmd = new SqlCommand("DELETE FROM dbo.Productos WHERE Id = @Id;", cn);
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

        cn.Open();
        return cmd.ExecuteNonQuery() > 0;
    }

    // ---------------------------------------------------------------
    // Punto 6: BUSCAR (por nombre o categoría, coincidencia parcial)
    // ---------------------------------------------------------------
    public List<Producto> Buscar(string termino)
    {
        var productos = new List<Producto>();

        const string sql = @"
            SELECT Id, Nombre, Descripcion, Precio, Stock, Categoria, FechaRegistro
              FROM dbo.Productos
             WHERE Nombre LIKE @Termino OR Categoria LIKE @Termino
             ORDER BY Nombre;";

        using var cn = _conexion.ObtenerConexion();
        using var cmd = new SqlCommand(sql, cn);
        cmd.Parameters.Add("@Termino", SqlDbType.NVarChar, 102).Value = $"%{termino.Trim()}%";

        cn.Open();
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            productos.Add(Mapear(dr));
        }

        return productos;
    }

    // ---------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------
    private static void AgregarParametros(SqlCommand cmd, Producto p)
    {
        cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = p.Nombre;
        cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 250).Value = (object?)p.Descripcion ?? DBNull.Value;

        var precio = cmd.Parameters.Add("@Precio", SqlDbType.Decimal);
        precio.Precision = 10;
        precio.Scale = 2;
        precio.Value = p.Precio;

        cmd.Parameters.Add("@Stock", SqlDbType.Int).Value = p.Stock;
        cmd.Parameters.Add("@Categoria", SqlDbType.NVarChar, 50).Value = (object?)p.Categoria ?? DBNull.Value;
    }

    private static Producto Mapear(SqlDataReader dr)
    {
        int iDescripcion = dr.GetOrdinal("Descripcion");
        int iCategoria = dr.GetOrdinal("Categoria");

        return new Producto
        {
            Id = dr.GetInt32(dr.GetOrdinal("Id")),
            Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
            Descripcion = dr.IsDBNull(iDescripcion) ? null : dr.GetString(iDescripcion),
            Precio = dr.GetDecimal(dr.GetOrdinal("Precio")),
            Stock = dr.GetInt32(dr.GetOrdinal("Stock")),
            Categoria = dr.IsDBNull(iCategoria) ? null : dr.GetString(iCategoria),
            FechaRegistro = dr.GetDateTime(dr.GetOrdinal("FechaRegistro"))
        };
    }
}
