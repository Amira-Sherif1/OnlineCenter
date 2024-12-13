using DataAccess.Reposetory.IReposetory;
using DataAccsess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace OnlineCenter.Controllers
{
    public class UserlecturesController : Controller
    {
        public ILectureRepository lectureRepository { get; set; }
        public IStudentLectureRepository StudentLectureRepository { get; set; }
        public UserManager<IdentityUser> userManager { get; set; }
        public UserlecturesController(UserManager<IdentityUser> userManager ,ILectureRepository lectureRepository, IStudentLectureRepository StudentLectureRepository)
        {
            this.lectureRepository = lectureRepository;
            this.StudentLectureRepository = StudentLectureRepository;
            this.userManager = userManager;
        }
        public IActionResult AllLectures(int CourseId )
        {
            var userid = userManager.GetUserId(User);
            var studentlectures = StudentLectureRepository.GetAll([e => e.Lecture], expression: e => e.StudentId == userid && e.Lecture.CourseId == CourseId).ToList();
            var studentLecturesDict = studentlectures.ToDictionary(sl => sl.LectureId, sl => sl.status);
            var lectures = lectureRepository.GetAll(expression: e => e.CourseId == CourseId);
            ViewBag.StudentLectures = studentLecturesDict;
            return View(lectures);
        }
        public IActionResult LectureDetails(int LectureId)
        {
           var lecture=lectureRepository.GetOne(expresion:e=>e.Id == LectureId);
            return View(lecture);
        }


    }
}
