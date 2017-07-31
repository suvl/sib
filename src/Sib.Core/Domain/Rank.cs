namespace Sib.Core.Domain
{
    using System;

    public class Rank : BaseDocument
    {
        public string RankName { get; set; }

        public Uri RankIcon { get; set; }

        public uint RankOrder { get; set; }
    }
}