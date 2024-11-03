using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;

namespace IzboriiS.Data.Models
{
    public enum ZahtevZaListu { cekanje, prihvaceno, odbijeno }

    [CollectionName("Users")] // Naziv kolekcije u MongoDB-u
    public class User : MongoIdentityUser<string>
    {
        public string Sediste { get; set; }
        public string Naziv { get; set; }
        public string PIB { get; set; }
        public DateTime? datumOsnivanja { get; set; } // Nullable zbog potencijalnih null vrednosti

        public ZahtevZaListu? status { get; set; } // Enumi se automatski serijalizuju u string

        public string? IzbornaListaId { get; set; } // Za referenciranje izborne liste
        public IzbornaLista? izbornaLista { get; set; } // Ovaj entitet bi bio uklopljen u dokument

        public DateTime? datumKreiranja { get; set; }
        public string? naCekanjuId { get; set; }

        public bool Blokiran { get; set; } = false; // Podrazumevana vrednost

        // Dodajemo dodatne atribute, ako su potrebni, kompatibilne sa MongoDB-om
    }
}
