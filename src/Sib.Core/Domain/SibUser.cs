namespace Sib.Core.Domain
{
    using System;

    public class SibUser : Person
    {
        public DateTime BirthDate { get; set; }

        public DateTime EntryDate { get; set; }

        public Section Section { get; set; }

        public Rank Rank { get; set; }
    }
}
