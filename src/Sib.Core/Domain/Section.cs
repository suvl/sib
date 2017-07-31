using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sib.Core.Domain
{
    public class Section : BaseDocument
    {
        public string SectionName { get; set; }

        public Uri SectionIcon { get; set; }

        public Uri SectionImage { get; set; }

        public uint SectionOrder { get; set; }
    }
}
