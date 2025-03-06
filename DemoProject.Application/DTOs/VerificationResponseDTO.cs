using DemoProject.Application.Mediator;
using System.Text.Json.Serialization;

namespace DemoProject.Application.DTOs
{
    public class VerificationResponseDTO: IApiResponse
    {
        [JsonPropertyName("verificationId")]
        public Guid VerificationId { get; set; }

        [JsonPropertyName("queryText")]
        public string QueryText { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }
    }
}
