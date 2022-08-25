using HandMaster_DataAccess;
using HandMaster_DataAccess.Repository.IRepository;
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
        private readonly ICategoryRepository _catRepo;
        public CategoryController(ICategoryRepository catRepo)
        {
            _catRepo = catRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _catRepo.GetAll();
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
                _catRepo.Add(cat);
                _catRepo.Save();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var cat = _catRepo.Find(id.GetValueOrDefault());
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
                _catRepo.Update(cat);
                _catRepo.Save();
                return RedirectToAction("Index");
            }
            return View(cat);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var cat = _catRepo.Find(id.GetValueOrDefault());
            if (cat == null)
                return NotFound();

            return View(cat);
        }
        [HttpPost]
        public IActionResult DeletePost(int? categoryId)
        {
            var cat = _catRepo.Find(categoryId.GetValueOrDefault());
            if (cat == null)
                return NotFound();
            _catRepo.Remove(cat);
            _catRepo.Save();
            return RedirectToAction("Index");
        }
    }
}
