using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>,IApplicationUserRepository
    {
        ApplicationDbContext dbcontext;
        public ApplicationUserRepository(ApplicationDbContext dbcontext) :base(dbcontext) 
        { 
            this.dbcontext = dbcontext;
        }
    }
}
