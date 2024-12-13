using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Models;

namespace DataAccess.Repository
{
    public class StudentRepository:Repository<Student>,IStudentRepository
    {
        ApplicationDbContext context;
        public StudentRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }    
    }
}
