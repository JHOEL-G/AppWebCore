using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AppWebCore.Models
{
    public class VarianteProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La talla es obligatoria")]
        public string Talla { get; set; } = string.Empty;

        [Required(ErrorMessage = "El color es obligatorio")]
        public string Color { get; set; } = string.Empty;

        [Range(0.01, 9999.99, ErrorMessage = "El precio debe estar entre 0.01 y 9999.99")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        public int ProductId { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }
    }
}