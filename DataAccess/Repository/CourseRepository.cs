using DataAccess;
using DataAccess.Reposetory.IReposetory;
using Models;

namespace DataAccess.Repository
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        ApplicationDbContext context;
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
