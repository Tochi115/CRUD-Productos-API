using System.ComponentModel.DataAnnotations;

namespace ProductosApi.Models;

/// <summary>
/// Entidad que representa un producto almacenado en la tabla dbo.Productos.
/// </summary>
public class Producto
{
    /// <summary>Identificador (IDENTITY). Se ignora al insertar.</summary>
    public int Id { get; set; }

    /// <summary>Nombre del producto.</summary>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción opcional.</summary>
    [StringLength(250, ErrorMessage = "La descripción no puede exceder 250 caracteres.")]
    public string? Descripcion { get; set; }

    /// <summary>Precio unitario (mayor o igual a 0).</summary>
    [Range(typeof(decimal), "0", "99999999", ErrorMessage = "El precio debe ser mayor o igual a 0.")]
    public decimal Precio { get; set; }

    /// <summary>Unidades disponibles (mayor o igual a 0).</summary>
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }

    /// <summary>Categoría opcional (ej. Electrónica, Papelería).</summary>
    [StringLength(50, ErrorMessage = "La categoría no puede exceder 50 caracteres.")]
    public string? Categoria { get; set; }

    /// <summary>Fecha de alta. La asigna la base de datos.</summary>
    public DateTime FechaRegistro { get; set; }
}
