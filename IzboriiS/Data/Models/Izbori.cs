using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace IzboriiS.Data.Models
{
    public class Izbori
    {
        [BsonId] // MongoDB koristi _id kao primarni ključ
        [BsonRepresentation(BsonType.ObjectId)] // Oznaka da je _id string tipa ObjectId
        public string IdIzbora { get; set; } // MongoDB koristi string za ObjectId

        public DateTime TerminIzbora { get; set; }
        public string DodatneInfo { get; set; }
        public int BrPrijavljenihListi { get; set; } = 0;

        public int? TipIzboraId { get; set; } // Nullable string za referencu

        [BsonIgnoreIfNull]
        public TipIzbora? TipIzbora { get; set; } // Opcionalna referenca na drugi entitet
    }
}
