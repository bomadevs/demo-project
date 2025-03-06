using DemoProject.Application.Mediator;
using System.Text.Json.Serialization;

namespace DemoProject.Application.DTOs
{
    public class BackendServiceResponse: IApiResponse
    {
        [JsonPropertyName("verificationId")]
        public Guid VerificationId { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("result")]
        public object Result { get; set; }

        [JsonPropertyName("otherResults")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<object>? OtherResults { get; set; }

        [JsonIgnore]
        public string Source { get; set; } = string.Empty;
    }
}
