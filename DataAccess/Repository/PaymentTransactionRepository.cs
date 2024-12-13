using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccsess.Reposetory.IReposetory;
using Models;

namespace DataAccess.Repository
{
    public class PaymentTransactionRepository : Repository<PaymentTransaction>, IPaymentTransactionRepository
    {
        ApplicationDbContext context;
        public PaymentTransactionRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
