using System;
namespace DemoProject.Infrastructure.Services
{
    /// <inheritdoc />
    public class FailureSimulationService : IFailureSimulationService
    {
        private readonly Random _random = new();

        /// <inheritdoc />
        public bool ShouldFail(int failurePercentage)
        {
            return _random.Next(1, 101) <= failurePercentage;
        }
    }
}
