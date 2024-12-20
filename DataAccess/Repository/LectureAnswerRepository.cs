using DataAccess;
using DataAccsess.Reposetory.IReposetory;
using Models;

namespace DataAccess.Repository
{
    public class LectureAnswerRepository:Repository<LectureAnswer>,ILectureAnswerRepository
    {
        ApplicationDbContext context;
        public LectureAnswerRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
