using Microsoft.AspNetCore.Identity.MongoDB;
using Sib.Core.Authentication;

namespace Sib.Core.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IApplicationUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Name { get; set; }
    }
}

