using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Repository
{
    public class TeacherBooksRepository : Repository<TeacherBook>,ITeacherBooksRepository
    {
        ApplicationDbContext dbcontext;
        public TeacherBooksRepository(ApplicationDbContext dbcontext) :base(dbcontext) 
        { 
            this.dbcontext = dbcontext;
        }
    }
}
