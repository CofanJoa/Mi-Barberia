using System.ComponentModel.DataAnnotations;

namespace MiBarberiaApp.Models
{
    public class Servicio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del servicio es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria.")]
        [Range(15, 120, ErrorMessage = "La duración debe estar entre 15 y 120 minutos.")]
        public int Duracion { get; set; } // en minutos

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, 20000, ErrorMessage = "El precio debe estar entre 0 y 1000.")]
        public decimal Precio { get; set; }

        public string? Descripcion { get; set; }
    }
}