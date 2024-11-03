using IzboriiS.DTO.Request;
using IzboriiS.DTO.Response;
using IzboriiS.IService;
using Microsoft.AspNetCore.Mvc;

namespace IzboriiS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IzbornaLController : ControllerBase
    {
        private readonly IIzbornaListaService izbornaLista;
        public IzbornaLController(IIzbornaListaService izbornaLista)
        {
            this.izbornaLista = izbornaLista;
        }

        [HttpGet("pregledajListe")]
        public async Task<IActionResult> GetAll()
        {
            var lista = await izbornaLista.GetAll();
            return Ok(lista);
        }

        [HttpPost("kreirajListu")]
        public async Task<IActionResult> Kreiraj(IzbornaLRequest izbornaL)
        {
            var res = await izbornaLista.KreirajListu(izbornaL);
            if (res.Success)
                return Ok(new { msg = res.Message });
            return BadRequest(new { msg = res.Message });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var res = await izbornaLista.GetById(id);
            if (res == null)
                return BadRequest(new { msg = "Morate imati izbornu listu!" });
            return Ok(res);
        }

        [HttpPut("PovuciKandidaturu/{id}")]
        public async Task<IActionResult> PovuciKandidaturu(string id)
        {
            var res = await izbornaLista.PovuciKandidaturu(id);
            if (res)
                return Ok(new { msg = "Povukli ste kandidaturu!" });
            return BadRequest(new { msg = "Nije moguće povući kandidaturu!" });
        }

        [HttpPost("ListaNaIzb")]
        public async Task<IActionResult> PrijaviListuNaIzbore(ListaIzboriRequest lista)
        {
            var res = await izbornaLista.PrijaviSeZaIzbore(lista);
            if (res.Success)
                return Ok(new { msg = res.Message });
            return BadRequest(new { msg = res.Message });
        }

        [HttpGet("PrikaziListeKojimaNePripada/{id}")]
        public async Task<IActionResult> PrikaziListeKojimaNePripada(string id)
        {
            var res = await izbornaLista.PrikaziListeKojimaNePripada(id);
            return Ok(res);
        }

        [HttpGet("UserNaCekanju/{id}")]
        public async Task<IActionResult> UserNaCekanju(string id)
        {
            var res = await izbornaLista.UserListaNaCekanju(id);
            return Ok(res);
        }

        [HttpGet("PrikaziListeKojimaPripada/{id}")]
        public async Task<IActionResult> PrikaziListeKojimaPripada(string id)
        {
            var res = await izbornaLista.PrikaziListeKojimaPripada(id);
            return Ok(res);
        }
    }
}
