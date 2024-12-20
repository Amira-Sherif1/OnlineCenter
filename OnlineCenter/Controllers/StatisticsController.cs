using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using DataAccsess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewData;
using System.Linq;

namespace OnlineCenter.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class StatisticsController : Controller
    {
        public IBookPaymentRepository BookPaymentRepository { get; set; }
        public IPaymentTransactionRepository paymentTransactionRepository { get; set; }
        public ITeacherCoursesRepository teacherCoursesRepository { get; set; }
        public IStudentRepository studentRepository { get; set; }
        public IStudentLectureRepository studentLectureRepository { get; set; }

        public StatisticsController(
            IStudentLectureRepository studentLectureRepository,
            ITeacherCoursesRepository teacherCoursesRepository,
            IStudentRepository studentRepository,
            IPaymentTransactionRepository paymentTransactionRepository,
            IBookPaymentRepository BookPaymentRepository)
        {
            this.BookPaymentRepository = BookPaymentRepository;
            this.paymentTransactionRepository = paymentTransactionRepository;
            this.teacherCoursesRepository = teacherCoursesRepository;
            this.studentRepository = studentRepository;
            this.studentLectureRepository = studentLectureRepository;
        }

        public IActionResult Index()
        {
            // Book sales
            var BooksSales = BookPaymentRepository.GetAll().Sum(e => e.Amount);
            var LecturesSales = paymentTransactionRepository.GetAll().Sum(e => e.Amount);

            // Student count
            var TotalStudents = studentRepository.GetAll().Count();

            // Active lectures count
            var ActiveLectures = teacherCoursesRepository.GetAll().Count();

            // Student grades distribution
            var studentGrades = studentRepository.GetAll([e=>e.Grade])
                .GroupBy(e => e.Grade.Name)
                .Select(g => new { Grade = g.Key, Count = g.Count() })
                .ToList();
            var gradeLabels = studentGrades.Select(g => g.Grade).ToArray();
            var gradeDistribution = studentGrades.Select(g => g.Count).ToArray();

            // Lecture enrollments distribution
            var lectureEnrollments = studentLectureRepository.GetAll([e=>e.Lecture])
                .GroupBy(e => e.Lecture.Title)
                .Select(g => new { LectureTitle = g.Key, Count = g.Count() })
                .ToList();
            var lectureNames = lectureEnrollments.Select(l => l.LectureTitle).ToArray();
            var lectureEnrollCounts = lectureEnrollments.Select(l => l.Count).ToArray();

            // Monthly revenue data
            var months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var monthlyBooksRevenue = new decimal[12];

            for (int month = 1; month <= 12; month++)
            {
                monthlyBooksRevenue[month - 1] = BookPaymentRepository.GetAll()
                    .Where(payment => payment.CreatedAt.Month == month)
                    .Sum(payment => payment.Amount);
            }
            var monthlyLecturesRevenue = new decimal[12];

            for (int month = 1; month <= 12; month++)
            {
                monthlyLecturesRevenue[month - 1] = paymentTransactionRepository.GetAll()
                    .Where(payment => payment.CreatedAt.Month == month)
                    .Sum(payment => payment.Amount);
            }

            // Prepare the model
            var model = new DashboardViewModel
            {
                TotalBookSales = BooksSales,
                TotalLectureRevenue = LecturesSales,
                TotalStudents = TotalStudents,
                ActiveLectures = ActiveLectures,
                GradeLabels = gradeLabels,
                GradeDistribution = gradeDistribution,
                LectureNames = lectureNames,
                LectureEnrollments = lectureEnrollCounts,
                MonthLabels = months,
                MonthlyBooksRevenue = monthlyBooksRevenue,
                MonthlyLecturesRevenue = monthlyLecturesRevenue
            };

            return View(model);
        }
    }
}
