using DataAccess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCenter.Controllers
{
    public class UserSubjectController : Controller
    {
        public ISubjectRepsitory subjectRepsitory { get; set; }
        public UserSubjectController(ISubjectRepsitory subjectRepsitory)
        {
            this.subjectRepsitory = subjectRepsitory;
        }
        public IActionResult AllSubjects(int GradeId)
        {
            var subjects = subjectRepsitory.GetAll([e=>e.Courses],expression:e=>e.GradeSubjects.Any(th=>th.GradeId==GradeId)).ToList();
            return View(subjects);
        }
    }
}
