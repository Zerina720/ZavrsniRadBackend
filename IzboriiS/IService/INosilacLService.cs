using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.DTO.Response;

namespace IzboriiS.IService
{
    public interface INosilacLService
    {
        public Task<NosilacLResponse> Create(NosilacListeRequest request);
        public Task<NosilacLResponse> Update(NosilacLUpdate request);
        public Task<NosilacListe> GetById(string IdListe);
    }
}
