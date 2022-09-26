using DAL.Entities;
using DAL.Exceptions;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly string connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LaboDB;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private readonly IConfiguration _Configuration;

        public AuthServices(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public MembersEntities Auth(string login, string password)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM [Members] WHERE [Email] = @p1 OR [Pseudo] = @p1";
                cmd.Parameters.AddWithValue("p1", login);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    MembersEntities member = new MembersEntities();
                    if (reader.Read())
                    {
                        member.Id = (Guid)reader["Id"];
                        member.Pseudo = (string)reader["Pseudo"];
                        member.Email = (string)reader["Email"];
                        member.Password = (string)reader["Password"];
                        member.ELO = (Int16)reader["ELO"];
                        member.BirthDate = (DateTime)reader["BirthDate"];
                        member.RoleID = (byte)reader["RoleID"];
                        member.GenderID = (byte)reader["GenderID"];
                    }
                    else throw new AuthExceptions("Identifiant incorrect");

                    if (member.Password != password) throw new AuthExceptions("Mot de passe incorrect");
                    return member;
                }
            }
        }

        public string GenerateToken(string secret, List<Claim> claims)
        {
            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SecurityTokenDescriptor TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = new SigningCredentials(Key,SecurityAlgorithms.HmacSha256Signature),
            };

            JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
            SecurityToken Token = TokenHandler.CreateToken(TokenDescriptor);
            return TokenHandler.WriteToken(Token);
        }
    }
}
