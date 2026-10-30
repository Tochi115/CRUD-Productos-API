using Microsoft.AspNetCore.Mvc;
using ProductosApi.Models;
using ProductosApi.Repositories;

namespace ProductosApi.Controllers;

/// <summary>
/// Endpoints CRUD del módulo de productos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductosController : ControllerBase
{
    private readonly IProductoRepository _repositorio;
    private readonly IProductoBusqueda _busqueda;

    public ProductosController(IProductoRepository repositorio, IProductoBusqueda busqueda)
    {
        _repositorio = repositorio;
        _busqueda = busqueda;
    }

    /// <summary>Consulta todos los productos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<Producto>), StatusCodes.Status200OK)]
    public ActionResult<List<Producto>> ObtenerTodos()
    {
        return Ok(_repositorio.ObtenerTodos());
    }

    /// <summary>Consulta un producto por su Id.</summary>
    /// <param name="id">Id del producto.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Producto> ObtenerPorId(int id)
    {
        var producto = _repositorio.ObtenerPorId(id);
        if (producto is null)
            return NotFound(new { mensaje = $"No existe un producto con Id {id}." });

        return Ok(producto);
    }

    /// <summary>Busca productos por nombre o categoría (coincidencia parcial).</summary>
    /// <param name="termino">Texto a buscar, por ejemplo: "mouse" o "papel".</param>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(List<Producto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<List<Producto>> Buscar([FromQuery] string termino)
    {
        if (string.IsNullOrWhiteSpace(termino))
            return BadRequest(new { mensaje = "Debes indicar un término de búsqueda." });

        return Ok(_busqueda.Buscar(termino));
    }

    /// <summary>Inserta un nuevo producto. El Id y la FechaRegistro los asigna la base de datos.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Producto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Producto> Insertar([FromBody] Producto producto)
    {
        int nuevoId = _repositorio.Insertar(producto);
        var creado = _repositorio.ObtenerPorId(nuevoId);

        return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoId }, creado);
    }

    /// <summary>Actualiza un producto existente.</summary>
    /// <param name="id">Id del producto a modificar.</param>
    /// <param name="producto">Nuevos datos del producto.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Producto> Actualizar(int id, [FromBody] Producto producto)
    {
        producto.Id = id; // el Id de la ruta manda

        if (!_repositorio.Actualizar(producto))
            return NotFound(new { mensaje = $"No existe un producto con Id {id}." });

        return Ok(_repositorio.ObtenerPorId(id));
    }

    /// <summary>Elimina un producto por su Id.</summary>
    /// <param name="id">Id del producto a eliminar.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id)
    {
        if (!_repositorio.Eliminar(id))
            return NotFound(new { mensaje = $"No existe un producto con Id {id}." });

        return Ok(new { mensaje = $"Producto {id} eliminado correctamente." });
    }
}
