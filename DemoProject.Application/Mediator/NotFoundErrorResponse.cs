namespace DemoProject.Application.Mediator
{
    public class NotFoundErrorResponse: ErrorResponse
    {
        public NotFoundErrorResponse(string message)
        {
            ErrorCode = ErrorCodes.NotFound.ToString();
            Errors = Errors = new Error[] { new(message) };
        }
    }
}
