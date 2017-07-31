namespace Sib.Core.Domain
{
    using System;

    using MongoDB.Bson.Serialization.Attributes;

    using Sib.Core.Interfaces;

    public class BaseDocument : IDocument
    {
        
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public DateTime Created { get; set; } = DateTime.UtcNow;
        
        public DateTime Updated { get; set; } = DateTime.UtcNow;
    }
}