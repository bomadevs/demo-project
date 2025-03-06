namespace DemoProject.Application.Mediator
{
    public abstract class ErrorResponse : IApiResponse
    {
        public string ErrorCode { get; set; }
        public Error[] Errors { get; set; }
    }
}
