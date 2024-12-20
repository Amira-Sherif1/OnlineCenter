using DataAccess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCenter.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class StudentController : Controller
    {
        public IStudentRepository StudentRepository { get; set; }
        public StudentController(IStudentRepository StudentRepository)
        {
            this.StudentRepository = StudentRepository;
        }
        public IActionResult AllStudents()
        {
            var students = StudentRepository.GetAll([e => e.ApplicationUser]);
            return View(students);
        }
        public IActionResult AllStudentsInLecture(int LectureId)
        {
            ViewBag.Lecture = LectureId;
            var students = StudentRepository.GetAll([e => e.ApplicationUser , e=>e.LectureAnswer , e => e.LectureAnswer.Answer] , expression:e=>e.StudentLectures.Any(th=>th.LectureId==LectureId));
            return View(students);
        }

    }
}
