using IzboriiS.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace IzboriiS.DTO.Request
{
    public class IzboriRequest
    {
        public DateTime TerminIzbora { get; set; }
        public string DodatneInfo { get; set; }
        public int TipIzboraId { get; set; }
    }
}
