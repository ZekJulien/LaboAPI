using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAuthServices
    {
        MembersEntities Auth(string login, string password);
        string GenerateToken(string secret, List<Claim> claims);
    }
}
