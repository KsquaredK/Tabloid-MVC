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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        // GET: CategoryController
        [Authorize]
        public ActionResult Index()
        {
            var categories = _categoryRepository.GetAll();

            return View(categories);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: CategoryController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                _categoryRepository.AddCategory(category);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(category);
            }
        }

        // GET: CategoryController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                _categoryRepository.UpdateCategory(category);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(category);
            }
        }

        // GET: CategoryController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Category category = _categoryRepository.GetCategoryById(id);
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category category)
        {
            try
            {
                category = _categoryRepository.GetCategoryById(id);
                _categoryRepository.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(category);
            }
        }
    }
}
