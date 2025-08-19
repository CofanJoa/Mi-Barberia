using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiBarberiaApp.Models;

namespace MiBarberiaApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Barbero> Barberos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Turno> Turnos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales
            modelBuilder.Entity<Turno>()
                .HasIndex(t => new { t.BarberoId, t.Fecha, t.HoraInicio })
                .IsUnique()
                .HasFilter("[Estado] IN ('Pendiente', 'Confirmado')");
        }
    }
}