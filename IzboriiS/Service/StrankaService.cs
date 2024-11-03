using AutoMapper;
using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.IService;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using IzboriiS.Data;
using IzboriiS.DTO.Response;

namespace IzboriiS.Service
{
    public class StrankaService : IStranka
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<OdbijeniZahtevi> _odbijeniZahteviCollection;
        private readonly IMapper _mapper;

        public StrankaService(MongoDbContextt dbContext, IMapper mapper)
        {
            _usersCollection = dbContext.GetCollection<User>("Users"); // Pretpostavka naziva kolekcije
            _odbijeniZahteviCollection = dbContext.GetCollection<OdbijeniZahtevi>("OdbijeniZahtevis");
            _mapper = mapper;
        }

        public async Task<User> Create(RegisterRequest stranka)
        {
            var user = _mapper.Map<User>(stranka);
            await _usersCollection.InsertOneAsync(user);
            return user;
        }

        public async Task<List<User>> GetAll()
        {
            var list = await _usersCollection.Find(x => x.UserName != "admin" && x.Naziv != "admin").ToListAsync();
            return list;
        }

        public async Task<List<User>> GetNeBlokirane()
        {
            var lista = await _usersCollection.Find(x => !x.Blokiran && x.UserName != "admin" && x.Naziv != "admin").ToListAsync();
            return lista;
        }

        public async Task<List<User>> GetZahteviNaCekanju(string idListe)
        {
            var zahtevi = await _usersCollection.Find(x => x.naCekanjuId == idListe).ToListAsync();
            return zahtevi;
        }

        public async Task<List<User>> GetById(string id)
        {
            var stranka = await _usersCollection.Find(x => x.Id == id && x.naCekanjuId != null).ToListAsync();
            return stranka;
        }

        public async Task<List<User>> NepotvrdjeniKorisnici()
        {
            var korisnici = await _usersCollection.Find(x => !x.EmailConfirmed).ToListAsync();
            return korisnici;
        }

        public async Task<IzbLCreateResponse> PrijaviIzbornuListu(ListaNaIzbRequest request)
        {   
            var odbijen = await _odbijeniZahteviCollection.Find(x => x.IzbornaListaId == request.Id && x.UserId == request.IdUser).FirstOrDefaultAsync();
            var user = await _usersCollection.Find(x => x.Id == request.IdUser).FirstOrDefaultAsync();

            if (user.IzbornaListaId != null)
                return new IzbLCreateResponse { Success = false, Message = "Već ste prijavljeni na izbornoj listi!" };
            if (user.naCekanjuId != null)
                return new IzbLCreateResponse { Success = false, Message = "Već ste na čekanju na nekoj listi!" };
            if (odbijen != null)
                return new IzbLCreateResponse { Success = false, Message = "Lista Vam je već jednom odbila zahtev!" };

            if (user.datumKreiranja.HasValue)
            {
                var razlika = (DateTime.Now - user.datumKreiranja)?.TotalSeconds;
                if (razlika < 259200)
                    return new IzbLCreateResponse { Success = false, Message = "Listu možete prijaviti nakon 3 dana od povlačenja kandidature!" };
            }

            user.naCekanjuId = request.Id;
            user.status = ZahtevZaListu.cekanje;

            var updateResult = await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
            if (updateResult.ModifiedCount > 0)
                return new IzbLCreateResponse { Success = true, Message = "Poslali ste zahtev za listu, Vaš status je 'na čekanju'!" };

            return new IzbLCreateResponse { Success = false, Message = "Neuspešno!" };
        }

        public async Task<List<User>> DaLiSuNaIstojListi(string id)
        {
            var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            var korisnici = await _usersCollection.Find(x => x.IzbornaListaId == user.IzbornaListaId && x.Id != id).ToListAsync();
            return korisnici;
        }

        public async Task<bool> PotvrdiKorisnika(string id)
        {
            var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            user.EmailConfirmed = true;

            var updateResult = await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> OdobriKorisnikuListu(string id)
        {
            var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            user.status = ZahtevZaListu.prihvaceno;
            user.IzbornaListaId = user.naCekanjuId;
            user.naCekanjuId = null;

            var updateResult = await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> OdbijZahtevZaListu(string idUser)
        {
            var user = await _usersCollection.Find(u => u.Id == idUser).FirstOrDefaultAsync();
            user.status = ZahtevZaListu.odbijeno;

            var odbijeniStatus = new OdbijeniZahtevi
            {
                UserId = idUser,
                IzbornaListaId = user.naCekanjuId
            };

            await _odbijeniZahteviCollection.InsertOneAsync(odbijeniStatus);
            user.naCekanjuId = null;

            var updateResult = await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> BlokirajKorisnika(string id)
        {
            var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            user.Blokiran = true;

            var updateResult = await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
            return updateResult.ModifiedCount > 0;
        }
    }
}
