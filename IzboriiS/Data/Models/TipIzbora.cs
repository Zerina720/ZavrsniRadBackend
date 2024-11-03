using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IzboriiS.Data.Models
{
    public class TipIzbora
    {
        [BsonId]
        public ObjectId Id { get; set; } // Primarni ključ

        public int IdTipa { get; set; } 
        public string Naziv { get; set; }
    }
}

