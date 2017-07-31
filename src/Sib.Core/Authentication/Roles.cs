using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sib.Core.Authentication
{
    using Microsoft.AspNetCore.Identity.MongoDB;
    public static class Roles
    {
        public const string Administrator = "ADMINISTRATOR";

        public const string Musician = "MUSICIAN";

        public const string Parent = "PARENT";

        public static IEnumerable<string> KnownRoles => new[] { Administrator, Musician, Parent };
    }
}
