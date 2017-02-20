namespace Sib.Core.Interfaces
{
    using System;

    public interface IDocument
    {
        string Id { get; set; }

        DateTime Created { get; set; }

        DateTime Updated { get; set; }
    }
}