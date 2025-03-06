using DemoProject.Application.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace DemoProject.API.SwaggerExamples
{
    public class BackendServiceRequestExample : IExamplesProvider<BackendServiceRequest>
    {
        public BackendServiceRequest GetExamples()
        {
            return new BackendServiceRequest
            {
                VerificationId = Guid.NewGuid(),
                Query = "Company"
            };
        }
    }
}
