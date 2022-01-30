using FreeCourse.Shared.Constants;
using IdentityServer4.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.IdentityServer.Services
{
    public class TokenExchangeExtensionsGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => CustomIdentityServerConstants.TokenExchangeGrantTypes;
        private readonly ITokenValidator _tokenValidator;

        public TokenExchangeExtensionsGrantValidator(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var requestraw = context.Request.Raw.ToString();
            var token = context.Request.Raw.Get("subject_token");

            if (string.IsNullOrEmpty(token))
            {
                context.Result=new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidRequest,"token missing");
                return;
            }

            var tokenValidateResult=await _tokenValidator.ValidateIdentityTokenAsync(token);
            if (tokenValidateResult.IsError)
            {
                context.Result= new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidGrant, "token invalid");
                return;
            }

            var subjectClaim = tokenValidateResult.Claims.FirstOrDefault(c=>c.Type=="sub");

            if (subjectClaim==null)
            {
                context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidGrant, "token must contain sub value");
                return;
            }

            context.Result = new GrantValidationResult(subjectClaim.Value,"access_token",tokenValidateResult.Claims);
            return;
        }
    }
}
