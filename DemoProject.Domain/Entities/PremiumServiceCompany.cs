using System.Text.Json.Serialization;

namespace DemoProject.Domain.Entities
{
    public class PremiumServiceCompany
    {
        public int Id { get; set; }

        [JsonPropertyName("companyIdentificationNumber")]
        public string CompanyIdentificationNumber { get; set; } = string.Empty;

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; } = string.Empty;

        [JsonPropertyName("registrationDate")]
        public string? RegistrationDate { get; set; }

        [JsonPropertyName("fullAddress")]
        public string? CompanyFullAddress { get; set; }

        [JsonPropertyName("isActive")]
        public bool? IsActive { get; set; }
    }
}
