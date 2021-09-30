using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using System;
using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        public IActionResult Index(int id)
        {
            CommentViewModel vm = new CommentViewModel();
            vm.CommentList = _commentRepository.GetCommentByPostId(id);
            vm.PostId = id;
            return View(vm);
        }

        public IActionResult Details(int id)
        {
            var comment = _commentRepository.GetCommentByPostId(id);
            if (comment == null)
            {
                int userId = GetCurrentUserProfileId();
                comment = _commentRepository.GetUserCommentById(id, userId);
                if (comment == null)
                {
                    return NotFound();
                }
            }
            return View(comment);
        }

        public IActionResult Create()
        {
            var vm = new CommentCreateViewModel();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(CommentCreateViewModel vm)
        {
            try
            {
                vm.Comment.CreateDateTime = DateAndTime.Now;
                vm.Comment.UserProfileId = GetCurrentUserProfileId();

                _commentRepository.Add(vm.Comment);

                return RedirectToAction("Details", new { id = vm.Comment.Id });
            }
            catch
            {
                return View(vm);
            }
        }
        public IActionResult Delete(int id)
        {
            List<Comment> comment = _commentRepository.GetCommentByPostId(id);

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Comment comment)
        {
            try
            {
                _commentRepository.DeleteComment(id);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(comment);
            }
        }

        public IActionResult UserComments()
        {
            int userProfileId = GetCurrentUserProfileId();
            var comments = _commentRepository.GetAllUserComments(userProfileId);

            return View(comments);
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        public IActionResult Edit(int id)
        {
            int userProfileId = GetCurrentUserProfileId();
            List<Comment> comment = _commentRepository.GetUserCommentById(id, userProfileId);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Comment comment)
        {
            try
            {
                _commentRepository.UpdateComment(comment);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(comment);
            }
        }
    }
}
