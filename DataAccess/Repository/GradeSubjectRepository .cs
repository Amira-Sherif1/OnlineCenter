using DataAccess;
using DataAccess.Reposetory.IReposetory;
using Models;

namespace DataAccess.Repository
{
    public class GradeSubjectRepository : Repository<GradeSubject>, IGradeSubjectRepository
    {
        ApplicationDbContext context;
        public GradeSubjectRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
