using Sib.Core.Models;

namespace Sib.Core.Helpers
{
    public static class ApplicationUserHelpers
    {
        public static void AddFbContextToUser(this ApplicationUser user, Microsoft.AspNetCore.Http.HttpContext context)
        {
            if (context.Items.ContainsKey("name"))
                user.Name = context.Items["name"] as string;

            if (context.Items.ContainsKey("firstname"))
                user.FirstName = context.Items["firstname"] as string;

            if (context.Items.ContainsKey("lastname"))
                user.LastName = context.Items["lastname"] as string;
        }
    }
}
