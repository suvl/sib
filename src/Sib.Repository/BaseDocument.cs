namespace Sib.Repository
{
    using System;

    using Core.Interfaces;

    public class BaseDocument : IDocument
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Updated { get; set; } = DateTime.UtcNow;
    }
}