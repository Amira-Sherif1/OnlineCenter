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
    }
}
