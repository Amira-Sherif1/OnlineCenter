using DataAccess.Reposetory.IReposetory;
using DataAccsess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCenter.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class TransactionsController : Controller
    {

        public IPaymentTransactionRepository paymentTransactionRepository { get; set; }
        public IBookPaymentRepository bookPaymentRepository { get; set; }
        public UserManager<IdentityUser> userManager { get; set; }
        public TransactionsController(IBookPaymentRepository bookPaymentRepository ,IPaymentTransactionRepository paymentTransactionRepository, UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
            this.paymentTransactionRepository = paymentTransactionRepository;
            this.bookPaymentRepository = bookPaymentRepository;
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
        public IActionResult BookTransactions()
        {
            var useid = userManager.GetUserId(User);
            if (User.IsInRole("Admin"))
            {
                var transactions = bookPaymentRepository.GetAll([e => e.Book]);
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
