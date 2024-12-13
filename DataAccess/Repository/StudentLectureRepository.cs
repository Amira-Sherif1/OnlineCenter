using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Models;

namespace DataAccess.Repository
{
    public class StudentLectureRepository : Repository<StudentLecture>,IStudentLectureRepository
    {
        ApplicationDbContext context;
        public StudentLectureRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }    
    }
}
