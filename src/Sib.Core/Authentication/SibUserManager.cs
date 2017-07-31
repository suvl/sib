// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SibUserManager.cs" company="BFBVM">
//   @ 2017 BFBVM
// </copyright>
// <summary>
//   The sib user manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

namespace Sib.Core.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The sib user manager.
    /// </summary>
    /// <typeparam name="TUser">The user object</typeparam>
    public class SibUserManager<TUser> : UserManager<TUser> where TUser : class, IApplicationUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SibUserManager{TUser}"/> class.
        /// </summary>
        /// <param name="store">
        /// The store.
        /// </param>
        /// <param name="optionsAccessor">
        /// The options accessor.
        /// </param>
        /// <param name="passwordHasher">
        /// The password hasher.
        /// </param>
        /// <param name="userValidators">
        /// The user validators.
        /// </param>
        /// <param name="passwordValidators">
        /// The password validators.
        /// </param>
        /// <param name="keyNormalizer">
        /// The key normalizer.
        /// </param>
        /// <param name="errors">
        /// The errors.
        /// </param>
        /// <param name="services">
        /// The services.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public SibUserManager(
            IUserStore<TUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<TUser> passwordHasher, 
            IEnumerable<IUserValidator<TUser>> userValidators, 
            IEnumerable<IPasswordValidator<TUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<SibUserManager<TUser>> logger) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            logger.LogDebug($".ctor {nameof(SibUserManager<TUser>)}");
        }

        /// <summary>
        /// The get user given name.
        /// </summary>
        /// <param name="principal">
        /// The principal.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">principal must not be null
        /// </exception>
        public async Task<string> GetUserGivenName(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            IApplicationUser user = await this.GetUserAsync(principal).ConfigureAwait(false);

            return user.Name;
        }
    }
}
