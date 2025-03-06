using DemoProject.Application.Mediator;
using MediatR;
using System.Text.Json.Serialization;

namespace DemoProject.Application.Requests
{
    /// <summary>
    /// Request to the backend service used in medaitor pattern, will be sent to the proper handler.
    /// </summary>
    public class BackendServiceRequest : IRequest<ApiResponseResult>
    {
        [JsonPropertyName("verificationId")]
        public Guid VerificationId { get; set; }
        public string Query { get; set; }

        public BackendServiceRequest() { }

        public BackendServiceRequest(Guid verificationId, string query)
        {
            this.VerificationId = verificationId;
            this.Query = query;
        }
    }
}
