using DataAccess;
using DataAccess.Reposetory.IReposetory;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        ApplicationDbContext dbContext;
        public BookRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
