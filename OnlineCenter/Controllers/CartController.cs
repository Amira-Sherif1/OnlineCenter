using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using DataAccsess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Stripe.Checkout;
using System.Security.Claims;
using Utilities;

namespace OnlineCenter.Controllers
{
    public class CartController : Controller
    {
        public ILectureRepository lectureRepository { get; set; }
        public IBookRepository bookRepository { get; set; }
        public IPaymentTransactionRepository paymentTransactionRepository { get; set; }
        public IBookPaymentRepository BookPaymentRepository { get; set; }
        public UserManager<IdentityUser> UserManager { get; set; }
        public CartController(IBookPaymentRepository BookPaymentRepository, IBookRepository bookRepository,
            UserManager<IdentityUser> UserManager, ILectureRepository lectureRepository, IPaymentTransactionRepository paymentTransactionRepository)
        {
            this.lectureRepository = lectureRepository;
            this.paymentTransactionRepository = paymentTransactionRepository;
            this.UserManager= UserManager;
            this.bookRepository= bookRepository;
            this.BookPaymentRepository = BookPaymentRepository;
        }
        public IActionResult LecturePay(int LectureId)
        {
            var lecture = lectureRepository.GetOne(expresion: e => e.Id == LectureId);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = lecture.Title,
                                // Description = model.ProductDescription, // Uncomment if needed
                            },
                            UnitAmount = (long)lecture.Price*100,
                        },
                        Quantity = 1,
                    }
                },
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success?session_id={{CHECKOUT_SESSION_ID}}&lecture_id={LectureId}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel?lecture_id={LectureId}",
            };
            var userid = UserManager.GetUserId(User);


            var service = new SessionService();
            var session = service.Create(options);
            var transaction = new PaymentTransaction
            {
                LectureId = LectureId,
                UserId = userid,
                StripeSessionId = session.Id,
                Amount = lecture.Price,
                Currency = "egp",
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            };
            paymentTransactionRepository.Add(transaction);
            paymentTransactionRepository.Save();
            return Redirect(session.Url);
        }
        public IActionResult BookPay(int BookId)
        {
            var book = bookRepository.GetOne(expresion: e => e.Id == BookId);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = book.Name,
                                // Description = model.ProductDescription, // Uncomment if needed
                            },
                            UnitAmount = (long)book.Price*100,
                        },
                        Quantity = 1,
                    }
                },
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/successbook?session_id={{CHECKOUT_SESSION_ID}}&BookId={BookId}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancelbook?BookId={BookId}",
            };
            var userid = UserManager.GetUserId(User);


            var service = new SessionService();
            var session = service.Create(options);
            var transaction = new BookPayment
            {
                BookId = BookId,
                UserId = userid,
                StripeSessionId = session.Id,
                Amount = book.Price,
                Currency = "egp",
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            };
            BookPaymentRepository.Add(transaction);
            BookPaymentRepository.Save();
            return Redirect(session.Url);
        }


    }

  
    

}