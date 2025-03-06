namespace DemoProject.Application.Mediator
{
    public class ServiceUnavailableErrorResponse: ErrorResponse
    {
        public ServiceUnavailableErrorResponse(string message)
        {
            ErrorCode = ErrorCodes.UnhandledError.ToString();
            Errors = Errors = new Error[] { new(message) };
        }
    }
}
