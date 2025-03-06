using DemoProject.Application.Mediator;
using System.Text.Json.Serialization;

namespace DemoProject.Application.DTOs
{
    public class FreeThirdPartyResponse: IApiDataResponse
    {
        [JsonPropertyName("results")]
        public IEnumerable<FreeThirdPartyResponseDTO> Results { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        // those don't go to the client...
        public bool HasResults() => Results != null && Results.Any();
        IEnumerable<object> IApiDataResponse.GetResults() => Results.Cast<object>();
    }

    public class FreeThirdPartyResponseDTO
    {
        [JsonPropertyName("cin")]
        public string? CIN { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("registration_date")]
        public string? RegistrationDate { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("is_active")]
        public bool? IsActive { get; set; }
    }
}
