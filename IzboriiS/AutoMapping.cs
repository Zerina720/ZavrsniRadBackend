using AutoMapper;
using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.DTO.Response;

namespace IzboriiS
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<RegisterRequest, User>();
            CreateMap<IzbornaLRequest, IzbornaLista>();
            CreateMap<IzboriRequest, Izbori>();
            CreateMap<NosilacListeRequest, NosilacListe>();
            CreateMap<User, RegisterResponse>();
            CreateMap<ListaNaIzbRequest, IzbornaLista>();
            CreateMap<ZahtevRequest, OdbijeniZahtevi>();
        }
    }
}
