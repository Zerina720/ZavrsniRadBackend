using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.DTO.Response;

namespace IzboriiS.IService
{
    public interface IIzbornaListaService
    {
        public Task<IzbLCreateResponse> KreirajListu(IzbornaLRequest izbornaLista);
        public Task<bool> PovuciKandidaturu(string id);
        public Task<List<IzbornaLista>> GetAll();
        public Task<IzbornaLista> GetById(string Id);
        public Task<List<IzbornaLista>> UserListaNaCekanju(string id);
      //  public Task<bool>Update(IzbornaLista lista, int Id);
       // public Task<bool> DeleteById(DeleteIzbornaListaRequest request);
        public Task<IzbLCreateResponse> PrijaviSeZaIzbore(ListaIzboriRequest lista);
        public Task<List<IzbornaLista>> PrikaziListeKojimaPripada(string id);
        public Task<List<IzbornaLista>> PrikaziListeKojimaNePripada(string id);
    }
}
