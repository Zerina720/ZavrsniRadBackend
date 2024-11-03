using AutoMapper;
using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.DTO.Response;
using IzboriiS.IService;
using MongoDB.Driver;
using MongoDbGenericRepository;

namespace IzboriiS.Service
{
    public class IzbornaListaService : IIzbornaListaService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<IzbornaLista> _izbornaListaCollection;
        private readonly IMongoCollection<Izbori> _izboriCollection;

        public IzbornaListaService(IMapper mapper, IMongoDbContext context)
        {
            _mapper = mapper;
            _usersCollection = context.GetCollection<User>("Users");
            _izbornaListaCollection = context.GetCollection<IzbornaLista>("IzbornaListas");
            _izboriCollection = context.GetCollection<Izbori>("Izboris");
        }

        public async Task<bool> PovuciKandidaturu(string id)
        {
            var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null) return false;

            user.datumKreiranja = DateTime.Now;
            user.status = null;
            user.naCekanjuId = null;

            var updateResult = await _usersCollection.ReplaceOneAsync(u => u.Id == id, user);
            return updateResult.ModifiedCount > 0;
        }

        public async Task<List<IzbornaLista>> GetAll()
        {
            return await _izbornaListaCollection.Find(_ => true).ToListAsync();
        }

        public async Task<IzbornaLista> GetById(string id)
        {
            return await _izbornaListaCollection.Find(l => l._id == id).FirstOrDefaultAsync();
        }

        public async Task<IzbLCreateResponse> KreirajListu(IzbornaLRequest izbornaLista)
        {
            var existingList = await _izbornaListaCollection.Find(x => x.Naziv == izbornaLista.Naziv).FirstOrDefaultAsync();
            if (existingList != null)
            {
                return new IzbLCreateResponse { Success = false, Message = "Lista sa tim imenom već postoji!" };
            }

            var user = await _usersCollection.Find(u => u.Id == izbornaLista.JMBG).FirstOrDefaultAsync();
            if (user.naCekanjuId != null)
            {
                return new IzbLCreateResponse { Success = false, Message = "Već se nalazite na listi čekanja!" };
            }

            if (user.IzbornaListaId != null)
            {
                return new IzbLCreateResponse { Success = false, Message = "Već se nalazite na nekoj listi!" };
            }

            if (user.datumKreiranja.HasValue && (DateTime.Now - user.datumKreiranja.Value).TotalSeconds < 259200)
            {
                return new IzbLCreateResponse { Success = false, Message = "Listu možete prijaviti nakon 3 dana od povlačenja kandidature!" };
            }

            var novaLista = _mapper.Map<IzbornaLista>(izbornaLista);
            await _izbornaListaCollection.InsertOneAsync(novaLista);

            user.IzbornaListaId = novaLista._id;
            await _usersCollection.ReplaceOneAsync(u => u.Id == user.Id, user);

            return new IzbLCreateResponse { Success = true, Message = "Lista je kreirana!" };
        }

        public async Task<IzbLCreateResponse> PrijaviSeZaIzbore(ListaIzboriRequest lista)
        {
            var izbor = await _izboriCollection.Find(i => i.IdIzbora == lista.id).FirstOrDefaultAsync();
            var iLista = await _izbornaListaCollection.Find(l => l._id == lista.IdIzborneListe).FirstOrDefaultAsync();

            if (iLista.IzboriId != null)
                return new IzbLCreateResponse { Success = false, Message = "Već ste se prijavili na izbore!" };

            if (izbor != null && izbor.TerminIzbora < DateTime.Now)
                return new IzbLCreateResponse { Success = false, Message = "Izbori su istekli!" };

            iLista.IzboriId = lista.id;
            await _izbornaListaCollection.ReplaceOneAsync(l => l._id == iLista._id, iLista);

            izbor.BrPrijavljenihListi++;
            await _izboriCollection.ReplaceOneAsync(i => i.IdIzbora == izbor.IdIzbora, izbor);

            return new IzbLCreateResponse { Success = true, Message = "Uspešno ste se prijavili za izbore!" };
        }

        public async Task<List<IzbornaLista>> PrikaziListeKojimaPripada(string id)
        {
            var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            return await _izbornaListaCollection.Find(x => x._id == user.IzbornaListaId).ToListAsync();
        }

        public async Task<List<IzbornaLista>> UserListaNaCekanju(string id)
        {
            var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            return await _izbornaListaCollection.Find(x => x._id == user.naCekanjuId).ToListAsync();
        }

        public async Task<List<IzbornaLista>> PrikaziListeKojimaNePripada(string id)
        {
            var user = await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            return await _izbornaListaCollection.Find(x => x._id != user.IzbornaListaId).ToListAsync();
        }
    }
}
