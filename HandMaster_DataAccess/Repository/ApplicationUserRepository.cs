using HandMaster_DataAccess.Repository.IRepository;
using HandMaster_Models;

namespace HandMaster_DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        //public void Update(ApplicationType obj)
        //{
        //    var objFromDb = FirstOrDefault(x => x.Id == obj.Id);
        //    if (objFromDb != null)
        //    {
        //        objFromDb.Name = obj.Name;
        //    }
        //}
    }
}
