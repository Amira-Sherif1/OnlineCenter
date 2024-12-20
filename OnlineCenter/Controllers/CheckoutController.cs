﻿using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccsess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewData;
using System.Security.Claims;
using Utilities;

namespace OnlineCenter.Controllers
{
    // Controllers/CheckoutController.cs
    public class CheckoutController : Controller
    {
        // private readonly ApplicationDbContext _context;
        public UserManager<IdentityUser> UserManager { get; set; }
        public IStudentLectureRepository studentLectureRepository { get; set; }
        public IPaymentTransactionRepository paymentTransactionRepository { get; set; }
        public IBookPaymentRepository BookPaymentRepository { get; set; }
        private readonly IStripeService _stripeService;
        public ILectureRepository lectureRepository { get; set; }
        public IBookRepository BookRepository { get; set; }
        public ICartRepository cartRepository { get; set; }

        public CheckoutController(ICartRepository cartRepository ,IBookRepository BookRepository,IBookPaymentRepository BookPaymentRepository ,ILectureRepository lectureRepository ,IStudentLectureRepository studentLectureRepository, UserManager<IdentityUser> UserManager, IPaymentTransactionRepository paymentTransactionRepository, IStripeService stripeService)
        {
           this.paymentTransactionRepository = paymentTransactionRepository;
            _stripeService = stripeService;
            this.UserManager = UserManager;
            this.studentLectureRepository = studentLectureRepository;
            this.lectureRepository = lectureRepository;
            this.BookPaymentRepository= BookPaymentRepository;
            this.BookRepository = BookRepository;
            this.cartRepository = cartRepository;
        }

        public async Task<IActionResult> Success(string session_id, int lecture_id)
        {
            var session = await _stripeService.GetSessionAsync(session_id);

            if (session.PaymentStatus == "paid")
            {
                var transaction = paymentTransactionRepository.GetOne(expresion:e=>e.StripeSessionId == session_id);

                if (transaction != null)
                {
                    transaction.Status = "completed";
                    paymentTransactionRepository.Save();
                    // await _context.SaveChangesAsync();

                    // Add user access to the lecture
                    var lecture = lectureRepository.GetOne(expresion:e=>e.Id==lecture_id);
                    var userLecture = new StudentLecture
                    {
                        StudentId = UserManager.GetUserId(User),
                        LectureId = lecture_id,
                        status = true
                    };
                    studentLectureRepository.Add(userLecture);
                    studentLectureRepository.Save();
                   
                }
            }

            return View(new CheckoutSuccessViewModel
            {
                LectureId = lecture_id
            });
        }

        public IActionResult Cancel(int lecture_id)
        {
            return View(new CheckoutCancelViewModel
            {
                LectureId = lecture_id
            });
        }



        public async Task<IActionResult> successbook(string session_id, int BookId)
        {
            var session = await _stripeService.GetSessionAsync(session_id);

            if (session.PaymentStatus == "paid")
            {
                var transaction = BookPaymentRepository.GetOne(expresion: e => e.StripeSessionId == session_id);

                if (transaction != null)
                {
                    transaction.Status = "completed";
                    BookPaymentRepository.Save();
                    // await _context.SaveChangesAsync();

                    // Add user access to the lecture
                    var Book = BookRepository.GetOne(expresion: e => e.Id == BookId);
                    var userbook = new Cart
                    {
                        StudentId = UserManager.GetUserId(User),
                        BookId = BookId,
                        status = true
                    };
                    cartRepository.Add(userbook);
                    cartRepository.Save();

                }
            }

            return View(new CheckoutSuccessBookViewModel
            {
                BookId = BookId
            });
        }

        public IActionResult cancelbook(int BookId)
        {
            return View(new CheckoutCancelBookViewModel
            {
                BookId = BookId
            });
        }
    }

    
}
