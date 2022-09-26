using DAL.Entities;
using DAL.Exceptions;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class MembersServices : IMembersServices
    {
        private readonly string connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LaboDB;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public void Add(MembersEntities Member)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO [Members] (Pseudo, Email, Password, Birthdate, GenderID) VALUES (@p1, @p2, @p3, @p4, @p5) ";
                cmd.Parameters.AddWithValue("p1", Member.Pseudo);
                cmd.Parameters.AddWithValue("p2", Member.Email);
                cmd.Parameters.AddWithValue("p3", Member.Password);
                cmd.Parameters.AddWithValue("p4", Member.BirthDate);
                cmd.Parameters.AddWithValue("p5", Member.GenderID);
                cmd.ExecuteNonQuery();
            };
        }



        public bool EmailCheck(string value)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM [Members] WHERE [Email] LIKE @p1";
                cmd.Parameters.AddWithValue("p1", value);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return true;
                    }
                    else throw new MembersExceptions("Email déjà existant");
                };
            }
        }

        public MembersRegisteredEntities? GetDetails(Guid Id)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM [Members] WHERE Id = @p1";
                cmd.Parameters.AddWithValue("p1", Id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MembersRegisteredEntities
                        {
                            Id = (Guid)reader["Id"],
                            Pseudo = (string)reader["Pseudo"],
                            Email = (string)reader["Email"],
                            ELO = (Int16)reader["ELO"],
                            BirthDate = (DateTime)reader["BirthDate"],
                            RoleID = (byte)reader["RoleID"],
                            GenderID = (byte)reader["GenderID"],
                        };
                    }
                    return null;
                }
            };
        }


        public bool PseudoCheck(string value)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM [Members] WHERE [Pseudo] LIKE @p1";
                cmd.Parameters.AddWithValue("p1", value);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return true;
                    }
                    else throw new MembersExceptions("Pseudo déjà existant");
                };
            };
        }

    }
}
