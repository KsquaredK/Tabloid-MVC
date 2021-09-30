using System.Collections.Generic;
using TabloidMVC.Controllers;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        void Add(Comment comment);
        public List<Comment> GetAllPublishedComments();
        public List<Comment> GetUserCommentById(int id, int userProfileId);
        void UpdateComment(Comment comment);
        void DeleteComment(int Id);

        List<Comment> GetAllUserComments(int userProfileId);
        public List<Comment> GetCommentByPostId(int PostId);
    }
}