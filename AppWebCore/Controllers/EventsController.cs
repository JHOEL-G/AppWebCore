using AppWebCore.Data;
using AppWebCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppWebCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var query = _context.Events.AsQueryable();

            if (start.HasValue && end.HasValue)
            {
                query = query.Where(e => e.Start < end && e.End > start);
            }

            var result = await query.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(string id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }
            return Ok(ev);
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event newEvent)
        {
            if (string.IsNullOrEmpty(newEvent.Id))
            {
                newEvent.Id = Guid.NewGuid().ToString();
            }

            if (newEvent.Start >= newEvent.End)
            {
                return BadRequest("La fecha de inicio debe ser menor que la fecha de fin.");
            }

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, newEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(string id, Event updatedEvent)
        {
            if (id != updatedEvent.Id)
            {
                return BadRequest("El ID del evento no coincide con el ID proporcionado en la URL.");
            }

            if (updatedEvent.Start >= updatedEvent.End)
            {
                return BadRequest("La fecha de inicio debe ser menor que la fecha de fin.");
            }

            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            existingEvent.Title = updatedEvent.Title;
            existingEvent.Start = updatedEvent.Start;
            existingEvent.End = updatedEvent.End;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.Location = updatedEvent.Location;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
