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
        public IPaymentTransactionRepository paymentTransactionRepository { get; set; }
        public UserManager<IdentityUser> UserManager { get; set; }
        public CartController(UserManager<IdentityUser> UserManager, ILectureRepository lectureRepository, IPaymentTransactionRepository paymentTransactionRepository)
        {
            this.lectureRepository = lectureRepository;
            this.paymentTransactionRepository = paymentTransactionRepository;
            this.UserManager= UserManager;
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
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancellecture_id={LectureId}",
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


    }

    // Controllers/PaymentController.cs
    

}