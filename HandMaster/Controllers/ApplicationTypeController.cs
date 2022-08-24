using HandMaster.Data;
using HandMaster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace HandMaster.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ApplicationTypeController(ApplicationDbContext dp)
        {
            _db = dp;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objList = _db.ApplicationType;
            return View(objList);
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType app)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Add(app);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(app);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var app = _db.ApplicationType.Find(id);
            if (app == null)
                return NotFound();

            return View(app);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType app)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(app);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(app);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var cat = _db.ApplicationType.Find(id);
            if (cat == null)
                return NotFound();

            return View(cat);
        }
        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            var app = _db.ApplicationType.Find(id);
            if (app == null)
                return NotFound();


            _db.ApplicationType.Remove(app);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
