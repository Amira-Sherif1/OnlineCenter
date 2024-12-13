using DataAccess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace OnlineCenter.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class GradeController : Controller
    {
        public IGradeRepository gradeRepository { get; set; }
        public GradeController(IGradeRepository gradeRepository)
        {
            this.gradeRepository = gradeRepository;
        }
        public IActionResult AllGrades()
        {
            var grades = gradeRepository.GetAll();
            return View(grades);
        }
        [HttpGet]
        public IActionResult AddGrade() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddGrade( Grade grade)
        {
            gradeRepository.Add(grade);
            gradeRepository.Save();
            return RedirectToAction(nameof(AllGrades));
        }
        public IActionResult EditGrade(int Id)
        {
            var grade = gradeRepository.GetOne(expresion:e=>e.Id==Id);
            return View(grade);
        }
        [HttpPost]
        public IActionResult EditGrade(Grade grade)
        {
            gradeRepository.Edit(grade);
            gradeRepository.Save();
            return RedirectToAction(nameof(AllGrades));

        }
        public IActionResult DeleteGrade(int Id)
        {
            var grade = gradeRepository.GetOne(expresion: e => e.Id == Id);
            gradeRepository.Delete(grade);
            gradeRepository.Save();
            return RedirectToAction(nameof(AllGrades));
        }

    }
}
