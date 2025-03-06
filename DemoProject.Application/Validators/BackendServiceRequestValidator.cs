using DemoProject.Application.Requests;
using FluentValidation;

namespace DemoProject.Application.Validators
{
    /// <summary>
    /// Adds validation for backend-service endpoint payload request.
    /// </summary> 
    public class BackendServiceRequestValidator: AbstractValidator<BackendServiceRequest>
    {
        public BackendServiceRequestValidator()
        {
            RuleFor(x => x.VerificationId)
                .NotEmpty().WithMessage("VerificationId is required.");

            RuleFor(x => x.Query)
                .NotEmpty().WithMessage("Query is required.");
        }
    }
}
