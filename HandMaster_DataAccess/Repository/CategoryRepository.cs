using HandMaster_DataAccess.Repository.IRepository;
using HandMaster_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandMaster_DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category obj)
        {
            var objFromDb = FirstOrDefault(x => x.CategoryId == obj.CategoryId);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.DisplayOrder = obj.DisplayOrder;
            }
        }
    }
}
