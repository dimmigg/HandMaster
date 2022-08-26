using HandMaster_DataAccess;
using HandMaster_DataAccess.Repository.IRepository;
using HandMaster_Models;
using HandMaster_Models.ViewModels;
using HandMaster_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HandMaster.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductRepository _prodRepo;
        private readonly IApplicationUserRepository _userRepo;
        private readonly IInquiryHeaderRepository _inqHRepo;
        private readonly IInquiryDetailRepository _inqDRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }
        public CartController(          
            IWebHostEnvironment webHostEnvironment, 
            IEmailSender emailSender,
            IProductRepository prodRepo,
            IInquiryHeaderRepository inqHRepo, 
            IInquiryDetailRepository inqDRepo,
            IApplicationUserRepository userRepo)
        {
            _prodRepo = prodRepo;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _inqHRepo = inqHRepo;
            _inqDRepo = inqDRepo;
            _userRepo = userRepo;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartsList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> prodList = _prodRepo.GetAll(x => prodInCart.Contains(x.Id));
            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index" )]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartsList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> prodList = _prodRepo.GetAll(x => prodInCart.Contains(x.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _userRepo.FirstOrDefault(x => x.Id == claim.Value),
                ProductList = prodList.ToList()
            };
            return View(ProductUserVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var pathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString()
                + "Inquiry.html";
            var subject = "Новый заказ";
            string htmlBody = "";
            using(StreamReader sr = System.IO.File.OpenText(pathToTemplate))
            {
                htmlBody = sr.ReadToEnd();
            }

            StringBuilder productLisrSB = new StringBuilder();
            foreach (var product in ProductUserVM.ProductList)
            {
                productLisrSB.Append($" - Name: {product.Name} <span style = 'font-size:14px;'> (ID: {product.Id})</span></br>");
            }

            string messageBody = string.Format(
                htmlBody,
                ProductUserVM.ApplicationUser.FullName,
                ProductUserVM.ApplicationUser.Email,
                ProductUserVM.ApplicationUser.PhoneNumber,
                productLisrSB.ToString());

            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);

            InquiryHeader inquiryHeader = new InquiryHeader()
            {
                ApplicationUserId = claim.Value,
                FullName = ProductUserVM.ApplicationUser.FullName,
                Email = ProductUserVM.ApplicationUser.Email,
                PhoneNumber = ProductUserVM.ApplicationUser.PhoneNumber,
                InquiryDate = DateTime.Now
            };

            _inqHRepo.Add(inquiryHeader);
            _inqHRepo.Save();

            foreach (var prod in ProductUserVM.ProductList)
            {
                InquiryDetail inquiryDetail = new InquiryDetail()
                {
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId = prod.Id
                };
                _inqDRepo.Add(inquiryDetail);               
            }
            _inqDRepo.Save();

            TempData[WC.Success] = "Заказ успешно добавлен";
            return RedirectToAction(nameof(InquaryConfirmation));
        }

        public IActionResult InquaryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartsList.Remove(shoppingCartsList.FirstOrDefault(x => x.ProductId == id));

            HttpContext.Session.Set(WC.SessionCart, shoppingCartsList);
            TempData[WC.Success] = "Товар удален из корзины";
            return RedirectToAction(nameof(Index));
        }
    }
}
