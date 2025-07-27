using AppWebCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

namespace AppWebCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppWebCore.Data.AppDbContext _context;

        public ProductsController(AppWebCore.Data.AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppWebCore.Models.Product>>> GetProducts()
        {
            return await _context.Products.Include(p => p.Variantes).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppWebCore.Models.Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Variantes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.ImagenUrl))
                return BadRequest("La imagen del producto es obligatoria.");

            if (product.Variantes == null || !product.Variantes.Any())
                return BadRequest("El producto debe tener al menos una variante.");

            product.FechaCreacion = DateTime.UtcNow;

            foreach (var variante in product.Variantes)
            {
                variante.Id = 0; 
            }

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var createdProduct = await _context.Products
                                                    .Include(p => p.Variantes) 
                                                    .FirstOrDefaultAsync(p => p.Id == product.Id);

                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("❌ Error de base de datos: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                Console.WriteLine("Inner Exception: " + (ex.InnerException?.Message ?? "Ninguna"));
                return StatusCode(500, $"Error de base de datos: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error general: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                Console.WriteLine("Inner Exception: " + (ex.InnerException?.Message ?? "Ninguna"));
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var existingProduct = await _context.Products
                                                .Include(p => p.Variantes)
                                                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            existingProduct.FechaCreacion = DateTime.SpecifyKind(product.FechaCreacion, DateTimeKind.Utc); // Mantener la fecha de creación

            var receivedVariantIds = product.Variantes.Select(v => v.Id).ToList();

            foreach (var existingVariante in existingProduct.Variantes.ToList())
            {
                if (!receivedVariantIds.Contains(existingVariante.Id))
                {
                    _context.Variantes.Remove(existingVariante);
                }
            }

            foreach (var receivedVariante in product.Variantes)
            {
                if (receivedVariante.Id == 0)
                {
                    receivedVariante.ProductId = existingProduct.Id; 
                    _context.Variantes.Add(receivedVariante);
                }
                else 
                {
                    var existingVariante = existingProduct.Variantes.FirstOrDefault(v => v.Id == receivedVariante.Id);
                    if (existingVariante != null)
                    {
                        _context.Entry(existingVariante).CurrentValues.SetValues(receivedVariante);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error al actualizar producto: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                Console.WriteLine("Inner Exception: " + (ex.InnerException?.Message ?? "Ninguna"));
                return StatusCode(500, $"Error interno del servidor al actualizar el producto: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
          
            var product = await _context.Products
                                        .Include(p => p.Variantes)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
