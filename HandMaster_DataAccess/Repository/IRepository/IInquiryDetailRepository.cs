using HandMaster_Models;

namespace HandMaster_DataAccess.Repository.IRepository
{
    public interface IInquiryDetailRepository : IRepository<InquiryDetail>
    {
        void Update(InquiryDetail obj);
    }
}
