using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IzboriiS.DTO.Request
{
    public class DeleteIzbornaListaRequest
    {
        public string idUser { get; set; }
        public int id { get; set; }
    }
}
