using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace OnlineCenter.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class AdminBooksController : Controller
    {
        public ITeacherBooksRepository TeacherBooksRepository { get; set; }
        public IBookRepository BookRepository { get; set; }
        public ITeacherRepository TeacherRepository { get; set; }
        public ICourseRepository courseRepository { get; set; }
        public IApplicationUserRepository ApplicationUserRepository { get; set; }
        public AdminBooksController(IApplicationUserRepository ApplicationUserRepository ,ICourseRepository courseRepository,ITeacherRepository TeacherRepository ,IBookRepository bookRepository, ITeacherBooksRepository TeacherBooksRepository)
        {
            this.BookRepository = bookRepository;
            this.TeacherBooksRepository = TeacherBooksRepository;
            this.TeacherRepository=TeacherRepository;
            this.courseRepository=courseRepository;
            this.ApplicationUserRepository=ApplicationUserRepository;
        }
        public IActionResult AllBooks()
        {
            
            var books = BookRepository.GetAll([e=>e.Teacherbook , e=>e.Teacherbook.Teacher , e => e.Teacherbook.Teacher.ApplicationUser])
                .AsQueryable().Include(e=>e.Teacherbook)
                .ThenInclude(e=>e.Teacher).ThenInclude(e=>e.ApplicationUser)
                .ToList();
                ;
            return View(books);
        }
        [HttpGet]
        public IActionResult AddBook()
        {
            var teachers = TeacherRepository.GetAll([e => e.ApplicationUser]);
            ViewBag.Teachers = teachers;
            var courses = courseRepository.GetAll();
            ViewBag.Courses = courses;
            return View();
        }
        [HttpPost]
        public IActionResult AddBook(Book book , int CourseId , string ApplicationUserId)
        {
            if (ModelState.IsValid) 
            {
                var teacherbooks = new TeacherBook()
                {
                    TeacherId = ApplicationUserId,
                    Teacher = TeacherRepository.GetOne(expresion: e => e.ApplicationUserId == ApplicationUserId),
                    CourseId = CourseId,
                    Course = courseRepository.GetOne(expresion:e=>e.Id==CourseId),
                    Book = book,
                    BookId=book.Id,                   
                };
                teacherbooks.Teacher.ApplicationUser = ApplicationUserRepository.GetOne(expresion:e=>e.Id==ApplicationUserId);
                book.Teacherbook = teacherbooks;
                BookRepository.Add(book);
                TeacherBooksRepository.Add(teacherbooks);
                BookRepository.Save();
                TeacherBooksRepository.Save();

                return RedirectToAction(nameof(AllBooks));
            }
            return RedirectToAction("error","Home");
            
        }


    }
}
