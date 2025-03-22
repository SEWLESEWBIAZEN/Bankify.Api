
namespace Bankify.Application.Common.Helpers.HttpService.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }=String.Empty;
        public object Data { get; set; }=new object();
        public string AccessToken { get; set; } = String.Empty;
    }
}


