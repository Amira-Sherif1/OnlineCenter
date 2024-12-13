using Models;
using DataAccess.Reposetory.IReposetory;
using DataAccess;
using DataAccess.Repository;

namespace DataAccess.Repository
{
    public class SubjectRepository : Repository<Subject> , ISubjectRepsitory
    {
        ApplicationDbContext context;
        public SubjectRepository(ApplicationDbContext context) : base(context) 
        { 
            this.context = context;
        }

    }
}
