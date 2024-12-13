using DataAccess.Reposetory.IReposetory;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AssistantInRepository : Repository<AssistIn>, IAssestInReposetory
    {
        ApplicationDbContext dbContextl; 
        public AssistantInRepository(ApplicationDbContext dbContextl) : base(dbContextl)
        {
            this.dbContextl = dbContextl;
        }
    }
}
