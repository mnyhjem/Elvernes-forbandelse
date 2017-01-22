using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Model.Items;

namespace ElvenCurse.Core.Services
{
    public class ItemsService : IItemsService
    {
        private readonly string _connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public int SaveItem(Item item)
        {
            if (string.IsNullOrWhiteSpace(item.Imagepath))
            {
                item.Imagepath = "";
            }
            if (string.IsNullOrWhiteSpace(item.Description))
            {
                item.Description = "";
            }

            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SaveItem";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("Id", item.Id));
                    cmd.Parameters.Add(new SqlParameter("Category", item.Category));
                    cmd.Parameters.Add(new SqlParameter("Type", item.Type));
                    cmd.Parameters.Add(new SqlParameter("Name", item.Name));
                    cmd.Parameters.Add(new SqlParameter("Description", item.Description));
                    cmd.Parameters.Add(new SqlParameter("ImagePath", item.Imagepath));

                    var r = cmd.ExecuteScalar();
                    if (r == null)
                    {
                        return item.Id;
                    }
                    else
                    {
                        return (int)(decimal)r;
                    }
                }
            }
        }

        public List<Item> GetItems()
        {
            var items = new List<Item>();
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "GetItems";
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            items.Add(MapItem(dr));
                        }
                    }
                }
            }
            return items;
        }


        public Item GetItem(int id)
        {
            using (var con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "GetItem";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("id", id));
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return MapItem(dr);
                        }
                    }
                }
            }
            return null;
        }

        private Item MapItem(SqlDataReader dr)
        {
            var item = new Item
            {
                Id = (int) dr["Id"],
                Category = (Itemcategory) dr["Category"],
                Type = (int) dr["Type"],
                Name = (string) dr["Name"],
                Description = (string) dr["description"],
                Imagepath = (string) dr["imagepath"]
            };

            return item;
        }

    }
}
