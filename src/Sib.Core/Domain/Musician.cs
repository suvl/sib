using Sib.Core.Models;

namespace Sib.Core.Domain
{
    using System;

    public class Musician : Person
    {
        public ApplicationUser SibIdentity { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime EntryDate { get; set; }

        public Section Section { get; set; }

        public Rank Rank { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string IdentityCard { get; set; }

        public string SocialSecurityNumber { get; set; }
    }
}
