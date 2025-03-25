using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace Bankify.Api.Filters
{
    public class AuthorizationHandler : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AuthorizationHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public List<string> Anonymous = new List<string>
        {
            "Auth-Login"            
        };

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context?.ActionDescriptor is not ControllerActionDescriptor descriptor)
                return;

            // Get API resource identifier
            string apiClaim = $"{descriptor.ControllerName}-{descriptor.ActionName}";

            // Skip authorization for anonymous endpoints
            if (Anonymous.Contains(apiClaim))
                return;

            // Get authorization header
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            
            if (string.IsNullOrEmpty(authHeader))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Authorization header is missing." });
                return;
            }

            // Extract token from "Bearer {token}" format
            var token = authHeader.Split(' ').LastOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Invalid token format." });
                return;
            }

            try
            {
                // Validate and read the token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                // Get claims from the token
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var userName = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                var email = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value;
                var userRoles = jwtToken.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

                // Store claims in HttpContext for later use
                context.HttpContext.Items["UserId"] = userId;
                context.HttpContext.Items["UserName"] = userName;
                context.HttpContext.Items["Email"]=email;
                context.HttpContext.Items["UserRoles"] = userRoles;
               
            }
            catch (SecurityTokenExpiredException)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Token has expired." });
                return;
            }
            catch (SecurityTokenValidationException)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Invalid token." });
                return;
            }
            catch (Exception ex)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Unauthorized", details = ex.Message });
                return;
            }
        }
    }
}