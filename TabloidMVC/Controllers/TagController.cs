using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;



namespace TabloidMVC.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        


        public TagController(ITagRepository tagRepository, IPostRepository postRepository)
        { 
            _tagRepository = tagRepository;
            
        }
        // GET: TagController
        [Authorize]
        public ActionResult Index()
        {
            List<Tag> tags = _tagRepository.GetAllTags();

            return View(tags);
        }

        //// GET: CategoryController/Details/5
        //public ActionResult Details(int id)
        //{

        //    return View();
        //}

        // GET: TagController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            try
            {
                _tagRepository.AddTag(tag);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                
                return View(tag);
            }
        }

        // GET: CategoryController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);
            
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: CategoryController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepository.EditTag(tag);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
               
                return View(tag);
            }
        }

        // GET: CategoryController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);
            return View(tag);
        }

        // POST: CategoryController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tag tag)
        {
            try
            {
                tag = _tagRepository.GetTagById(id);
                _tagRepository.DeleteTag(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                
                return View(tag);
            }
        }
        // /tag/assigntagtopost/id
        //public ActionResult AssignTagToPost(int id)
        //{
        //    var post = _postRepository.GetUserPostById(id, GetCurrentUserProfileId());

        //    if (post == null)
        //    {
        //        return StatusCode(403);
        //    }
           
        //    var tags = _tagRepository.GetAllTags();
        //    return View(tags);

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AssignTagToPost(Tag tag, Post post)
        //{
        //    var tags = _tagRepository.GetAllTags();
        //    try
        //    {
        //        _tagRepository.AddTagToPost(tag, post);
        //        return RedirectToAction("Index");
        //    }

        //    catch (Exception ex)
        //    {
        //        return View(tags);
        //    }
        //}

        //private int GetCurrentUserProfileId()
        //{
        //    string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    return int.Parse(id);
        //}
    }
}