using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Model.InteractiveObjects;
using ElvenCurse.Core.Model.Npcs;
using ElvenCurse.Core.Model.Tilemap;
using ElvenCurse.Core.Utilities;

namespace ElvenCurse.Core.Services
{
    public class WorldService:IWorldService
    {
        private readonly string _connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public IEnumerable<Character> GetOnlineCharacters()
        {
            throw new NotImplementedException();
        }

        public void EnterWorld(string getUserId)
        {
            throw new NotImplementedException();
        }

        public void LeaveWorld(string getUserId)
        {
            throw new NotImplementedException();
        }

        public List<Terrainfile> GetTerrains()
        {
            var list = new List<Terrainfile>();
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "GetAllTerrains";
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new Terrainfile
                            {
                                Id = (int)dr["id"],
                                Filename = (string)dr["Filename"],
                                Tileset = ParseTerrain((string)dr["data"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public Terrainfile GetTerrain(int id)
        {
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "GetTerrain";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("id", id));
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new Terrainfile
                            {
                                Id = (int)dr["id"],
                                Filename = (string)dr["Filename"],
                                Tileset = ParseTerrain((string)dr["data"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool SaveTerrain(Terrainfile model, string data)
        {
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SaveTerrain";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("Id", model.Id));
                    cmd.Parameters.Add(new SqlParameter("Filename", model.Filename));
                    cmd.Parameters.Add(new SqlParameter("data", data));

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
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
                                Tilemap = ParseTilemap((string)dr["Json"]),
                                Name = (string)dr["Name"]
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool SaveMap(Worldsection section, string mapdata)
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
                    cmd.Parameters.Add(new SqlParameter("Json", mapdata));
                    cmd.Parameters.Add(new SqlParameter("Name", section.Name));

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Npc> GetAllNpcs()
        {
            var list = new List<Npc>();
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "GetAllNpcs";
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Npc npc;
                            switch ((Npctype) dr["type"])
                            {
                                case Npctype.Hunter:
                                    npc = new HunterNpc();
                                    break;

                                default:
                                    continue;
                            }

                            // standard items
                            npc.Id = (int) dr["id"];
                            npc.Name = (string) dr["name"];
                            npc.Race = (Npcrace) dr["race"];
                            npc.Status = (Npcstatus) dr["status"];
                            npc.CurrentLocation = new Location
                            {
                                WorldsectionId = (int) dr["CurrentWorldsectionId"],
                                X = (int) dr["CurrentX"],
                                Y = (int) dr["CurrentY"]
                            };
                            npc.DefaultLocation = new Location
                            {
                                WorldsectionId = (int) dr["DefaultWorldsectionId"],
                                X = (int) dr["DefaultX"],
                                Y = (int) dr["DefaultY"]
                            };
                            list.Add(npc);
                        }
                    }
                }
            }
            return list;
        }

        public List<InteractiveObject> GetAllInteractiveObjects()
        {
            var list = new List<InteractiveObject>();
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "GetAllInteractiveObjects";
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            InteractiveObject io;
                            switch ((InteractiveobjectType)dr["type"])
                            {
                                case InteractiveobjectType.Portal:
                                    io = new Portal();
                                    break;

                                default:
                                    continue;
                            }

                            // standard items
                            io.Id = (int)dr["id"];
                            io.Name = (string)dr["name"];
                            io.Parameters = new List<InteractiveobjectParameter>();
                            io.Location = new Location
                            {
                                WorldsectionId = (int)dr["WorldsectionId"],
                                X = (int)dr["X"],
                                Y = (int)dr["Y"]
                            };
                            list.Add(io);
                        }
                    }
                }

                // hent parametre til objecterne..
                foreach (var io in list)
                {
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "GetAllInteractiveObjectParameters";
                        cmd.Parameters.Add(new SqlParameter("ioid", io.Id));
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                io.Parameters.Add(new InteractiveobjectParameter
                                {
                                    Id = (int)dr["id"],
                                    Key = (string)dr["key"],
                                    Value = (string)dr["value"]
                                });
                            }
                        }
                    }
                }
            }
            return list;
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
                                //Mapdata = (string)dr["Json"],
                                Tilemap = ParseTilemap((string)dr["Json"]),
                                Name = (string)dr["Name"]
                            };
                        }
                    }
                }
            }
            return null;
        }

        private Tilemap ParseTilemap(string mapdata)
        {
            try
            {
                var xRoot = new XmlRootAttribute();
                xRoot.ElementName = "map";

                var t = new XmlSerializer(typeof(Tilemap), xRoot);
                var map = t.Deserialize(mapdata.ToStream()) as Tilemap;
                return map;
            }
            catch (XmlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        private Tileset ParseTerrain(string data)
        {
            try
            {
                var xRoot = new XmlRootAttribute();
                xRoot.ElementName = "tileset";

                var t = new XmlSerializer(typeof(Tileset), xRoot);
                var terrain = t.Deserialize(data.ToStream()) as Tileset;
                return terrain;
            }
            catch (XmlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
