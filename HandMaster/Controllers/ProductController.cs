using HandMaster_DataAccess;
using HandMaster_DataAccess.Repository.IRepository;
using HandMaster_Models;
using HandMaster_Models.ViewModels;
using HandMaster_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HandMaster.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _prodRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository prodRepo, IWebHostEnvironment webHostEnvironment)
        {
            _prodRepo = prodRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _prodRepo.GetAll(includeProperties: "Category,ApplicationType");

            //foreach(var obj in objList)
            //{
            //    obj.Category = _db.Category.FirstOrDefault(x => x.CategoryId == obj.CategoryId);
            //    obj.ApplicationType = _db.ApplicationType.FirstOrDefault(x => x.Id == obj.ApplicationTypeId);
            //}

            return View(objList);
        }


        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.CategoryId.ToString()
            //});

            //ViewBag.CategoryDropDown = CategoryDropDown;

            //Product product = new Product();

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _prodRepo.GetAllDropDownList(WC.CategoryName),
                ApplicationTypeSelectList = _prodRepo.GetAllDropDownList(WC.ApplicationTypeName),
            };
            if (id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _prodRepo.Find(id.GetValueOrDefault());
                if(productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if(productVM.Product.Id == 0)
                {
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    _prodRepo.Add(productVM.Product);
                    
                }
                else
                {
                    var objFromDb = _prodRepo.FirstOrDefault(x => x.Id == productVM.Product.Id, isTracking: false);
                    if(files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);
                        if(System.IO.File.Exists(oldFile))
                            System.IO.File.Delete(oldFile);

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }
                    _prodRepo.Update(productVM.Product);
                }
                _prodRepo.Save();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _prodRepo.GetAllDropDownList(WC.CategoryName);
            productVM.ApplicationTypeSelectList = _prodRepo.GetAllDropDownList(WC.ApplicationTypeName);
            return View(productVM);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var product = _prodRepo.FirstOrDefault(x => x.Id == id, includeProperties:"Category,ApplicationType");

            if (product == null)
                return NotFound();

            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var product = _prodRepo.Find(id.GetValueOrDefault());
            if (product == null)
                return NotFound();

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, product.Image);
            if (System.IO.File.Exists(oldFile))
                System.IO.File.Delete(oldFile);

            _prodRepo.Remove(product);
            _prodRepo.Save();
            return RedirectToAction("Index");
        }

    }
}
