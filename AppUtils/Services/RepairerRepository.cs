using AppUtils.Data;
using AppUtils.Services.IRepositories;

namespace AppUtils.Services
{
    public class RepairerRepository : RepositoryBase<Repairer>, IRepairerRepository
    {
        public RepairerRepository(DbAppUtils db) : base(db)
        {
        }
    }
}
