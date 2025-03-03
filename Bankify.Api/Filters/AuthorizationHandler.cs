
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bankify.Api.Filters
{
    public class AuthorizationHandler : IAuthorizationFilter
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IIdentityService _identityService;
        //private static string _enviromentVariable = Environment.GetEnvironmentVariable("GetNetErpServiceID");
        //private static long _serviceId = 28;

        public AuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            //_identityService = identityService;
        }

        public List<string> Anonymous = new List<string>
        {
            //"Actions-IdentityClaimSeeder",
            //"Account-ForgotPassword",
            //"Account-CreateUser",
            //"Case-GenerateFileNumber",
            //"Contract-GenerateFileNumber",
            //"Matters-GenerateFileNumber",
            //"UserGuide-GetAll",
            //"Template-DownloadTemplate",
            //"Documents-GetDocumentUrl",
            //"Addendum-GenerateAddendumNumber"
        };
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //if (context != null && context?.ActionDescriptor is ControllerActionDescriptor descriptor)
            //{
            //    // get api resource
            //    string apiClaim = string.Format("{0}-{1}", descriptor.ControllerName, descriptor.ActionName);

            //    //get header values
            //    var serviceKey = context.HttpContext.Request.Headers["Servicekey"].ToString();
            //    var email = context.HttpContext.Request.Headers["email"].ToString();
            //    var accessToken = context.HttpContext.Request.Headers["accessToken"].ToString();
            //    var idToken = context.HttpContext.Request.Headers["IdToken"].ToString();
            //    var OrganizationId = context.HttpContext.Request.Headers["OrganizationId"].ToString();
            //    var clientClaim = context.HttpContext.Request.Headers["clientClaim"].ToString();
            //    var IsMultiTenant = Convert.ToBoolean(context.HttpContext.Request.Headers["IsMultiTenant"]);
            //    if (!Anonymous.Contains(apiClaim))
            //    {
            //        if (String.IsNullOrEmpty(accessToken))
            //        {
            //            context.Result = new UnauthorizedObjectResult(new { message = "Access token is empty." });
            //            return;
            //        }

            //        //validate request using identityService
            //        var isValidRequest = _identityService.ValidateAllToken(accessToken, idToken, apiClaim, clientClaim, _serviceId, OrganizationId, IsMultiTenant);

            //        if (isValidRequest.IsError)
            //        {
            //            isValidRequest.Errors[0].Message = $"User is not Authorized to access {apiClaim}";
            //            context.Result = new UnauthorizedObjectResult(isValidRequest);
            //        }
            //        else if (isValidRequest.Message == "101")
            //        {
            //            isValidRequest.Errors[0].Message = $"User is not Authorized to access {apiClaim}";
            //            context.Result = new UnauthorizedObjectResult(isValidRequest);
            //        }
            //        else if (isValidRequest.Message == "102")
            //        {
            //            isValidRequest.Errors[0].Message = $"User is not Authorized to access {apiClaim}";
            //            context.Result = new UnauthorizedObjectResult(isValidRequest);
            //        }
            //        else if (isValidRequest.Message == "103")
            //        {
            //            isValidRequest.Errors[0].Message = $"User is not Authorized to access {apiClaim}";
            //            context.Result = new UnauthorizedObjectResult(isValidRequest);
            //        }
            //        else if (isValidRequest.Message == "104")
            //        {
            //            isValidRequest.Errors[0].Message = $"User is not Authorized to access {apiClaim}";
            //            context.Result = new UnauthorizedObjectResult(isValidRequest);
            //        }
            //        else
            //        {
            //            _httpContextAccessor?.HttpContext?.Session.SetString("client", isValidRequest.Payload.ClientId);
            //            _httpContextAccessor?.HttpContext?.Session.SetString("user", isValidRequest.Payload.UserId);
            //            _httpContextAccessor?.HttpContext?.Session.SetString("email", email);
            //            _httpContextAccessor?.HttpContext?.Session.SetString("accessToken", accessToken);
            //            _httpContextAccessor?.HttpContext?.Session.SetString("idToken", idToken);
            //        }

            //    }
            //    //_httpContextAccessor?.HttpContext?.Session.SetString("user", "Default User");
            //    //_httpContextAccessor?.HttpContext?.Session.SetString("email", email);

            //}
            //else
            //{
            //    context.Result = new UnauthorizedObjectResult(new { message = "context is  empty." });
            //}

                _httpContextAccessor?.HttpContext?.Session.SetString("user", "Default User");
                _httpContextAccessor?.HttpContext?.Session.SetString("email", "sewlesewbiazen65@gmail.com");
        }
    }
}
