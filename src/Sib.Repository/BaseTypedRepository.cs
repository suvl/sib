namespace Sib.Repository
{
    using System;
    using System.Threading.Tasks;
    
    using MongoDB.Driver;

    using Core.Interfaces;

    using MongoDB.Bson;

    using Sib.Core.Settings;

    public abstract class BaseTypedRepository<TDoc> : BaseRepository, IBaseTypedRepository<TDoc>
        where TDoc : IDocument
    {
        private IMongoCollection<TDoc> collection;

        protected IMongoCollection<TDoc> Collection => this.collection ?? (this.collection = this.Setup().GetAwaiter().GetResult());

        protected readonly IndexKeysDefinitionBuilder<TDoc> IndexBuilder;

        protected readonly FilterDefinitionBuilder<TDoc> FilterBuilder;

        protected BaseTypedRepository(MongoDbSettings settings)
            : base(settings)
        {
            this.IndexBuilder = new IndexKeysDefinitionBuilder<TDoc>();
            this.FilterBuilder = new FilterDefinitionBuilder<TDoc>();
        }

        protected virtual Task<IMongoCollection<TDoc>> Setup()
        {
            var collection = this.Database.GetCollection<TDoc>(typeof(TDoc).Name, new MongoCollectionSettings
            {
                AssignIdOnInsert = true,
                WriteConcern = WriteConcern.Acknowledged,
                ReadPreference = ReadPreference.PrimaryPreferred
            });

            return Task.FromResult(collection);
        }

        public virtual Task Insert(TDoc document)
        {
            return this.Collection.InsertOneAsync(document);
        }

        public virtual Task Delete(TDoc document)
        {
            return this.Collection.DeleteOneAsync(this.IdFilter(document));
        }

        public virtual Task Update(TDoc document)
        {
            document.Updated = DateTime.UtcNow;

            return this.Collection.ReplaceOneAsync(
                this.IdFilter(document),
                document);
        }

        public virtual async Task<TDoc> FindById(string id)
        {
            var cursor = await this.Collection.FindAsync(this.FilterBuilder.Eq(_ => _.Id, id)).ConfigureAwait(false);
            return await cursor.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        protected FilterDefinition<TDoc> IdFilter(TDoc doc)
        {
            return this.FilterBuilder.Eq(_ => _.Id, doc.Id);
        }
    }
}