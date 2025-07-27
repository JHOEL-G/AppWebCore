using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AppWebCore.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        [JsonPropertyName("imagenUrl")]
        public string? ImagenUrl { get; set; }

        public List<VarianteProduct> Variantes { get; set; } = new();

        public DateTime FechaCreacion { get; set; }
    }
}
