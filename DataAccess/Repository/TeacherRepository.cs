using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Repository
{
    public class TeacherRepository : Repository<Teacher>,ITeacherRepository
    {
        ApplicationDbContext dbcontext;
        public TeacherRepository(ApplicationDbContext dbcontext) :base(dbcontext) 
        { 
            this.dbcontext = dbcontext;
        }
    }
}
