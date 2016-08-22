using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Model.Tilemap;
using Newtonsoft.Json;

namespace ElvenCurse.Core.Services
{
    public class WorldService:IWorldService
    {
        private readonly string _connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public IEnumerable<Character> GetOnlineCharacters()
        {
            throw new System.NotImplementedException();
        }

        public void EnterWorld(string getUserId)
        {
            throw new System.NotImplementedException();
        }

        public void LeaveWorld(string getUserId)
        {
            throw new System.NotImplementedException();
        }

        public List<Worldsection> GetMaps()
        {
            var list = new List<Worldsection>();
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "GetAllWorldsections";
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new Worldsection
                            {
                                Id = (int)dr["id"],
                                MapchangeDown = (int)dr["Mapchange_Down"],
                                MapchangeUp = (int)dr["Mapchange_Up"],
                                MapchangeRight = (int)dr["Mapchange_Right"],
                                MapchangeLeft = (int)dr["Mapchange_Left"],
                                Json = (string)dr["Json"],
                                Tilemap = ParseTilemap((string)dr["Json"]),
                                Name = (string)dr["Name"]
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool SaveMap(Worldsection section)
        {
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SaveWorldsection";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("Id", section.Id));
                    cmd.Parameters.Add(new SqlParameter("Mapchange_Down", section.MapchangeDown));
                    cmd.Parameters.Add(new SqlParameter("Mapchange_Up", section.MapchangeUp));
                    cmd.Parameters.Add(new SqlParameter("Mapchange_Right", section.MapchangeRight));
                    cmd.Parameters.Add(new SqlParameter("Mapchange_Left", section.MapchangeLeft));
                    cmd.Parameters.Add(new SqlParameter("Json", section.Json));
                    cmd.Parameters.Add(new SqlParameter("Name", section.Name));

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public Worldsection GetMap(int locationWorldsectionId)
        {
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "GetWorldsection";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("id", locationWorldsectionId));
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new Worldsection
                            {
                                Id = (int)dr["id"],
                                MapchangeDown = (int)dr["Mapchange_Down"],
                                MapchangeUp = (int)dr["Mapchange_Up"],
                                MapchangeRight = (int)dr["Mapchange_Right"],
                                MapchangeLeft = (int)dr["Mapchange_Left"],
                                Json = (string)dr["Json"],
                                Tilemap = ParseTilemap((string)dr["Json"]),
                                Name = (string)dr["Name"]
                            };
                        }
                    }
                }
            }
            return null;
        }

        private Tilemap ParseTilemap(string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Tilemap>(json);
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (JsonSerializationException)
            {
                return null;
            }
        }
    }
}
