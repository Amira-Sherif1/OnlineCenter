using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace OnlineCenter.Controllers
{
    //[Authorize(Roles = "Admin,Teacher")]
    public class BookController : Controller
    {
        public ApplicationDbContext _db;
        public IBookRepository BookRepository { get; set; }
        public UserManager<IdentityUser> userManager { get; set; }
        public ICartRepository cartRepository { get; set; }
        public BookController(ICartRepository cartRepository ,UserManager<IdentityUser> userManager ,IBookRepository BookRepository)
        {
            this.BookRepository = BookRepository;
            this.userManager = userManager;
            this.cartRepository = cartRepository;
        }
        public IActionResult Index()
        {
            var userid = userManager.GetUserId(User);
            var studentbooks = cartRepository.GetAll([e => e.Book], expression: e => e.StudentId == userid);
            var studentLecturesDict = studentbooks.ToDictionary(sl => sl.BookId, sl => sl.status);
            ViewBag.StudentBooks = studentLecturesDict;

            var books = BookRepository.GetAll([m=>m.Teacherbook , m=>m.Teacherbook.Teacher , m=>m.Teacherbook.Teacher.ApplicationUser])  // Assuming this returns List<Book>
                .AsQueryable()  
                .Include(b => b.Teacherbook)
                .ThenInclude(b => b.Teacher)
                .ThenInclude(b=>b.ApplicationUser)
                .ToList();

            return View(books);
        }
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            BookRepository.Add(book);
            BookRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int bookId)
        {
            var book = BookRepository.GetOne([e=>e.Teacherbook],expresion:e=>e.Id == bookId);
           // var book= _db.Books.Find(bookId);
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            BookRepository.Edit(book);
            BookRepository.Save();
            //_db.Books.Update(book);
            //_db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int bookId)
        {
            //Book book = new Book() { Id = bookId };
            //_db.Books.Remove(book);
            //_db.SaveChanges();
            Book book = BookRepository.GetOne(expresion: e => e.Id == bookId);
            BookRepository.Delete(book);
            BookRepository.Save();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
