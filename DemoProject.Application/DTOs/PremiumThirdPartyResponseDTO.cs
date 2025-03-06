using DemoProject.Application.Mediator;
using System.Text.Json.Serialization;

namespace DemoProject.Application.DTOs
{
    public class PremiumThirdPartyResponse: IApiDataResponse
    {
        [JsonPropertyName("results")]
        public IEnumerable<PremiumThirdPartyResponseDTO> Results { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        // those don't go to the client...
        public bool HasResults() => Results != null && Results.Any();
        IEnumerable<object> IApiDataResponse.GetResults() => Results.Cast<object>();
    }

    public class PremiumThirdPartyResponseDTO
    {
        [JsonPropertyName("companyIdentificationNumber")]
        public string CompanyIdentificationNumber { get; set; }

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }

        [JsonPropertyName("registrationDate")]
        public string RegistrationDate { get; set; }

        [JsonPropertyName("companyFullAddress")]
        public string CompanyFullAddress { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }
}
