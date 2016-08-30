﻿using System;
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

        public Character GetCharacter(string userId, int characterId)
        {
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "GetCharacter";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("userId", userId));
                    cmd.Parameters.Add(new SqlParameter("characterId", characterId));
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var character = new Character
                            {
                                Id = (int) dr["id"],
                                Name = (string) dr["name"]
                            };

                            if (dr["Worldsectionid"] == DBNull.Value)
                            {
                                character.Location = GetDefaultLocation(character);
                            }
                            else
                            {
                                character.Location = new Location
                                {
                                    WorldsectionId = (int)dr["worldsectionid"],
                                    X = (int)dr["x"],
                                    Y = (int)dr["y"]
                                };
                            }

                            return character;
                        }
                    }
                }
            }
            return null;
        }
        
        public void SetCharacterOnline(string userId, int selectedCharacterId)
        {
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SetCharacterOnline";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("userId", userId));
                    cmd.Parameters.Add(new SqlParameter("characterId", selectedCharacterId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Character GetOnlineCharacter(string userId)
        {
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "GetOnlineCharacterForUser";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("userId", userId));
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var character = new Character
                            {
                                Id = (int)dr["id"],
                                Name = (string)dr["name"]
                            };

                            if (dr["Worldsectionid"] == DBNull.Value)
                            {
                                character.Location = GetDefaultLocation(character);
                            }
                            else
                            {
                                character.Location = new Location
                                {
                                    WorldsectionId = (int)dr["worldsectionid"],
                                    X = (int)dr["x"],
                                    Y = (int)dr["y"]
                                };
                            }

                            return character;
                        }
                    }
                }
            }
            return null;
        }

        public void SavePlayerinformation(Character character)
        {
            // Save position
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SetCharacterPosition";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("characterId", character.Id));
                    cmd.Parameters.Add(new SqlParameter("worldsectionId", character.Location.WorldsectionId));
                    cmd.Parameters.Add(new SqlParameter("x", character.Location.X));
                    cmd.Parameters.Add(new SqlParameter("y", character.Location.Y));

                    cmd.ExecuteNonQuery();
                }
            }

            // Save inventory and so on...
        }

        private Location GetDefaultLocation(Character character)
        {
            return new Location
            {
                X = 80,
                Y = 30,
                WorldsectionId = 3
            };
        }
    }
}
