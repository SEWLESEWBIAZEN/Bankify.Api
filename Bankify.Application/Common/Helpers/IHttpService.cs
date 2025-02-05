using Bankify.Application.Common.Helpers.HttpService.Models;
using Response = Bankify.Application.Common.Helpers.HttpService.Models.Response;


namespace Bankify.Application.Common.Helpers.IHttpService
{
    public interface IHttpService : IDisposable
    {
        Response responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}