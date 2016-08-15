using System.Collections.Generic;
using System.Data.SqlClient;
using ElvenCurse.Core.Interfaces;
using System.Configuration;
using System.Data;
using ElvenCurse.Core.Model;

namespace ElvenCurse.Core.Services
{
    public class CharacterService:ICharacterService
    {
        private readonly string _connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Character> GetCharactersForUser(string userId)
        {
            var characters = new List<Character>();
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "GetCharactersForUser";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("userId", userId));
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            characters.Add(new Character
                            {
                                Id = (int)dr["id"],
                                Name = (string)dr["name"]
                            });
                        }
                    }
                }
            }
            return characters;
        }

        public bool CreateNewCharacter(string userId, Character model)
        {
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "AddNewCharacter";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("userId", userId));
                    cmd.Parameters.Add(new SqlParameter("name", model.Name));

                    var r = cmd.ExecuteScalar();

                    return r != null && (decimal) r > 0;
                }
            }
        }
    }
}
