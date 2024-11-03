using AutoMapper;
using IzboriiS.Data;
using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.DTO.Response;
using IzboriiS.IService;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using MongoDbGenericRepository;

namespace IzboriiS.Service
{
    public class IzboriService : IIzboriService
    {
        private readonly IMongoCollection<Izbori> _izboriCollection;
        private readonly IMongoCollection<TipIzbora> _tipIzboraCollection;
        private readonly IMongoCollection<IzbornaLista> _izbornaListaCollection;
        private readonly IMapper mapper;

        public IzboriService(
            MongoDbContextt database,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _izboriCollection = database.GetCollection<Izbori>("Izboris");
            _tipIzboraCollection = database.GetCollection<TipIzbora>("TipIzboras");
            _izbornaListaCollection = database.GetCollection<IzbornaLista>("IzbornaListas");
            this.mapper = mapper;
        }

        public async Task<IzboriResponse> KreirajIzbore(IzboriRequest request)
        {
            var result = mapper.Map<Izbori>(request);

            // Setovanje tipa izbora prema prosleđenom ID-u
            result.TipIzboraId = request.TipIzboraId;

            // Ubacivanje u kolekciju
            await _izboriCollection.InsertOneAsync(result);

            return new IzboriResponse { Success = true, Message = "Uspešno ste objavili izbore!" };
        }

        public async Task<List<TipIzbora>> GetAllTips()
        {
            var tip = await _tipIzboraCollection.Find(_ => true).ToListAsync();
            return tip;
        }

        public async Task<List<Izbori>> GetAll()
        {
            // Preuzimanje svih izbora
            var izbori = await _izboriCollection.Find(_ => true).ToListAsync();
            return izbori;
        }

        public async Task<bool> DeleteIzbori(string Id)
        {
            // Pronađi izbore po ID-u
            var izbori = await _izboriCollection.Find(x => x.IdIzbora == Id).FirstOrDefaultAsync();
            if (izbori == null)
                return false;

            // Pronađi listu koja pripada ovim izborima
            var lista = await _izbornaListaCollection.Find(x => x.IzboriId == Id).FirstOrDefaultAsync();

            // Ako lista postoji, postavi IzboriId na null i ažuriraj je prema starom Id
            if (lista != null)
            {
                lista.IzboriId = null;
                await _izbornaListaCollection.ReplaceOneAsync(x => x._id == lista._id, lista);
            }

            await _izboriCollection.DeleteOneAsync(x => x.IdIzbora == Id);

            return true;
        }

    }
}
