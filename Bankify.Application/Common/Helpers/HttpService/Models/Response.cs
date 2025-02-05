namespace Bankify.Application.Common.Helpers.HttpService.Models
{
    public class Response
    {
        public Response()
        {
            ErrorMessages = new List<string>();
        }
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        public string DisplayMessage { get; set; } = String.Empty;
        public List<string> ErrorMessages { get; set; }
    }
}
