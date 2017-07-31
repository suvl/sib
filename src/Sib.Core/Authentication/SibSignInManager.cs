namespace Sib.Core.Authentication
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class SibSignInManager<TUser> : SignInManager<TUser> where TUser : class, IApplicationUser
    {
        public SibSignInManager(SibUserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SibSignInManager<TUser>> logger)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger)
        {
            logger.LogDebug($".ctor {nameof(SibSignInManager<TUser>)}");
        }
    }
}
