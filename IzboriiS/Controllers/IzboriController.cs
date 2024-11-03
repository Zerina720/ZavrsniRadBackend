using IzboriiS.DTO.Request;
using IzboriiS.IService;
using Microsoft.AspNetCore.Mvc;

namespace IzboriiS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IzboriController : ControllerBase
    {
        private readonly IIzboriService izbori;

        public IzboriController(IIzboriService izbori)
        {
            this.izbori = izbori;
        }

        [HttpGet("pregledajIzbore")]
        public async Task<IActionResult> GetAll()
        {
            var listaIzbora = await izbori.GetAll();
            return Ok(listaIzbora);
        }

        // Endpoint za preglede svih tipova izbora
        [HttpGet("pregledajTipIzbora")]
        public async Task<IActionResult> GetAllTips()
        {
            var tip = await izbori.GetAllTips();
            return Ok(tip);
        }

        [HttpPost("kreirajIzbore")]
        public async Task<IActionResult> Kreiraj(IzboriRequest izborireq)
        {
            var res = await izbori.KreirajIzbore(izborireq);
            if (res.Success)
                return Ok(new { msg = res.Message });
            return BadRequest(new { msg = res.Message });
        }

        // Endpoint za brisanje izbora na osnovu ID-a
        [HttpDelete("izbrisiIzbore/{id}")]
        public async Task<IActionResult> Izbrisi(string id)
        {
            var res = await izbori.DeleteIzbori(id);
            if (res)
                return Ok(new { msg = "Izbrisali ste izbore!" });
            return BadRequest(new { msg = "Nije moguće izbrisati izbore!" });
        }
    }
}
