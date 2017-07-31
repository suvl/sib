namespace Sib.Repository
{
    using System;

    using MongoDB.Driver;

    using Sib.Core.Settings;

    public abstract class BaseRepository
    {
        protected readonly IMongoClient Client;

        protected readonly IMongoDatabase Database;

        protected BaseRepository(MongoDbSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.Client = new MongoClient(settings.ConnectionString);
            this.Database = this.Client.GetDatabase(settings.DatabaseName);
        }
    }
}