using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using IzboriiS.Data.Models;

namespace IzboriiS.Data
{
    public class AppDataContext
    {
        private readonly IMongoDatabase _database;

        public AppDataContext(IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase("Izbori");
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var userCollection = _database.GetCollection<User>("Users");

            var nazivIndex = Builders<User>.IndexKeys.Ascending(u => u.Naziv);
            var pibIndex = Builders<User>.IndexKeys.Ascending(u => u.PIB);

            userCollection.Indexes.CreateOne(new CreateIndexModel<User>(nazivIndex, new CreateIndexOptions { Unique = true }));
            userCollection.Indexes.CreateOne(new CreateIndexModel<User>(pibIndex, new CreateIndexOptions { Unique = true }));
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<IzbornaLista> IzbornaListas => _database.GetCollection<IzbornaLista>("IzborneListas");
        public IMongoCollection<Izbori> Izboris => _database.GetCollection<Izbori>("Izboris");
        public IMongoCollection<NosilacListe> NosilacListes => _database.GetCollection<NosilacListe>("NosilacListes");
        public IMongoCollection<TipIzbora> TipIzboras => _database.GetCollection<TipIzbora>("TipIzboras");
        public IMongoCollection<OdbijeniZahtevi> OdbijeniZahtevis => _database.GetCollection<OdbijeniZahtevi>("OdbijeniZahtevis");
    }
}
