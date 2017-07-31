using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;

namespace Sib.Core.Authentication
{
    public interface IApplicationUser
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Name { get; set; }
        string Id { get; set; }
        string UserName { get; set; }
        string NormalizedUserName { get; set; }
        string SecurityStamp { get; set; }
        string Email { get; set; }
        string NormalizedEmail { get; set; }
        bool EmailConfirmed { get; set; }
        string PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        bool TwoFactorEnabled { get; set; }
        DateTime? LockoutEndDateUtc { get; set; }
        bool LockoutEnabled { get; set; }
        int AccessFailedCount { get; set; }
        List<string> Roles { get; set; }
        string PasswordHash { get; set; }
        List<IdentityUserLogin> Logins { get; set; }
        List<IdentityUserClaim> Claims { get; set; }
        List<IdentityUserToken> Tokens { get; set; }
        void AddRole(string role);
        void RemoveRole(string role);
        void AddLogin(UserLoginInfo login);
        void RemoveLogin(string loginProvider, string providerKey);
        bool HasPassword();
        void AddClaim(Claim claim);
        void RemoveClaim(Claim claim);
        void ReplaceClaim(Claim existingClaim, Claim newClaim);
        void SetToken(string loginProider, string name, string value);
        string GetTokenValue(string loginProider, string name);
        void RemoveToken(string loginProvider, string name);
        string ToString();
    }
}