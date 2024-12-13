using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Reposetory
{
    public class CartRepository: Repository<Cart> , ICartRepository
    {
        ApplicationDbContext dbContext;
        public CartRepository(ApplicationDbContext dbContext): base(dbContext) 
        {
            this.dbContext = dbContext;
        }
    }
}
