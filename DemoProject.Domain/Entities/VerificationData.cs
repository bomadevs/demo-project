using System.Text.Json.Serialization;

namespace DemoProject.Domain.Entities
{
    public class VerificationData
    {
        public int Id { get; set; }

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
