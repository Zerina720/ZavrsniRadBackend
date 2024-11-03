using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbGenericRepository;

namespace IzboriiS.Data
{
    public class MongoDbContextt : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContextt(IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase("Izbori");
        }

        public IMongoClient Client => throw new NotImplementedException();

        public IMongoDatabase Database => throw new NotImplementedException();

        public void DropCollection<TDocument>(string partitionKey = null)
        {
            _database.DropCollection(typeof(TDocument).Name);
        }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Collection name cannot be null or empty");
            }

            // Logovanje imena kolekcije
            Console.WriteLine($"Getting collection: {name}");

            return _database.GetCollection<TDocument>(name);
        }


        public void SetGuidRepresentation(GuidRepresentation guidRepresentation)
        {
            throw new NotImplementedException();
        }
    }
}
