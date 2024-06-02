using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProveedorEntity.Models
{
    public partial class Proveedor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Rellene este campo, por favor.")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "Rellene este campo, por favor.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Rellene este campo, por favor.")]
        public string Direccion { get; set; } = null!;
        [Required(ErrorMessage = "Rellene este campo, por favor.")]
        public string? Telefono { get; set; }
        public byte[]? Imagen { get; set; }
    }
}
