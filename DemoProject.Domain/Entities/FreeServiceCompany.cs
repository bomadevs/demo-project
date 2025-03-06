using System.Text.Json.Serialization;

namespace DemoProject.Domain.Entities
{
    public class FreeServiceCompany
    {
        public int Id { get; set; }

        [JsonPropertyName("cin")]
        public string CIN { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("registration_date")]
        public string? RegistrationDate { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("is_active")]
        public bool? IsActive { get; set; }
    }
}
