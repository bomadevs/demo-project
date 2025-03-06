using DemoProject.Application.Requests;
using FluentValidation;

namespace DemoProject.Application.Validators
{
    /// <summary>
    /// Adds validation rules for the VerificationRequest.
    /// </summary>
    public class VerificationRequestValidator: AbstractValidator<VerificationRequest>
    {
        public VerificationRequestValidator()
        {
            RuleFor(x => x.VerificationId)
                .NotEmpty().WithMessage("VerificationId is required.");
        }
    }
}
