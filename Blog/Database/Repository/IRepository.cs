using Blog.Comments;
using Blog.Models;
using Blog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Database.Repository
{
    public interface IRepository
    {
        Post GetPost(int id);
        List<Post> GetAllPosts();
        IndexViewModel GetAllPosts(int pageNumber);
        IndexViewModel GetAllPosts(int pageNumber, string category);
        List<Post> GetAllPostsCategory(string category);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void RemovePost(int id);

        void AddSubComment(SubComment comment);

        Task<bool> SaveChangesAsync();
    }
}
