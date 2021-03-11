using Blog.Database.FileManager;
using Blog.Database.Repository;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PanelController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public PanelController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }

        public IActionResult Index()
        {
            var posts = _repo.GetAllPosts();
            return View(posts);
        }

        public IActionResult Post(int id)
        {
            var post = _repo.GetPost(id);
            return View(post);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View(new PostViewModel());
            else
            {
                var post = _repo.GetPost((int)id);
                return View(new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    Description = post.Description,
                    Tags = post.Tags,
                    Category = post.Category,
                    CurrentImage = post.Image,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel pvm)
        {
            var post = new Post
            {
                Id = pvm.Id,
                Title = pvm.Title,
                Body = pvm.Body,
                Description = pvm.Description,
                Tags = pvm.Tags,
                Category = pvm.Category,
            };

            if (pvm.Image == null)
                post.Image = pvm.CurrentImage;
            else 
            {
                if (!string.IsNullOrEmpty(pvm.CurrentImage))
                    _fileManager.RemoveImage(pvm.CurrentImage);

                post.Image = await _fileManager.SaveImage(pvm.Image);   // handle image
            }

            Console.WriteLine(post.Id);
            if (post.Id > 0)
                _repo.UpdatePost(post);
            else
                _repo.AddPost(post);

            if (await _repo.SaveChangesAsync())
                return RedirectToAction("Index");
            else
                return View(post);

        }


        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            _repo.RemovePost(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
