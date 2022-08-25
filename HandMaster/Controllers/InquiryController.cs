using HandMaster_DataAccess.Repository.IRepository;
using HandMaster_Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HandMaster.Controllers
{
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inqHRepo;
        private readonly IInquiryDetailRepository _inqDRepo;

        [BindProperty]
        public InquiryVM InquiryVM { get; set; }
        public InquiryController(IInquiryHeaderRepository inqHRepo, IInquiryDetailRepository inqDRepo)
        {
            _inqHRepo = inqHRepo;
            _inqDRepo = inqDRepo;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Details(int id)
        {
            InquiryVM = new InquiryVM()
            {
                InquiryHeader = _inqHRepo.FirstOrDefault(x => x.Id == id),
                InquiryDetail = _inqDRepo.GetAll(x => x.InquiryHeaderId == id, includeProperties:"Product")
            };

            return View(InquiryVM);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = _inqHRepo.GetAll() });
        }
        #endregion
    }
}
