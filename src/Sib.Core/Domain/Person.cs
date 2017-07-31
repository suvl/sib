namespace Sib.Core.Domain
{
    using Models;

    public abstract class Person : BaseDocument
    {
        public ApplicationUser IdentityUser { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }
    }
}
