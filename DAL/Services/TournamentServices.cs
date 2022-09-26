using DAL.Entities;
using DAL.Exceptions;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class TournamentServices : ITournamentServices
    {
        private readonly string connectionstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LaboDB;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private readonly IMembersServices _MembersServices;
        public TournamentServices(IMembersServices membersServices)
        {
            _MembersServices = membersServices;
        }

        public void Add(TournamentEntities tournament)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO [Tournament] ([Name], [Location],  [MinNumberPlayers], [MaxNumberPlayers], [MinELO], [MaxELO], [WomenOnly], [RegistrationEndDate]) OUTPUT INSERTED.Id VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p8, @p9)";
                cmd.Parameters.AddWithValue("p1", tournament.Name);
                cmd.Parameters.AddWithValue("p2", tournament.Location);
                cmd.Parameters.AddWithValue("p3", tournament.MinNumberPlayers);
                cmd.Parameters.AddWithValue("p4", tournament.MaxNumberPlayers);
                cmd.Parameters.AddWithValue("p5", tournament.MinELO);
                cmd.Parameters.AddWithValue("p6", tournament.MaxELO);
                cmd.Parameters.AddWithValue("p8", tournament.WomenOnly);
                cmd.Parameters.AddWithValue("p9", tournament.RegistrationEndDate);
                try
                {
                    Guid Id = (Guid)cmd.ExecuteScalar();
                    con.Close();
                    AddCategory(Id, tournament.CategoryID);
                }
                catch (TournamentExceptions)
                {
                    throw new TournamentExceptions("Erreur lors de l'ajouts dans la DB");
                }
            };
        }

        public bool AddCategory(Guid Id, IEnumerable<int> number)
        {
            if(number == Enumerable.Empty<int>())
            {
                return false;
            }
            else
            {
                foreach (int item in number)
                {
                    using SqlConnection con = new SqlConnection(connectionstring);
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO [TournamentCategory](Id, CategoryId) VALUES (@p1, @p2)";
                        cmd.Parameters.AddWithValue("p1", Id);
                        cmd.Parameters.AddWithValue("p2", item);
                        cmd.ExecuteNonQuery();
                    };
                }
                return true;
            }
        }

        public bool canRegister(Guid IdTournament = default, Guid IdMember = default)
        {
            if(IdTournament == default || IdMember == default) return false;
            MembersRegisteredEntities? Member = _MembersServices.GetDetails(IdMember);
            TournamentDetailsEntities Tournament = GetDetails(IdTournament);
            bool FoundMember = false;

            if (Member == null || Tournament == null) return false;
            if (Tournament.StatusID != 1) return false;
            if (Tournament.MembersRegistered != null)
            {
                if (Tournament.MembersRegistered.Count<MembersRegisteredEntities>() == Tournament.MaxNumberPlayers) return false;
            }
            if (Tournament.RegistrationEndDate < DateTime.Now) return false;
            if (Tournament.MembersRegistered != null)
            {
                foreach (MembersRegisteredEntities item in Tournament.MembersRegistered)
                {
                    if (Member.Id == item.Id)
                    {
                        FoundMember = true;
                        break;
                    }
                }
            }
            if (FoundMember) return false;
            int age;
            TimeSpan DayTournament = Tournament.RegistrationEndDate.Subtract(DateTime.Now);
            double TotalDay = DayTournament.TotalDays;
            DateTime BirthDate = Member.BirthDate.AddDays(TotalDay);
            age = DateTime.Now.Year - BirthDate.Year - (DateTime.Now.Month < BirthDate.Month ? 1 : DateTime.Now.Day < BirthDate.Day ? 1 : 0);
            switch (age)
            {
                case < 18:
                    if (Tournament.CategoryID.FirstOrDefault((x) => x.Equals(1)) != 1)
                    {
                        return false;
                    }
                    break;
                case >= 18 and < 60:
                    if (Tournament.CategoryID.FirstOrDefault((x) => x.Equals(2)) != 2)
                    {
                        return false;
                    };
                    break;
                case >= 60:
                    if (Tournament.CategoryID.FirstOrDefault((x) => x.Equals(3)) != 3)
                    {
                        return false;
                    };
                    break;
                default:

            }

            if (Member.ELO > Tournament.MaxELO) return false;
            if (Member.ELO < Tournament.MinELO) return false;

            if (Tournament.WomenOnly != (Member.GenderID != 1)) return false;
            
            return true;
        }

        public void Delete(Guid Id)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = @"
                    DELETE FROM TournamentCategory WHERE [Id] = @p1
                    DELETE FROM [Tournament] WHERE [Id] = @p1
                ";
                cmd.Parameters.AddWithValue("p1", Id);
                if (cmd.ExecuteNonQuery() == -1)
                {
                    throw new TournamentExceptions("Tournoi non trouvé");
                }
            };
        }

        public IEnumerable<TournamentIndexEntities> GetAll(Guid IdMember = default)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = @"SELECT TOP(10) t.*, ISNULL(Players, 0) Players, ISNULL(isRegistered, 0) isRegistered FROM [Tournament] t
                                                    LEFT JOIN
                                                    (SELECT TournamentID, COUNT(*) as Players FROM TournamentRegistration GROUP BY TournamentID )
                                                    g ON g.TournamentID = t.Id
                                                    LEFT JOIN
                                                    (SELECT TournamentID, MembersID, COUNT(*) as isRegistered FROM TournamentRegistration GROUP BY MembersID, TournamentID)
                                                    r ON r.MembersID = @p1 AND r.TournamentID = t.Id
                                                    WHERE [StatusID] != 3 ORDER BY [TournamentUpdateDate] DESC";
                cmd.Parameters.AddWithValue("p1", IdMember);
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                    yield return new TournamentIndexEntities
                    {
                        Id = (Guid)reader["Id"],
                        Name = (string)reader["Name"],
                        Location = (string)reader["Location"],
                        MinNumberPlayers = (Int16)reader["MinNumberPlayers"],
                        MaxNumberPlayers = (Int16)reader["MaxNumberPlayers"],
                        MinELO = (Int16)reader["MinELO"],
                        MaxELO = (Int16)reader["MaxELO"],
                        CategoryID = GetCategory((Guid)reader["Id"]),
                        StatusID = (Byte)reader["StatusID"],
                        Round = (Int16)reader["Round"],
                        WomenOnly = (bool)reader["WomenOnly"],
                        RegistrationEndDate = (DateTime)reader["RegistrationEndDate"],
                        TournamentCreateDate = (DateTime)reader["TournamentCreateDate"],
                        TournamentUpdateDate = (DateTime)reader["TournamentUpdateDate"],
                        Players = (int)reader["Players"],
                        canRegister = canRegister((Guid)reader["Id"], IdMember),
                        isRegistered = (int)reader["isRegistered"],
                    };
                    }
                }
            };
        }

        public IEnumerable<int> GetCategory(Guid Id)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM [TournamentCategory] WHERE Id = @p1";
                cmd.Parameters.AddWithValue("p1", Id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return (int)reader["CategoryId"];
                    }
                }
            };
        }

        public TournamentDetailsEntities GetDetails(Guid Id)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM [Tournament] WHERE Tournament.Id = @p1";
                cmd.Parameters.AddWithValue("p1", Id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    { 
                            return new TournamentDetailsEntities
                            {
                                Id = (Guid)reader["Id"],
                                Name = (string)reader["Name"],
                                Location = (string)reader["Location"],
                                MinNumberPlayers = (Int16)reader["MinNumberPlayers"],
                                MaxNumberPlayers = (Int16)reader["MaxNumberPlayers"],
                                MinELO = (Int16)reader["MinELO"],
                                MaxELO = (Int16)reader["MaxELO"],
                                CategoryID = GetCategory(Id),
                                StatusID = (Byte)reader["StatusID"],
                                Round = (Int16)reader["Round"],
                                WomenOnly = (bool)reader["WomenOnly"],
                                RegistrationEndDate = (DateTime)reader["RegistrationEndDate"],
                                TournamentCreateDate = (DateTime)reader["TournamentCreateDate"],
                                TournamentUpdateDate = (DateTime)reader["TournamentUpdateDate"],
                                MembersRegistered = GetRegisteredMembers(Id)
                            };
                    }
                    throw new TournamentExceptions("Tournoi non trouvé");
                }
            };
        }

        public IEnumerable<MembersRegisteredEntities> GetRegisteredMembers(Guid Id)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM [TournamentRegistration]
                                                         INNER JOIN[Members] ON [Members].Id = [TournamentRegistration].MembersID 
                                                         WHERE TournamentRegistration.TournamentID = @p1";
                cmd.Parameters.AddWithValue("p1", Id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new MembersRegisteredEntities
                        {
                            Id = (Guid)reader["Id"],
                            Pseudo = (string)reader["Pseudo"],
                            Email = (string)reader["Email"],
                            BirthDate = (DateTime)reader["BirthDate"],
                            ELO = (Int16)reader["ELO"],
                            GenderID = (byte)reader["GenderID"],
                            RoleID = (byte)reader["RoleID"],
                        };
                    }
                }
            };
        }

        public DateTime GetRegistrationEndDate(Guid Id)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "SELECT [RegistrationEndDate] FROM [Tournament] WHERE [Id] = @p1";
                cmd.Parameters.AddWithValue("p1", Id);
                //return DateTime.Parse(cmd.ExecuteScalar().ToString());
                return (DateTime)cmd.ExecuteScalar();
             };
        }

        public byte GetStatus(Guid Id)
        {
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "SELECT [StatusID] FROM [Tournament] WHERE [Id] = @p1";
                cmd.Parameters.AddWithValue("p1", Id);
                try
                {
                    return (byte)cmd.ExecuteScalar();
                }
                catch
                {
                    throw new TournamentExceptions("Tournoi non trouvé");
                }
            };
        }

        public void TournamentInscription(Guid TournamentId, Guid MemberId)
        {
            MembersRegisteredEntities? Member = _MembersServices.GetDetails(MemberId);
            TournamentDetailsEntities? Tournament = GetDetails(TournamentId);
            bool FoundMember = false;

            if (Member == null || Tournament == null) throw new TournamentExceptions();
            if (Tournament.StatusID != 1) throw new Exceptions.TournamentExceptions("Le tournoi a déjà commencer ou est terminé");
            if (Tournament.MembersRegistered != null)
            {
            if (Tournament.MembersRegistered.Count<MembersRegisteredEntities>() == Tournament.MaxNumberPlayers) throw new Exceptions.TournamentExceptions("Le tournoi est complet");
            }
            if ( Tournament.RegistrationEndDate < DateTime.Now) throw new Exceptions.TournamentExceptions("La date d'inscription du tournoi est dépassé");
            if (Tournament.MembersRegistered != null)
            {
                foreach (MembersRegisteredEntities item in Tournament.MembersRegistered)
                {
                    if (Member.Id == item.Id)
                    {
                        FoundMember = true;
                        break;
                    }
                }
            }
            if (FoundMember) throw new Exceptions.TournamentExceptions("Vous êtes déjà incrits a ce tournois");
            int age;
            TimeSpan DayTournament = Tournament.RegistrationEndDate.Subtract(DateTime.Now);
            double TotalDay = DayTournament.TotalDays;
            DateTime BirthDate = Member.BirthDate.AddDays(TotalDay);
            age = DateTime.Now.Year - BirthDate.Year - (DateTime.Now.Month < BirthDate.Month ? 1 : DateTime.Now.Day < BirthDate.Day ? 1 : 0);
            switch (age)
            {
                case < 18:
                    if (Tournament.CategoryID.FirstOrDefault((x) => x.Equals(1)) != 1)
                    {
                        throw new Exceptions.TournamentExceptions("Tournoi non complatible avec votre catégories");
                    }
                    break;
                case >=18 and <60:
                    if(Tournament.CategoryID.FirstOrDefault((x) => x.Equals(2)) != 2)
                    {
                        throw new Exceptions.TournamentExceptions("Tournoi non complatible avec votre catégories");
                    };
                    break;
                case >= 60:
                    if (Tournament.CategoryID.FirstOrDefault((x) => x.Equals(3)) != 3)
                    {
                        throw new Exceptions.TournamentExceptions("Tournoi non complatible avec votre catégories");
                    };
                    break;
                    default:
                    throw new Exceptions.TournamentExceptions("??");
            }

            if (Member.ELO > Tournament.MaxELO) throw new Exceptions.TournamentExceptions("Votre ELO dépasse l'ELO maximal du tournoi");
            if (Member.ELO < Tournament.MinELO) throw new Exceptions.TournamentExceptions("Votre ELO est inférieur à l'ELO minimal du tournoi");

            if (Tournament.WomenOnly != (Member.GenderID !=1)) throw new Exceptions.TournamentExceptions("Ce tournoi est reserver au Fille");
            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO [TournamentRegistration] ([TournamentID],[MembersID]) VALUES (@p1, @p2)";
                cmd.Parameters.AddWithValue("p1", TournamentId);
                cmd.Parameters.AddWithValue("p2", MemberId);
                cmd.ExecuteNonQuery();
            };

        }

        

        public void TournamentUnscription(Guid TournamentId, Guid MemberId)
        {
            TournamentDetailsEntities? Tournament = GetDetails(TournamentId);
            bool FoundMember = false;

            if(Tournament.StatusID != 1) throw new Exceptions.TournamentExceptions("Tournoi à déjà commencer ou est terminé");
            if (Tournament.MembersRegistered != null)
            {
            foreach (MembersRegisteredEntities item in Tournament.MembersRegistered)
            {
                if (MemberId == item.Id)
                {
                    FoundMember = true;
                    break;
                }
            }
            }
            if (!FoundMember) throw new Exceptions.TournamentExceptions("Vous ne participer pas au tournoi");

            using SqlConnection con = new SqlConnection(connectionstring);
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM [TournamentRegistration] WHERE [TournamentID] = @p1 AND [MembersID] = @p2";
                cmd.Parameters.AddWithValue("p1", TournamentId);
                cmd.Parameters.AddWithValue("p2", MemberId);
                cmd.ExecuteNonQuery();
            };
        }
    }
}