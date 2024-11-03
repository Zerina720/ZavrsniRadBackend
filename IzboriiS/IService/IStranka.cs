using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.DTO.Response;

namespace IzboriiS.IService
{
    public interface IStranka
    {
        public Task<List<User>> GetAll();
        public Task<List<User>> GetById(string id);
        public Task<List<User>> GetZahteviNaCekanju(string idListe);
        public Task<User> Create(RegisterRequest stranka);
        public Task<IzbLCreateResponse> PrijaviIzbornuListu(ListaNaIzbRequest request);
        public Task<List<User>> DaLiSuNaIstojListi(string id);
        public Task<bool> PotvrdiKorisnika(string id);
        public Task<bool> OdobriKorisnikuListu(string id);
        public Task<bool> BlokirajKorisnika(string id);
        public Task<List<User>> NepotvrdjeniKorisnici();
        public Task<bool> OdbijZahtevZaListu(string idUser);
        public Task<List<User>> GetNeBlokirane();

    }
}
