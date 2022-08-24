using HandMaster_DataAccess;
using HandMaster_Models;
using HandMaster_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace HandMaster.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext dp)
        {
            _db = dp;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _db.Category;
            return View(objList);
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category cat)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(cat);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var cat = _db.Category.Find(id);
            if (cat == null)
                return NotFound();

            return View(cat);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(cat);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var cat = _db.Category.Find(id);
            if (cat == null)
                return NotFound();

            return View(cat);
        }
        [HttpPost]
        public IActionResult DeletePost(int? categoryId)
        {
            var cat = _db.Category.Find(categoryId);
            if (cat == null)
                return NotFound();


            _db.Category.Remove(cat);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
