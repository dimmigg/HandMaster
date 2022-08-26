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
    public class ApplicationTypeController : Controller
    {
        private readonly IApplicationTypeRepository _appTypeRepo;
        public ApplicationTypeController(IApplicationTypeRepository appTypeRepo)
        {
            _appTypeRepo = appTypeRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objList = _appTypeRepo.GetAll();
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
                _appTypeRepo.Add(app);
                _appTypeRepo.Save();
                TempData[WC.Success] = "Апптайп успешно добавлен";
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "Произошла непредвиденная ошибка";
            return View(app);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var app = _appTypeRepo.Find(id.GetValueOrDefault());
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
                _appTypeRepo.Update(app);
                _appTypeRepo.Save();
                TempData[WC.Success] = "Апптайп успешно отредактирован";
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "Произошла непредвиденная ошибка";
            return View(app);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var cat = _appTypeRepo.Find(id.GetValueOrDefault());
            if (cat == null)
                return NotFound();

            return View(cat);
        }
        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            var app = _appTypeRepo.Find(id.GetValueOrDefault());
            if (app == null)
                return NotFound();


            _appTypeRepo.Remove(app);
            _appTypeRepo.Save();
            TempData[WC.Success] = "Апптайп успешно удален";
            return RedirectToAction("Index");
        }

    }
}
