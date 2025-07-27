using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppWebCore.Models
{
    public class Event
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("start")]
        public DateTime Start { get; set; }

        [JsonPropertyName("end")]
        public DateTime End { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

    }
}
