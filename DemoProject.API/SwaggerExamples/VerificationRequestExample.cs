using DemoProject.Application.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace DemoProject.API.SwaggerExamples
{
    public class VerificationRequestExample : IExamplesProvider<VerificationRequest>
    {
        public VerificationRequest GetExamples()
        {
            return new VerificationRequest
            {
                VerificationId = Guid.NewGuid()
            };
        }
    }
}
