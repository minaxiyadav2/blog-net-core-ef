using Blog.Comments;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Database.Repository
{
    public class Repository : IRepository
    {
        private AppDbContext _ctx;

        public Repository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public void AddPost(Post post)
        {
            _ctx.Posts.Add(post);
        }

        public List<Post> GetAllPosts()
        {
            return _ctx.Posts
                .ToList();
        }

        public IndexViewModel GetAllPosts(int pageNumber)
        {
            int pageSize = 5;
            int skipAmount = pageSize * (pageNumber - 1);
            int postsCount = _ctx.Posts.Count();

            return new IndexViewModel
            {
                PageNumber = pageNumber,
                NextPage = postsCount > skipAmount + pageSize,
                Posts = _ctx.Posts
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList()
            };
        }

        public IndexViewModel GetAllPosts(int pageNumber, string category)
        {
            Func<Post, bool> InCategory = (post) => { return post.Category.ToLower().Equals(category.ToLower()); };

            int pageSize = 5;
            int skipAmount = pageSize * (pageNumber - 1);

            var query = _ctx.Posts.AsQueryable();
               

            if (!String.IsNullOrEmpty(category))
                query = query.Where(x => InCategory(x));

            int postsCount = query.Count();

            return new IndexViewModel
            {
                PageNumber = pageNumber,
                PageCount= (int) Math.Ceiling((double)postsCount / pageSize),
                NextPage = postsCount > skipAmount + pageSize,
                Category = category,
                Posts = query
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList()
            };
        }

        public List<Post> GetAllPostsCategory(string category)
        {
            Func<Post, bool> InCategory = (post) => { return post.Category.ToLower().Equals(category.ToLower()); };
            return _ctx.Posts
                .Where(x => InCategory(x))
                .ToList();
        }

        public Post GetPost(int id)
        {
            return _ctx.Posts
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.SubComments)
                .FirstOrDefault(p => p.Id == id);
        }

        public void RemovePost(int id)
        {
            _ctx.Posts.Remove(GetPost(id));
        }            

        public void UpdatePost(Post post)
        {
            _ctx.Posts.Update(post);
        }

        public async Task<bool> SaveChangesAsync()
        {
            if(await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public void AddSubComment(SubComment comment)
        {
            _ctx.SubComments.Add(comment);
        }
    }
}
