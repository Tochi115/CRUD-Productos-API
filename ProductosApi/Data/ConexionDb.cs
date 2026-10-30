using Microsoft.Data.SqlClient;

namespace ProductosApi.Data;

/// <summary>
/// Punto 1: CONEXIÓN.
/// Lee la cadena de conexión de appsettings.json y entrega objetos SqlConnection.
/// Cada operación abre y cierra su propia conexión (el pool de ADO.NET las reutiliza).
/// </summary>
public class ConexionDb
{
    private readonly string _cadenaConexion;

    public ConexionDb(IConfiguration configuration)
    {
        _cadenaConexion = configuration.GetConnectionString("ProductosDB")
            ?? throw new InvalidOperationException(
                "No se encontró la cadena de conexión 'ProductosDB' en appsettings.json.");
    }

    /// <summary>Crea una nueva conexión (sin abrir) hacia SQL Server.</summary>
    public SqlConnection ObtenerConexion() => new SqlConnection(_cadenaConexion);
}
