using DataAccess;
using DataAccess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace OnlineCenter.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class BookController : Controller
    {
        public ApplicationDbContext _db;
        public IBookRepository BookRepository { get; set; }
        public BookController(IBookRepository BookRepository)
        {
            this.BookRepository = BookRepository;
        }
        public IActionResult Index()
        {

            //var books = _db.Books.ToList();
            var books = BookRepository.GetAll();
            
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
            var book = BookRepository.GetOne(expresion:e=>e.Id == bookId);
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
