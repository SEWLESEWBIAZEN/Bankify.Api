namespace Bankify.Domain.Common
{
    public class NotValidException : Exception
    {
        internal NotValidException()
        {
            ValidationErrors = [];
        }

        internal NotValidException(string message) : base(message)
        {
            ValidationErrors = [];
        }

        internal NotValidException(string message, Exception inner) : base(message, inner)
        {
            ValidationErrors = [];
        }
        public List<string> ValidationErrors { get; }
    }
}
