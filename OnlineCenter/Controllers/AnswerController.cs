using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using DataAccsess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Models;

namespace OnlineCenter.Controllers
{
    public class AnswerController : Controller
    {
        // public ApplicationDbContext _db;
        public IAnswerRepository answerRepository { get; set; }
        public IStudentLectureRepository studentRepository { get; set; }
        public ILectureRepository LectureRepository { get; set; }
        public ILectureAnswerRepository LectureAnswerRepository { get; set; }
        public UserManager<IdentityUser> userManager { get; set; }
        public AnswerController(UserManager<IdentityUser> userManager,ILectureAnswerRepository LectureAnswerRepository,ILectureRepository LectureRepository,IStudentLectureRepository studentRepository,IAnswerRepository answerRepository)
        {
            this.answerRepository = answerRepository;
            this.studentRepository = studentRepository;
            this.LectureRepository = LectureRepository;
            this.LectureAnswerRepository=LectureAnswerRepository;
            this.userManager = userManager;
        }
        public IActionResult GetAll(string StudentId , int LectureId)
        {
            var answers = answerRepository.GetAll([e=>e.lectureAnswer],expression:e=>e.lectureAnswer.LectureId==LectureId && e.lectureAnswer.StudentId==StudentId);           
            return View(answers);
        }
        public IActionResult Create(int LectureId)
        {
            ViewBag.LectureId = LectureId;
            return View();
        }


        [HttpPost]
        public  IActionResult Create(Answer answer,IFormFile AnswerDoc , int LectureId)
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
            var lectureanswer = new LectureAnswer()
            {
                StudentId = userManager.GetUserId(User),
                LectureId= LectureId,
                AnswerId=answer.Id,
                Answer=answer,
                Grade=0
            };
            answerRepository.Add(answer);
            answerRepository.Save();
            LectureAnswerRepository.Add(lectureanswer);
            LectureAnswerRepository.Save();
            return RedirectToAction("LectureDetails", "UserLectures", new {LectureId = LectureId});
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

