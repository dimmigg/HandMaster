using HandMaster_Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace HandMaster_DataAccess.Repository.IRepository
{
    public interface IInquiryHeaderRepository : IRepository<InquiryHeader>
    {
        void Update(InquiryHeader obj);
    }
}
