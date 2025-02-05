
namespace Bankify.Domain.Models.Shared
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string StatusPhrase { get; set; }
        public List<string> Errors { get; } = [];
        public DateTime Timestamp { get; set; }
    }
}
