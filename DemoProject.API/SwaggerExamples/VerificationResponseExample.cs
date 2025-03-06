using DemoProject.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace DemoProject.API.SwaggerExamples
{
    public class VerificationResponseExample : IExamplesProvider<VerificationResponseDTO>
    {
        public VerificationResponseDTO GetExamples()
        {
            return new VerificationResponseDTO
            {
                VerificationId = Guid.NewGuid(),
                QueryText = "Company",
                Timestamp = DateTime.UtcNow,
                Result = "string",
                Source = "FREE"
            };
        }
    }
}
