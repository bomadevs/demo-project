using DemoProject.Application.Mediator;
using MediatR;

namespace DemoProject.Application.Requests
{
    /// <summary>
    /// Request to get verification data.
    /// </summary>
    public class VerificationRequest: IRequest<ApiResponseResult>
    {
        public Guid VerificationId { get; set; }

        public VerificationRequest() { }

        public VerificationRequest(Guid verificationId)
        {
            this.VerificationId = verificationId;
        }
    }
}
