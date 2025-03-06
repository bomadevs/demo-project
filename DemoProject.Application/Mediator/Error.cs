namespace DemoProject.Application.Mediator
{
    /// <summary>
    /// Common error class to be used in the application.
    /// </summary>
    public class Error
    {
        public Error(string errorMsg)
        {
            ErrorMessage = errorMsg;
        }

        public Error(string errorMsg, string propertyName, object attemptedValue)
        {
            ErrorMessage = errorMsg;
        }

        public string ErrorMessage { get; set; }
    }
}
