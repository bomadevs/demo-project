using DemoProject.Application.DTOs;

namespace DemoProject.Application.Mediator
{
    /// <summary>
    /// Interface for API data response.
    /// </summary>
    public interface IApiDataResponse: IApiResponse
    {
        bool HasResults();

        IEnumerable<object> GetResults();
    }
}
