namespace DemoProject.Application.Mediator
{
    public class ApiResponseResult : HandlerResult<IApiResponse>
    {
        public ApiResponseResult() { }

        public ApiResponseResult(IApiResponse result)
        {
            this.Result = result;
        }

        // check if the response has some results...
        public bool HasResults
        {
            get
            {
                // for data responses only...
                if (Result is IApiDataResponse dataResponse)
                {
                    return dataResponse.HasResults();
                }
                return false;
            }
        }

        public bool IsFailure => Result is ServiceUnavailableErrorResponse;
    }
}
