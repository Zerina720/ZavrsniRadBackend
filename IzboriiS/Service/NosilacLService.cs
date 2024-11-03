using AutoMapper;
using IzboriiS.Data;
using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.DTO.Response;
using IzboriiS.IService;
using MongoDB.Driver;
using MongoDbGenericRepository;

namespace IzboriiS.Service
{
    public class NosilacLService : INosilacLService
    {
        private readonly IMongoCollection<NosilacListe> _nosilacListes;
        private readonly IMapper _mapper;

        public NosilacLService(MongoDbContextt database, IMapper mapper)
        {
            _nosilacListes = database.GetCollection<NosilacListe>("NosilacListes");
            this._mapper = mapper;
        }

        public async Task<NosilacLResponse> Create(NosilacListeRequest request)
        {
            var idIzbExist = await _nosilacListes.Find(x => x.IzbornaListaId == request.IzbornaListaId).FirstOrDefaultAsync();
            var nosilacExist = await _nosilacListes.Find(x => x.JMBG == request.JMBG).FirstOrDefaultAsync();

            if (nosilacExist != null)
                return new NosilacLResponse { Success = false, Message = "Već postoji nosilac liste sa unetim JMBG-om!" };

            if (request.JMBG.Length != 13)
                return new NosilacLResponse { Success = false, Message = "JMBG mora imati 13 karaktera!" };

            if (idIzbExist != null)
                return new NosilacLResponse { Success = false, Message = "Izborna lista već ima nosioca liste!" };

            var result = _mapper.Map<NosilacListe>(request);

            await _nosilacListes.InsertOneAsync(result);

            return new NosilacLResponse { Success = true, Message = "Uspešno ste dodali nosioca liste!" };
        }

        public async Task<NosilacLResponse> Update(NosilacLUpdate request)
        {
            var idIzbExist = await _nosilacListes.Find(x => x.JMBG == request.JMBG).FirstOrDefaultAsync();
            if (idIzbExist == null)
                return new NosilacLResponse { Success = false, Message = "Ne postoji nosioc liste!" };

            var updateDefinition = Builders<NosilacListe>.Update.Set(x => x.IzbornaListaId, request.IzbornaListaId.ToString());
            var updateResult = await _nosilacListes.UpdateOneAsync(x => x.JMBG == request.JMBG, updateDefinition);

            if (updateResult.ModifiedCount > 0)
                return new NosilacLResponse { Success = true, Message = "Uspešno ste promenili nosioca liste!" };

            return new NosilacLResponse { Success = false, Message = "Niste promenili informacije o nosiocu liste!" };
        }

        public async Task<NosilacListe> GetById(string IdListe)
        {
            return await _nosilacListes.Find(x => x.IzbornaListaId == IdListe).FirstOrDefaultAsync();
        }
    }
}
