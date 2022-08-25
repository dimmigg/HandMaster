using HandMaster_Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace HandMaster_DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);

        IEnumerable<SelectListItem> GetAllDropDownList(string obj);
    }
}
