namespace DemoProject.Infrastructure.Services
{
    /// <summary>
    /// Service to simulate intermittent failures in API responses...
    /// </summary>
    public interface IFailureSimulationService
    {
        /// <summary>
        /// Determines whether the request should fail with a 503 error.
        /// </summary>
        bool ShouldFail(int failurePercentage);
    }
}
