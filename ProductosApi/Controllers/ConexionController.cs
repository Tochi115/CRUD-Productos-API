using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProductosApi.Data;

namespace ProductosApi.Controllers;

/// <summary>
/// Endpoint para comprobar desde Swagger que la conexión a SQL Server funciona.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ConexionController : ControllerBase
{
    private readonly ConexionDb _conexion;

    public ConexionController(ConexionDb conexion)
    {
        _conexion = conexion;
    }

    /// <summary>Abre una conexión de prueba y devuelve datos del servidor.</summary>
    [HttpGet("probar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Probar()
    {
        try
        {
            using var cn = _conexion.ObtenerConexion();
            cn.Open();

            return Ok(new
            {
                conectado = true,
                servidor = cn.DataSource,
                baseDeDatos = cn.Database,
                versionSqlServer = cn.ServerVersion
            });
        }
        catch (SqlException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                conectado = false,
                error = ex.Message
            });
        }
    }
}
