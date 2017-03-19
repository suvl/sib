using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Sib.Core.Domain;
using Sib.Core.Settings;
using Xunit;

namespace Sib.Repository.Tests
{
    public class TestServiceRepository
    {

        private ServiceRepository sut;

        public TestServiceRepository()
        {
            sut = new ServiceRepository(new MongoDbSettings { ConnectionString = "mongodb://localhost:27017", DatabaseName = "SibTests"});
        }
        
        [Fact, Trait("Category", "Integration")]
        public async Task TestInsert()
        {
            var location = Guid.NewGuid().ToString();
            var time = DateTime.UtcNow;
            var start = DateTime.UtcNow.TimeOfDay;

            var service = new Service
            {
                Location = location,
                Date = time,
                Start = start
            };

            await sut.Insert(service).ConfigureAwait(false);
            
            var services = await sut.GetServicesBetween(DateTime.MinValue, DateTime.MaxValue).ConfigureAwait(false);
            var reposervices = await services.ToListAsync().ConfigureAwait(false);
            Assert.True(reposervices.Any(_ => _.Location == location));
        }

        [Fact, Trait("Category", "Integration")]
        public async Task TestUpdate()
        {
            var services = await sut.GetServicesBetween(DateTime.MinValue, DateTime.MaxValue).ConfigureAwait(false);
            var reposervices = await services.ToListAsync().ConfigureAwait(false);

            var rnd = new Random();
            var idx = rnd.Next(0, reposervices.Count);

            var service = reposervices[idx];

            service.Location = Guid.NewGuid().ToString();

            await sut.Update(service).ConfigureAwait(false);

            services = await sut.GetServicesBetween(DateTime.MinValue, DateTime.MaxValue).ConfigureAwait(false);
            reposervices = await services.ToListAsync().ConfigureAwait(false);
            Assert.True(reposervices.Any(_ => _.Location == service.Location));
        }
    }
}
