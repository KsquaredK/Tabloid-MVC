using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;


namespace TabloidMVC.Repositories
{
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IConfiguration config) : base(config) { }
        public List<Tag> GetAllTags()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @" 
                        SELECT Id, [Name] 
                        FROM Tag 
                        ORDER BY Name";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Tag> tags = new List<Tag>();

                    while (reader.Read())
                    {
                        Tag tag = new Tag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                        };
                        tags.Add(tag);
                    }

                    reader.Close();

                    return tags;
                }
            }
        }

        public void AddTag(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Tag ( Name )
                        OUTPUT INSERTED.ID
                        VALUES ( @Name )
                        ";

                    cmd.Parameters.AddWithValue("@Name", tag.Name);

                    tag.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
