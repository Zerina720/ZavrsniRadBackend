using IzboriiS.Data.Models;
using IzboriiS.DTO.Request;
using IzboriiS.IService;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace IzboriiS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NosilacListeController : ControllerBase
    {
        private readonly INosilacLService _nosilacService;

        public NosilacListeController(INosilacLService nosilacService)
        {
            _nosilacService = nosilacService;
        }

        [HttpPost("kreirajNosioca")]
        public async Task<IActionResult> Kreiraj(NosilacListeRequest nosilacreq)
        {
            var res = await _nosilacService.Create(nosilacreq);
            if (res.Success)
                return Ok(new { msg = res.Message });
            return BadRequest(new { msg = res.Message });
        }

        [HttpPut("IzmeniNosioca")]
        public async Task<IActionResult> Izmeni(NosilacLUpdate nosilacL)
        {
            var res = await _nosilacService.Update(nosilacL);
            if (res.Success)
                return Ok(new { msg = res.Message });
            return BadRequest(new { msg = res.Message });
        }

        [HttpGet("GetNosilacById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            // Proveri da li je prosleđeni ID validan ObjectId format
            //if (!ObjectId.TryParse(id, out _))
              //  return BadRequest(new { msg = "Nevažeći format ID-ja!" });

            var res = await _nosilacService.GetById(id);
            if (res == null)
                return BadRequest(new { msg = "Ne postoji takav nosilac liste!" });
            return Ok(res);
        }
    }
}
