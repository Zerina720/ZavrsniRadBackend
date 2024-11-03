using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace IzboriiS.Data.Models
{
    public class IzbornaLista
    {
        [BsonId] // Oznaka da je RedniBrListe primarni ključ
        public string _id { get; set; } = Guid.NewGuid().ToString(); // Automatski generisan jedinstveni string

        public string Naziv { get; set; }
        public string Slogan { get; set; }

        [BsonRepresentation(BsonType.ObjectId)] // Referenca na dokument Izbori
        public string? IzboriId { get; set; }

        [BsonIgnoreIfNull]
        public Izbori? Izbori { get; set; } // Opcionalna referenca na entitet Izbori
    }
}
