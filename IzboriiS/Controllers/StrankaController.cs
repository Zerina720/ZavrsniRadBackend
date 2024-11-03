using AutoMapper;
using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.IService;
using IzboriiS.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IzboriiS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StrankaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStranka strankaService;
        private readonly IAuthService authService;
        private readonly RoleManager<AppRole> role;  // Ostavili smo RoleManager umesto RoleService

        public StrankaController(IStranka stranka, IAuthService authService, RoleManager<AppRole> role)
        {
            this.strankaService = stranka;
            this.authService = authService;
            this.role = role;
        }

        // Get all users
        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var users = await strankaService.GetAll();
            return Ok(users);
        }

        // User registration
        [HttpPost("registracija")]
        public async Task<IActionResult> Kreiraj(RegisterRequest register)
        {
            var res = await authService.Create(register);
            if (res.Success)
                return Ok(new { msg = res.Message });
            return BadRequest(new { msg = res.Message });
        }

        [HttpDelete("izbrisiUsera/{id}")]
        public async Task<IActionResult> Izbrisi(string id)
        {
            var res = await authService.DeleteUser(id);
            if (res)
                return Ok(new { msg = "Izbrisali ste korisnika!" });
            return BadRequest(new { msg = "Nije moguće izbrisati korisnika!" });
        }

        // User login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var res = await authService.login(request);
            if (res.user == null)
                return BadRequest(new { msg = res.poruka });
            return Ok(res);
        }

        // Register election list
        [HttpPost("prijaviIzbListu")]
        public async Task<IActionResult> PrijaviListuNaIzbore(ListaNaIzbRequest request)
        {
            var res = await strankaService.PrijaviIzbornuListu(request);
            if (res.Success)
                return Ok(new { msg = res.Message });
            return BadRequest(new { msg = res.Message });
        }

        // Check if users are on the same election list
        [HttpGet("pregledajKorisnikeNaIstojListi/{id}")]
        public async Task<IActionResult> korisniciNaIstojListi(string id)
        {
            var users = await strankaService.DaLiSuNaIstojListi(id);
            return Ok(users);
        }

        // Get user by ID
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> getUserById(string id)
        {
            var user = await strankaService.GetById(id);
            if (user == null)
                return NotFound(new { msg = "Korisnik nije pronađen" });
            return Ok(user);
        }

        // Get unconfirmed users
        [HttpGet("nepotvrdjeniKorisnici")]
        public async Task<IActionResult> nepotvrdjeniKorisnici()
        {
            var users = await strankaService.NepotvrdjeniKorisnici();
            return Ok(users);
        }

        // Get non-blocked users
        [HttpGet("neBlokirani")]
        public async Task<IActionResult> neBlokiraniKorisnici()
        {
            var users = await strankaService.GetNeBlokirane();
            return Ok(users);
        }

        // Get pending requests for election list by ID
        [HttpGet("naCekanju/{id}")]
        public async Task<IActionResult> naCekanju(string id)
        {
            var requests = await strankaService.GetZahteviNaCekanju(id);
            return Ok(requests);
        }

        // Reject election list request by ID
        [HttpPut("OdbijZahtevZaListu/{id}")]
        public async Task<IActionResult> OdbijZahtevZaListu(string id)
        {
            var res = await strankaService.OdbijZahtevZaListu(id);
            if (res)
                return Ok(new { msg = "Odbili ste korisnika!" });
            return BadRequest(new { msg = "Niste odbili korisnika!" });
        }

        // Accept user to election list
        [HttpPut("PrihvatiKorisnikaNaListu/{id}")]
        public async Task<IActionResult> PrihvatiKorisnikaNaListu(string id)
        {
            var res = await strankaService.OdobriKorisnikuListu(id);
            if (res)
                return Ok(new { msg = "Korisnik je na Vašoj listi!" });
            return BadRequest(new { msg = "Neuspešno!" });
        }

        // Confirm user by ID
        [HttpPut("PotvrdiKorisnika/{id}")]
        public async Task<IActionResult> PotvrdiKorisnika(string id)
        {
            var res = await strankaService.PotvrdiKorisnika(id);
            if (res)
                return Ok(new { msg = "Potvrdili ste korisnika!" });
            return BadRequest(new { msg = "Niste potvrdili korisnika!" });
        }

        // Block user by ID
        [HttpPut("BlokirajKorisnika/{id}")]
        public async Task<IActionResult> BlokirajKorisnika(string id)
        {
            var res = await strankaService.BlokirajKorisnika(id);
            if (res)
                return Ok(new { msg = "Blokirali ste korisnika!" });
            return BadRequest(new { msg = "Niste blokirali korisnika!" });
        }

        // Get all roles (still using RoleManager<AppRole>)
        [HttpGet("/role")]
        public async Task<IActionResult> getRoles()
        {
            var lista = await role.Roles.ToListAsync();
            return Ok(lista);
        }
    }
}
