namespace DemoProject.Application.Mediator
{
    /// <summary>
    /// Handler result class for the mediator pattern.
    /// </summary>
    public class HandlerResult<T>
    {
        public T Result { get; set; } = default!;

        public HandlerResult() { }

        public HandlerResult(T result)
        {
            this.Result = result;
        }
    }
}
