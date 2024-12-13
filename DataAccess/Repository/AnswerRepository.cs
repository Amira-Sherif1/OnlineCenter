using DataAccess.Reposetory.IReposetory;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository

    {
        ApplicationDbContext context;
        public AnswerRepository(ApplicationDbContext dbContext):base(dbContext) 
        {
            this.context = dbContext;
        }
    }
}
