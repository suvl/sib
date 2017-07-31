using System.Collections.Generic;
using MongoDB.Driver.GeoJsonObjectModel;

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

            if (collection.Count(FilterDefinition<Service>.Empty) == 0)
            {
                // generate 4 debug services
                for (uint i = 0; i < 4; i++)
                {
                    var service = new Service
                    {
                        Coordinates = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(41.433333, -7.15)),
                        // { Coordinates = new GeoJson2DCoordinates(41.433333, -7.15) }
                        Created = DateTime.UtcNow,
                        Date = DateTime.Parse("2017-08-27"),
                        End = TimeSpan.Parse($"1{i}:00:00"),
                        Location = "Teste " + i,
                        Start = TimeSpan.Parse("06:00:00"),
                        Updated = DateTime.UtcNow,
                        Work = new List<string>
                        {
                            "Arruada",
                            "Missa",
                            "Concerto",
                            "Entrega"
                        }
                    };

                    await collection.InsertOneAsync(service).ConfigureAwait(false);
                }
            }

            return collection;
        }
    }
}