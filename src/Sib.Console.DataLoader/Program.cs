namespace Sib.Console.DataLoader
{
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;

    class Program
    {
        const string FileName = @"musicians.json";

        static void Main(string[] args)
        {
            var content = System.IO.File.ReadAllText(FileName);
            var musicianList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MusicianRecord>>(content);
            musicianList.ForEach(m => Console.WriteLine(m.Name));

            MongoClient client = new MongoClient(new MongoUrl("")); // get this from config
            IMongoDatabase database = client.GetDatabase("Sib_Dev");
            IMongoCollection<MusicianRecord> collection = database.GetCollection<MusicianRecord>(nameof(MusicianRecord));

            collection.InsertMany(musicianList);
        }
    }
}
