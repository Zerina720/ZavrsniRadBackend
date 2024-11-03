using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IzboriiS.Data.Models
{
    public class NosilacListe
    {
        [BsonId] // MongoDB koristi _id kao primarni ključ
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // MongoDB koristi ObjectId umesto stringa

        public string JMBG { get; set; } // JMBG ne bi trebalo da bude primarni ključ u MongoDB
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Titula { get; set; }
        public string? IzbornaListaId { get; set; }

        [BsonIgnoreIfNull]
        public IzbornaLista? IzbornaLista { get; set; } // Opcionalna referenca na IzbornaLista entitet
    }
}
