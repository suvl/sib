namespace Sib.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MongoDB.Driver;

    using Sib.Core.Domain;
    using Sib.Core.Settings;

    public class MusicianRepository : BaseTypedRepository<Musician>
    {
        public MusicianRepository(MongoDbSettings settings)
            : base(settings)
        {
        }

        public Task<IAsyncCursor<Musician>> GetMusiciansBySection(Section section)
        {
            var filter = this.FilterBuilder.Eq(_ => _.Section, section);
            return this.Collection.FindAsync<Musician>(filter);
        }
    }
}