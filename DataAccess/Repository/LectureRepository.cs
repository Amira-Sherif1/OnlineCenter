using DataAccess;
using DataAccsess.Reposetory.IReposetory;
using Models;

namespace DataAccess.Repository
{
    public class LectureRepository:Repository<Lecture>,ILectureRepository
    {
        ApplicationDbContext context;
        public LectureRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
