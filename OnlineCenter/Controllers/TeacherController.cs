using DataAccess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCenter.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class TeacherController : Controller
    {
        public ITeacherRepository teacherRepository { get; set; }
        public TeacherController(ITeacherRepository teacherRepository)
        {
            this.teacherRepository = teacherRepository;
        }
        public IActionResult AllTeachers()
        {
            var teachers = teacherRepository.GetAll([e=>e.ApplicationUser]);
            return View(model:teachers);
        }
        public IActionResult AllTeachersmain()
        {
            var teachers = teacherRepository.GetAll([e => e.ApplicationUser]);
            return View(model: teachers);
        }

    }
}
