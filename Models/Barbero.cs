using System.ComponentModel.DataAnnotations;

namespace MiBarberiaApp.Models
{
    public class Barbero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del barbero es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        public bool Activo { get; set; } = true;
    }
}