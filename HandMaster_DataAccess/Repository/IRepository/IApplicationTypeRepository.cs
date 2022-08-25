using HandMaster_Models;

namespace HandMaster_DataAccess.Repository.IRepository
{
    public interface IApplicationTypeRepository : IRepository<ApplicationType>
    {
        void Update(ApplicationType obj);
    }
}
