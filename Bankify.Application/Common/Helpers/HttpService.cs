using Bankify.Application.Common.Helpers.HttpService.Models;
using Bankify.Application.Common.Helpers.IHttpService;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Lms.Application.Common.Helpers.HttpService
{
    public class HttpService(IHttpClientFactory httpClient) : IHttpService
    {
        public Response responseModel { get; set; } = new Response();

        public void Dispose() => GC.SuppressFinalize(true);

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(apiRequest.AccessToken))
                {
                    ArgumentNullException argumentNullException = new();
                    throw argumentNullException;
                }

                var client = httpClient.CreateClient("BankifyServiceAPI");
                client.DefaultRequestHeaders.Clear();
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                message.Headers.Add("Servicekey", "89gIsmabiYR0OuW1B6NHovQsmWB8");
                message.RequestUri = new Uri(apiRequest.Url);
                message.Method = _getMethodType(apiRequest.ApiType);
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
                //
                //string json = "{\"YourBooleanProperty\":\"true\"}";
                //var deserializedObject = JsonConvert.DeserializeObject<T>(json);
                //bool value = deserializedObject;
                var response = await client.SendAsync(message);
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            catch (Exception ex)
            {
                return (T)(object)new Response
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
            }
        }

        private static HttpMethod _getMethodType(ApiType apiType) =>
            apiType switch
            {
                ApiType.GET => HttpMethod.Get,
                ApiType.POST => HttpMethod.Post,
                ApiType.PUT => HttpMethod.Put,
                ApiType.DELETE => HttpMethod.Delete,
                _ => throw new ArgumentException(message: "Invalid enum value", paramName: nameof(apiType))
            };
    }
}
