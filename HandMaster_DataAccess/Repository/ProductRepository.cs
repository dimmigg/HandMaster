using HandMaster_DataAccess.Repository.IRepository;
using HandMaster_Models;
using HandMaster_Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace HandMaster_DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if(obj == WC.CategoryName)
            {
                return _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CategoryId.ToString()
                });
            }
            if(obj == WC.ApplicationTypeName)
            {
                return _db.ApplicationType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return null;
        }

        public void Update(Product obj)
        {
            _db.Product.Update(obj);
        }

    }
}
