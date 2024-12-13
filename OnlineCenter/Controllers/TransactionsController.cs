using DataAccsess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCenter.Controllers
{
    public class TransactionsController : Controller
    {
        public IPaymentTransactionRepository paymentTransactionRepository { get; set; }
        public UserManager<IdentityUser> userManager { get; set; }
        public TransactionsController(IPaymentTransactionRepository paymentTransactionRepository, UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
            this.paymentTransactionRepository = paymentTransactionRepository;
        }
        public IActionResult LectureTranactions()
        {
            var useid = userManager.GetUserId(User);
            if (User.IsInRole("Admin"))
            {
                var transactions = paymentTransactionRepository.GetAll([e=>e.Lecture]);
                return View(transactions.ToList());
            }
            else
            {
                
                //// logic for teacher 
            }
            return View();
        }
    }
}
