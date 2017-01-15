using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ElvenCurse.Core.Engines.Messagequeue;
using ElvenCurse.Core.Interfaces;

namespace ElvenCurse.Core.Services
{
    public class MessagequeueService :IMessagequeueService
    {
        private readonly string _connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Queueelement> GetMessagequeue()
        {
            var list = new List<Queueelement>();
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "GetMessageQueue";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var e = new Queueelement
                            {
                                Id = (int)dr["id"],
                                Type = (Messagetype)dr["messagetype"],
                                Parameters = (string)dr["parameters"],
                                Queuetime = (DateTime)dr["queuetime"]
                            };
                            list.Add(e);
                        }
                    }
                }
            }
            return list;
        }

        public void Push(Queueelement element)
        {
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "QueueMessagequeueElement";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("messagetype", element.Type));
                    cmd.Parameters.Add(new SqlParameter("parameters", element.Parameters));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
