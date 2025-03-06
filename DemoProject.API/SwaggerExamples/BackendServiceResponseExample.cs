using DemoProject.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace DemoProject.API.SwaggerExamples
{

    public class BackendServiceResponseExample : IExamplesProvider<BackendServiceResponse>
    {
        public BackendServiceResponse GetExamples()
        {
            return new BackendServiceResponse
            {
                VerificationId = Guid.NewGuid(),
                Query = "Company",
                Result = new
                {
                    cin = "123456",
                    name = "Company",
                    registration_date = "2025-03-05",
                    address = "123 Main St",
                    is_active = true
                },
                OtherResults = new List<object>
            {
                new { cin = "654321", name = "Company", registration_date = "2021-06-15", address = "456 Elm St", is_active = true }
            }
            };
        }
    }
}
