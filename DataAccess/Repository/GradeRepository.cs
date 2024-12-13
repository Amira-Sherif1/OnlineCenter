using DataAccess;
using DataAccess.Reposetory.IReposetory;
using Models;

namespace DataAccess.Repository
{
    public class GradeRepository : Repository<Grade>, IGradeRepository
    {
        ApplicationDbContext context;
        public GradeRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
