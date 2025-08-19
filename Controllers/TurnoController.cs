using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiBarberiaApp.Data;
using MiBarberiaApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MiBarberiaApp.Controllers
{
    [Authorize]
    public class TurnoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TurnoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Turno
        public async Task<IActionResult> Index()
        {
            var turnos = await _context.Turnos
                .Include(t => t.Barbero)
                .Include(t => t.Cliente)
                .Include(t => t.Servicio)
                .ToListAsync();

            return View(turnos);
        }

        // GET: Turno/Reservar
        public async Task<IActionResult> Reservar()
        {
            ViewBag.Servicios = new SelectList(await _context.Servicios.ToListAsync(), "Id", "Nombre");
            ViewBag.Barberos = new SelectList(await _context.Barberos.Where(b => b.Activo).ToListAsync(), "Id", "Nombre");
            return View();
        }

        // GET: Turno/GetServiciosDuracion
        [HttpGet]
        public IActionResult GetServiciosDuracion()
        {
            var servicios = _context.Servicios
                .Select(s => new { s.Id, s.Duracion })
                .ToList();

            return Json(servicios.ToDictionary(s => s.Id, s => s.Duracion));
        }

        // GET: Turno/GetHorariosDisponibles
        [HttpGet]
        public async Task<IActionResult> GetHorariosDisponibles(int barberoId, string fecha, int servicioId)
        {
            var fechaObj = DateTime.Parse(fecha);
            var servicio = await _context.Servicios.FindAsync(servicioId);

            if (servicio == null)
                return BadRequest("Servicio no válido");

            // Obtener turnos existentes
            var turnosOcupados = await _context.Turnos
                .Where(t => t.BarberoId == barberoId &&
                           t.Fecha.Date == fechaObj.Date &&
                           t.Estado != "Cancelado")
                .OrderBy(t => t.HoraInicio)
                .ToListAsync();

            // Generar horarios disponibles
            var horariosDisponibles = new List<dynamic>();
            TimeSpan horaInicio = TimeSpan.FromHours(14); // 14:00
            TimeSpan horaFinMax = TimeSpan.FromHours(18).Subtract(TimeSpan.FromMinutes(servicio.Duracion));

            TimeSpan intervalo = TimeSpan.FromMinutes(15); // Intervalo entre turnos

            while (horaInicio <= horaFinMax)
            {
                TimeSpan horaFinal = horaInicio.Add(TimeSpan.FromMinutes(servicio.Duracion));
                bool disponible = true;

                // Verificar solapamiento
                foreach (var turno in turnosOcupados)
                {
                    if ((horaInicio >= turno.HoraInicio && horaInicio < turno.HoraFin) ||
                        (horaFinal > turno.HoraInicio && horaFinal <= turno.HoraFin) ||
                        (horaInicio <= turno.HoraInicio && horaFinal >= turno.HoraFin))
                    {
                        disponible = false;
                        break;
                    }
                }

                if (disponible)
                {
                    horariosDisponibles.Add(new
                    {
                        horaValue = horaInicio.ToString(),
                        horaDisplay = horaInicio.ToString(@"hh\:mm") + " - " + horaFinal.ToString(@"hh\:mm")
                    });
                }

                horaInicio = horaInicio.Add(intervalo);
            }

            return Json(horariosDisponibles);
        }

        // POST: Turno/Reservar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservar([Bind("ServicioId,BarberoId,HoraInicio")] Turno turno, string Fecha)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el cliente actual (usuario autenticado)
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == User.Identity.Name);

                    if (cliente == null)
                    {
                        // Si el cliente no existe en nuestra tabla, crearlo
                        cliente = new Cliente
                        {
                            Nombre = User.Identity.Name,
                            Email = User.Identity.Name,
                            Telefono = "Sin especificar"
                        };
                        _context.Add(cliente);
                        await _context.SaveChangesAsync();
                    }

                    // Configurar el turno
                    turno.Fecha = DateTime.Parse(Fecha);
                    turno.ClienteId = cliente.Id;
                    turno.Estado = "Confirmado";
                    turno.FechaCreacion = DateTime.Now;

                    // Calcular hora de fin
                    var servicio = await _context.Servicios.FindAsync(turno.ServicioId);
                    turno.HoraFin = turno.HoraInicio.Add(TimeSpan.FromMinutes(servicio.Duracion));

                    _context.Add(turno);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Confirmacion", new { id = turno.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al reservar el turno: " + ex.Message);
                }
            }

            // Si hay errores, recargar los datos necesarios
            ViewBag.Servicios = new SelectList(_context.Servicios, "Id", "Nombre", turno.ServicioId);
            ViewBag.Barberos = new SelectList(_context.Barberos.Where(b => b.Activo), "Id", "Nombre", turno.BarberoId);
            return View(turno);
        }

        // GET: Turno/Confirmacion/5
        public async Task<IActionResult> Confirmacion(int? id)
        {
            if (id == null || _context.Turnos == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Barbero)
                .Include(t => t.Cliente)
                .Include(t => t.Servicio)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // GET: Turno/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Turnos == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Barbero)
                .Include(t => t.Cliente)
                .Include(t => t.Servicio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // GET: Turno/Create
        public IActionResult Create()
        {
            ViewData["BarberoId"] = new SelectList(_context.Barberos, "Id", "Nombre");
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre");
            ViewData["ServicioId"] = new SelectList(_context.Servicios, "Id", "Nombre");
            return View();
        }

        // POST: Turno/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,HoraInicio,HoraFin,ClienteId,ServicioId,BarberoId,Estado,Notas,FechaCreacion")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BarberoId"] = new SelectList(_context.Barberos, "Id", "Nombre", turno.BarberoId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre", turno.ClienteId);
            ViewData["ServicioId"] = new SelectList(_context.Servicios, "Id", "Nombre", turno.ServicioId);
            return View(turno);
        }

        // GET: Turno/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Turnos == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }
            ViewData["BarberoId"] = new SelectList(_context.Barberos, "Id", "Nombre", turno.BarberoId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre", turno.ClienteId);
            ViewData["ServicioId"] = new SelectList(_context.Servicios, "Id", "Nombre", turno.ServicioId);
            return View(turno);
        }

        // POST: Turno/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,HoraInicio,HoraFin,ClienteId,ServicioId,BarberoId,Estado,Notas,FechaCreacion")] Turno turno)
        {
            if (id != turno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurnoExists(turno.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BarberoId"] = new SelectList(_context.Barberos, "Id", "Nombre", turno.BarberoId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Nombre", turno.ClienteId);
            ViewData["ServicioId"] = new SelectList(_context.Servicios, "Id", "Nombre", turno.ServicioId);
            return View(turno);
        }

        // GET: Turno/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Turnos == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Barbero)
                .Include(t => t.Cliente)
                .Include(t => t.Servicio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // POST: Turno/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Turnos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Turnos'  is null.");
            }
            var turno = await _context.Turnos.FindAsync(id);
            if (turno != null)
            {
                _context.Turnos.Remove(turno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Administrar()
        {
            var turnos = await _context.Turnos
                .Include(t => t.Cliente)
                .Include(t => t.Servicio)
                .Include(t => t.Barbero)
                .OrderByDescending(t => t.Fecha)
                .ThenBy(t => t.HoraInicio)
                .ToListAsync();

            return View(turnos.GroupBy(t => t.Fecha.Date).OrderBy(g => g.Key));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, string estado)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return NotFound();

            turno.Estado = estado;
            _context.Update(turno);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Administrar));
        }

        private bool TurnoExists(int id)
        {
            return _context.Turnos.Any(e => e.Id == id);
        }
    }
}