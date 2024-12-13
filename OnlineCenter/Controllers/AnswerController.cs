using DataAccess;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Models;

namespace OnlineCenter.Controllers
{
    public class AnswerController : Controller
    {
        // public ApplicationDbContext _db;
        public AnswerRepository answerRepository { get; set; }
        public AnswerController(AnswerRepository answerRepository)
        {
            this.answerRepository = answerRepository;
        }
        public IActionResult Index()
        {
            var answers = answerRepository.GetAll();
            //var answers = _db.Answers.ToList();
           
            return View(answers);
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public  IActionResult Create(Answer answer,IFormFile AnswerDoc)
        {
            if (AnswerDoc.Length > 0)
            {
                var fileName=Guid.NewGuid().ToString()+Path.GetExtension(AnswerDoc.FileName);
                var filePath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Document",fileName);
                using(var stream = System.IO.File.OpenWrite(filePath))
                {
                     AnswerDoc.CopyTo(stream);
                }
                answer.AnswerDoc=fileName;
            }
            answerRepository.Add(answer);
            answerRepository.Save();
            //_db.Answers.Add(answer);
            //_db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int answerId)
        {
            var answer = answerRepository.GetOne(expresion:e=>e.Id==answerId);
            //var answer = _db.Answers.Find(answerId);
            return View(answer);
        }

        [HttpPost]
        public IActionResult Edit(Answer answer,IFormFile AnswerDoc)
        {
            if(AnswerDoc!=null && AnswerDoc.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(AnswerDoc.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Document", fileName);
                using (var stream = System.IO.File.OpenWrite(filePath))
                {
                    AnswerDoc.CopyTo(stream);
                }
                answer.AnswerDoc = fileName;
            }
            else
            {
                var oldAnswer = answerRepository.GetOne(expresion: e => e.Id == answer.Id, tracking: false);
                //var oldAnswer=_db.Answers.AsNoTracking().FirstOrDefault(p=>p.Id==answer.Id);
                answer.AnswerDoc = oldAnswer.AnswerDoc;
            }
            //course = new Course();
            answerRepository.Edit(answer);
            answerRepository.Save();
            //_db.Answers.Update(answer);
            //_db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int answerId)
        {
            var oldanswer = answerRepository.GetOne(expresion: e => e.Id == answerId);
            var oldfilepath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Document", oldanswer.AnswerDoc);
            if (System.IO.File.Exists(oldfilepath1))
            {
                System.IO.File.Delete(oldfilepath1);
            }
            var answer = answerRepository.GetOne(expresion:e=>e.Id == answerId);
            answerRepository.Delete(answer);
            answerRepository.Save();
            //Answer answer = new Answer() { Id = answerId };
            //_db.Answers.Remove(answer);
            //_db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Display(int answerId)
        {
            var answer= answerRepository.GetOne(expresion:e=>e.Id==answerId);
            //var answer = _db.Answers.Find(answerId);

            return View(answer);
        }

      



    }
}

