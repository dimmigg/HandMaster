using HandMaster_DataAccess.Repository.IRepository;
using HandMaster_Models;

namespace HandMaster_DataAccess.Repository
{
    public class ApplicationTypeRepository : Repository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ApplicationType obj)
        {
            var objFromDb = FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
            }
        }
    }
}
