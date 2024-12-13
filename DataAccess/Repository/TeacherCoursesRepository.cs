using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Repository
{
    public class TeacherCoursesRepository : Repository<TeacherCourse>,ITeacherCoursesRepository
    {
        ApplicationDbContext dbcontext;
        public TeacherCoursesRepository(ApplicationDbContext dbcontext) :base(dbcontext) 
        { 
            this.dbcontext = dbcontext;
        }
    }
}
