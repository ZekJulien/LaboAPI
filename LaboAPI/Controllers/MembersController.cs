using DAL.Entities;
using DAL.Exceptions;
using DAL.Interfaces;
using LaboAPI.DTO;
using LaboAPI.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace LaboAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMembersServices _MembersServices;

        public MembersController(IMembersServices membersServices)
        {
            _MembersServices = membersServices;
        }

        [HttpPost]
        public IActionResult Add(MembersFormDTO dto)
        {
            //Pseudo
            try
            {
                _MembersServices.PseudoCheck(dto.Pseudo);
            }
            catch (MembersExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            
            //Email
            try
            {
                _MembersServices.EmailCheck(dto.Email);
            }
            catch (MembersExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            //Password
            byte[] plainPassword = Encoding.UTF8.GetBytes(dto.Password);
            string hashedPassword = Encoding.UTF8.GetString(SHA512.HashData(plainPassword));

            dto.Password = hashedPassword;
            try
            {
                 _MembersServices.Add(dto.ToDAL());
            }
            catch
            {
                return BadRequest("Erreur lors de l'ajout dans la DB");
            }
           
            return Ok();
        }

        [HttpHead("PseudoCheck")]
        public IActionResult PseudoCheck(string pseudo)
        {
            try
            {
                _MembersServices.PseudoCheck(pseudo);
                return Ok();
            }
            catch (MembersExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpHead("EmailCheck")]
        public IActionResult EmailCheck(string email)
        {
            try
            {
                _MembersServices.EmailCheck(email);
                return Ok();
            }
            catch (MembersExceptions ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
