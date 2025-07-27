using AppWebCore.Data;
using AppWebCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppWebCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contacto>>> GetContactos()
        {
            return await _context.Contactos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contacto>> GetContacto(int id)
        {
            var contacto = await _context.Contactos.FindAsync(id);
            if (contacto == null)
            {
                return NotFound();
            }
            return contacto;
        }

        [HttpPost]
        public async Task<ActionResult<Contacto>> PostContacto (Contacto contacto)
        {
            if (string.IsNullOrWhiteSpace(contacto.Nombre) || 
                string.IsNullOrWhiteSpace(contacto.Apellido) || 
                string.IsNullOrWhiteSpace(contacto.Correo))
            {
                return BadRequest("Nombre, Apellido y Correo son obligatorios.");
            }

            try
            {
                _context.Contactos.Add(contacto);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetContacto), new { id = contacto.Id }, contacto);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Error en la base de datos: " + ex.Message);
                return StatusCode(500, "Error al guardar el contacto en la base de datos.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error general: " + ex.Message);
                return StatusCode(500, "Error inesperado al guardar el contacto.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContacto(int id)
        {
            var contacto = await _context.Contactos.FindAsync(id);

            if (contacto == null) { return NotFound(); }

            _context.Contactos.Remove(contacto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
