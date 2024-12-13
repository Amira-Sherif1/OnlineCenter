using DataAccess.Reposetory.IReposetory;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AssistantRepository : Repository<Assistant>, IAssistantRepository
    {
        ApplicationDbContext dbContextl; 
        public AssistantRepository(ApplicationDbContext dbContextl) : base(dbContextl)
        {
            this.dbContextl = dbContextl;
        }
    }
}
