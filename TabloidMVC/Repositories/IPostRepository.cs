using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void AddPost(Post post);

        void EditPost(Post post);

        void DeletePost(int id);
        List<Post> GetAllPublishedPosts();
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        List<Post> GetAllPostsFromCurrentUser(int userProfileId);

    }
}