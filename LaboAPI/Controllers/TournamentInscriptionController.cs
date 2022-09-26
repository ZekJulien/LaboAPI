using DAL.Exceptions;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace LaboAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentInscriptionController : ControllerBase
    {
        private readonly ITournamentServices _TournamentServices;

        public TournamentInscriptionController(ITournamentServices tournamentServices)
        {

            _TournamentServices = tournamentServices;
        }



        [HttpPost("{TournamentId}")]
        [Authorize]
        public IActionResult Inscription([FromRoute] Guid TournamentId)
        {
            try
            {
                Guid Id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _TournamentServices.TournamentInscription(TournamentId, Id);
                return Ok();
            }
            catch (TournamentExceptions ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{TournamentId}")]
        [Authorize]
        public IActionResult Unscription([FromRoute] Guid TournamentId)
        {
            try
            {
                Guid Id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _TournamentServices.TournamentUnscription(TournamentId, Id);
                return Ok();
            }
            catch (TournamentExceptions ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
