using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.DTO.Response;

namespace IzboriiS.IService
{
    public interface IIzboriService
    {
        public Task<IzboriResponse> KreirajIzbore(IzboriRequest izbori);
        public Task<bool> DeleteIzbori(string Id);
        public Task<List<Izbori>> GetAll();
        public Task<List<TipIzbora>> GetAllTips();
       
    }
}
