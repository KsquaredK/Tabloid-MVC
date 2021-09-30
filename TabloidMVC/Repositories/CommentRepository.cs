using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;


namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }
        public List<Comment> GetAllPublishedComments()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT c.Id, c.PostId, c.UserProfileId, c.Subject, C.Content, C.CreateDateTime
                       FROM Comment c
                              LEFT JOIN Category c ON p.CategoryId = c.id
                              LEFT JOIN UserProfile u ON c.UserProfileId = u.id
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                       WHERE c.PostId = @id";
                    var reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                    while (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = DbUtils.GetNullableString(reader, "Subject"),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                        };

                        comments.Add(comment);

                    }

                    reader.Close();

                    return comments;
                }
            }
        }

        public List<Comment> GetCommentByPostId(int PostId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT c.Id, c.PostId, c.UserProfileId, c.Subject, C.Content, C.CreateDateTime
                       FROM Comment c
                       INNER JOIN Post p ON p.Id = c.PostId
                       WHERE c.PostId = @id";

                    cmd.Parameters.AddWithValue("@id", PostId);
                    var reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                    while (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = DbUtils.GetNullableString(reader, "Subject"),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                        };

                        comments.Add(comment);

                    }

                    reader.Close();

                    return comments;
                }
            }
        }

        public List<Comment> GetUserCommentById(int id, int userProfileId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT c.Id, c.PostId, c.UserProfileId, c.Subject, C.Content, C.CreateDateTime
                       FROM Comment c
                       INNER JOIN UserProfile ON u.Id = u.PostId
                       WHERE c.PostId = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@userProfileId", userProfileId);
                    var reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                    while (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = DbUtils.GetNullableString(reader, "Subject"),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                        };

                        comments.Add(comment);

                    }

                    reader.Close();

                    return comments;
                }
            }
        }


        public void Add(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (
                            Id, PostId, UserProfileId, Subject, Content,
                            CreateDateTime )
                        OUTPUT INSERTED.ID
                        VALUES (
                            @Id, @PostId, @UserProfileId, @Subject, @Content,
                            @CreateDateTime )";
                    cmd.Parameters.AddWithValue("@Id", comment.Id);
                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@UserProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@Subject", DbUtils.ValueOrDBNull(comment.Subject));
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@CreateDateTime", comment.CreateDateTime);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }


        public void UpdateComment(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Comment
                            SET  
                                PostId = @postId, 
                                UserProfileId = @userProfileId, 
                                Subject = @subject, 
                                Content = @content,
                                CreateDateTime = @createDateTime
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@postId", comment.PostId);
                    cmd.Parameters.AddWithValue("@userProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@createDateTime", comment.CreateDateTime);
                    cmd.Parameters.AddWithValue("@id", comment.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteComment(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Comment
                            WHERE Id = @id
                        ";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Comment> GetAllUserComments(int userProfileId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT p.Id, p.Title, p.Content, 
                              p.ImageLocation AS HeaderImage,
                              p.CreateDateTime, p.PublishDateTime, p.IsApproved,
                              p.CategoryId, p.UserProfileId,
                              c.[Name] AS CategoryName,
                              u.FirstName, u.LastName, u.DisplayName, 
                              u.Email, u.CreateDateTime, u.ImageLocation AS AvatarImage,
                              u.UserTypeId, 
                              ut.[Name] AS UserTypeName
                         FROM Comment c
                        WHERE PublishDateTime < SYSDATETIME() AND p.UserProfileId = @userProfileId
                        ORDER BY p.CreateDateTime DESC;";

                    cmd.Parameters.AddWithValue("@userProfileId", userProfileId);
                    var reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                    while (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = DbUtils.GetNullableString(reader, "Subject"),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                        };

                        comments.Add(comment);

                    }

                    reader.Close();

                    return comments;
                }
            }
        }
    }
}
