namespace Sib.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MongoDB.Driver;

    using Sib.Core.Domain;
    using Sib.Core.Settings;

    public class SibUserRepository : BaseTypedRepository<SibUser>
    {
        public SibUserRepository(MongoDbSettings settings)
            : base(settings)
        {
        }

        public Task<IAsyncCursor<SibUser>> GetUsersBySection(Section section)
        {
            var filter = this.FilterBuilder.Eq(_ => _.Section, section);
            return this.Collection.FindAsync<SibUser>(filter);
        }
    }
}