using HandMaster_DataAccess.Repository.IRepository;
using HandMaster_Models;

namespace HandMaster_DataAccess.Repository
{
    public class InquiryDetailRepository : Repository<InquiryDetail>, IInquiryDetailRepository
    {
        private readonly ApplicationDbContext _db;

        public InquiryDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InquiryDetail obj)
        {
            _db.InquiryDetail.Update(obj);
        }
    }
}
