using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IzboriiS.Data.Models
{
    public class OdbijeniZahtevi
    {
        [BsonId] // MongoDB koristi _id kao primarni ključ
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdZahteva { get; set; } // MongoDB koristi ObjectId umesto int za primarni ključ

 
        public string? IzbornaListaId { get; set; }

        [BsonIgnoreIfNull]
        public IzbornaLista? IzbornaLista { get; set; }

        public string? UserId { get; set; }

        [BsonIgnoreIfNull]
        public User? User { get; set; }
    }
}
