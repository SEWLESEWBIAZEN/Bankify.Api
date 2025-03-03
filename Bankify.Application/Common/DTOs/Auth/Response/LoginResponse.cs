namespace Bankify.Application.Common.DTOs.Auth.Response
{
    public class LoginResponse
    {
        public bool Success { get; set; } = true;
        public string Token { get; set; } = "";

    }
}
