namespace IzboriiS.DTO.Request
{
    public enum StatusZahteva
    {
        cekanje, prihvaceno, odbijeno
    }
    public class NaCekanjuRequest
    {
        public int IzbornaListaId { get; set; }
    }
}
