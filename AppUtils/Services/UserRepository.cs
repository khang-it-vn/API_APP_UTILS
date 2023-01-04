using AppUtils.Data;
using AppUtils.Services.IRepositories;

namespace AppUtils.Services
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DbAppUtils db) : base(db)
        {
        }
    }
}
