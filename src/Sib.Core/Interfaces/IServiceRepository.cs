namespace Sib.Core.Interfaces
{
    using System;
    using System.Threading.Tasks;

    using MongoDB.Driver;

    using Domain;

    public interface IServiceRepository : IBaseTypedRepository<Service>
    {
        Task<IAsyncCursor<Service>> GetServicesBetween(DateTime start, DateTime end);
    }
}