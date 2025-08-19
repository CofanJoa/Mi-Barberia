using System.ComponentModel.DataAnnotations;

namespace MiBarberiaApp.Models
{
    public class Turno
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        [Display(Name = "Hora de Inicio")]
        public TimeSpan HoraInicio { get; set; }

        [Display(Name = "Hora de Fin")]
        public TimeSpan HoraFin { get; set; }

        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [Display(Name = "Servicio")]
        public int ServicioId { get; set; }
        public Servicio? Servicio { get; set; }

        [Display(Name = "Barbero")]
        public int BarberoId { get; set; }
        public Barbero? Barbero { get; set; }

        public string Estado { get; set; } = "Pendiente";
        public string? Notas { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}