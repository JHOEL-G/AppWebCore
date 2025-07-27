using AppWebCore.Data;
using AppWebCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppWebCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VariantesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppWebCore.Models.VarianteProduct>>> GetVariantes()
        {
            return await _context.Variantes
                .Include(v => v.Product)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppWebCore.Models.VarianteProduct>> GetVariante(int id)
        {
            var variante = await _context.Variantes
                .Include(v => v.Product)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (variante == null)
            {
                return NotFound();
            }

            return variante;
        }

        [HttpPost] 
        public async Task<ActionResult<AppWebCore.Models.VarianteProduct>> PostVariante(VarianteProduct variante)
        {
            var producto = await _context.Products.FindAsync(variante.ProductId);
            if (producto == null)
            {
                return BadRequest("el producto asociado no existe");
            }

            _context.Variantes.Add(variante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVariante), new {id = variante.Id}, variante);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutVariante(int id, VarianteProduct variante)
        {
            if (id != variante.Id)
            {
                return BadRequest();
            }

            _context.Entry(variante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Variantes.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVariante (int id)
        {
            var variante = await _context.Variantes.FindAsync(id);
            if (variante == null)
            {
                return NotFound();
            }

            _context.Variantes.Remove(variante);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
