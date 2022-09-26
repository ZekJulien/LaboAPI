using DAL.Exceptions;
using DAL.Interfaces;
using LaboAPI.DTO;
using LaboAPI.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LaboAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentServices _TournamentServices;

        public TournamentController(ITournamentServices tournamentServices)
        {
            
            _TournamentServices = tournamentServices;
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public IActionResult Add(TournamentFormDTO dto)
        {
            if (dto.MinNumberPlayers < 2 || dto.MinNumberPlayers > 32)
            {
                return BadRequest("Le nombre de joueur minimun doit etre supérieur a 2 ou inférieur a 32");
            }
            if (dto.MaxNumberPlayers < 2 || dto.MaxNumberPlayers > 32)
            {
                return BadRequest("Le nombre de joueur minimun doit etre supérieur a 2 ou inférieur a 32");
            }
            if (dto.MinELO < 0 || dto.MinELO > 3000)
            {
                return BadRequest("Le nombre de ELO minimun doit etre supérieur a 0 ou inférieur a 3000");
            }
            if (dto.MaxELO < 0 || dto.MaxELO > 3000)
            {
                return BadRequest("Le nombre de ELO minimun doit etre supérieur a 0 ou inférieur a 3000");
            }

            DateTime now = DateTime.Now;
            dto.RegistrationEndDate = now.AddDays(dto.MinNumberPlayers);

            try
            {
                _TournamentServices.Add(dto.ToDAL());
            }
            catch
            {
                return BadRequest("Erreur lors de l'ajout dans la DB");
            }

            return Ok();
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "1")]
        public IActionResult Delete([FromRoute] Guid Id)
        {
            try
            {
                 if (_TournamentServices.GetStatus(Id) == 2)
                 {
                  return BadRequest("Vous ne pouvez pas supprimer un tournois en cours");
                 }
            }
            catch(TournamentExceptions ex)
            {
                return NotFound(ex.Message);
            }
           
            try
            {
                _TournamentServices.Delete(Id);
            }
            catch(TournamentExceptions ex)
            {
                return NotFound(ex.Message);
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
           
            try
            {
                if (User.Identity?.IsAuthenticated != false)
                {
                    Guid Id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    return Ok(_TournamentServices.GetAll(Id));
                }
                else return Ok(_TournamentServices.GetAll());
            }
            catch (TournamentExceptions ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public IActionResult GetDetails([FromRoute] Guid Id)
        {
            try
            {
                return Ok(_TournamentServices.GetDetails(Id));
            }
            catch (TournamentExceptions ex)
            {
                return NotFound(ex.Message);
            }
        }


    }

        

    
}
