using DataAccess.Reposetory.IReposetory;
using DataAccsess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;

namespace OnlineCenter.Controllers
{
    public class UserlecturesController : Controller
    {
        public ILectureRepository lectureRepository { get; set; }
        public IStudentLectureRepository StudentLectureRepository { get; set; }
        public ILectureAnswerRepository LectureAnswerRepository { get; set; }
        public UserManager<IdentityUser> userManager { get; set; }
        public UserlecturesController(ILectureAnswerRepository LectureAnswerRepository,UserManager<IdentityUser> userManager ,ILectureRepository lectureRepository, IStudentLectureRepository StudentLectureRepository)
        {
            this.lectureRepository = lectureRepository;
            this.StudentLectureRepository = StudentLectureRepository;
            this.userManager = userManager;
            this.LectureAnswerRepository = LectureAnswerRepository;
        }
        public IActionResult AllLectures(int CourseId )
        {
            var userid = userManager.GetUserId(User);
            var studentlectures = StudentLectureRepository.GetAll([e => e.Lecture ], expression: e => e.StudentId == userid && e.Lecture.CourseId == CourseId).ToList();
            var studentLecturesDict = studentlectures.ToDictionary(sl => sl.LectureId, sl => sl.status);
            ViewBag.StudentLectures = studentLecturesDict;
            var lectures = lectureRepository.GetAll(expression: e => e.CourseId == CourseId);
            return View(lectures);
        }
        public IActionResult LectureDetails(int LectureId)
        {
           var lecture=lectureRepository.GetOne(expresion:e=>e.Id == LectureId);
            var userId=userManager.GetUserId(User); 
            var lectureanswer = LectureAnswerRepository.GetAll([e=>e.Answer],expression:e=>e.LectureId == LectureId && e.StudentId==userId).FirstOrDefault();
            if (lectureanswer == null)
            {
                ViewBag.lectureanswer = null;


            }
            else
            {
                var answer = lectureanswer.Answer.AnswerDoc;
                ViewBag.lectureanswer = answer;


            }
            return View(lecture);
        }


    }
}
