namespace LocationSystem.Application.Exceptions
{
    public class ApplicationCustomException : Exception
    {
        public int StatusCode { get; }

        public ApplicationCustomException(string message, int statusCode = 400)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}