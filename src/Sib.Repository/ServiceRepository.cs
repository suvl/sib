namespace Sib.Repository
{
    using System;
    using System.Threading.Tasks;

    using Core.Domain;

    using MongoDB.Driver;

    using Core.Settings;

    using Sib.Core.Interfaces;

    public class ServiceRepository: BaseTypedRepository<Service>, IServiceRepository
    {
        public ServiceRepository(MongoDbSettings settings)
            : base(settings)
        {
        }

        public Task<IAsyncCursor<Service>> GetServicesBetween(DateTime start, DateTime end)
        {
            var filter = this.FilterBuilder.And(
                this.FilterBuilder.Gt(_ => _.Date, start),
                this.FilterBuilder.Lt(_ => _.Date, end));
            return this.Collection.FindAsync(filter);
        }

        protected override async Task<IMongoCollection<Service>> Setup()
        {
            var collection = await base.Setup().ConfigureAwait(false);
            var index = this.IndexBuilder.Descending(_ => _.Date);
            await collection.Indexes.CreateOneAsync(index).ConfigureAwait(false);
            return collection;
        }
    }
}