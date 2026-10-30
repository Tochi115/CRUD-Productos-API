using ProductosApi.Models;

namespace ProductosApi.Repositories;

/// <summary>
/// Interface adicional para el punto "Buscar".
/// Se separa para no modificar la interface IProductoRepository que pide la actividad.
/// </summary>
public interface IProductoBusqueda
{
    /// <summary>Busca productos cuyo nombre o categoría contengan el término.</summary>
    List<Producto> Buscar(string termino);
}
