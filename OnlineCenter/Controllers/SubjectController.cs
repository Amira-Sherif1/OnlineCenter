using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;

namespace OnlineCenter.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class SubjectController : Controller
    {
        public ISubjectRepsitory subjectRepsitory { get; set; }
        public IGradeRepository gradeRepository { get; set; }
        public IGradeSubjectRepository gradeSubjectRepository { get; set; }
        public SubjectController(ISubjectRepsitory subjectRepsitory, IGradeRepository gradeRepository, IGradeSubjectRepository gradeSubjectRepository)
        {
             this.subjectRepsitory = subjectRepsitory;
            this.gradeRepository = gradeRepository;
            this.gradeSubjectRepository = gradeSubjectRepository;
        }
        public IActionResult AllSubjects()
        {
            var subjects = subjectRepsitory.GetAll();


            return View(subjects);
        }
        [HttpGet]
        public IActionResult AddSubject()
        {
            var grades = gradeRepository.GetAll();
            ViewBag.Grades = new SelectList(grades, "Id", "Name");
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddSubject(Subject subject, int GradeId)
        {
            if (ModelState.IsValid)
            {
                // Ensure GradeId is valid and maps to a GradeSubject
                //var gradesubject = gradeSubjectRepository.GetOne(expresion: e => e.GradeId == GradeId);

                // Initialize GradeSubjects if null and add the associated GradeSubject
                var gradesubject = new GradeSubject()
                {
                    GradeId= GradeId,
                    SubjectId=subject.Id,
                };
                    // Add subject to the repository
                    subjectRepsitory.Add(subject);
                    subjectRepsitory.Save();
                    subject.GradeSubjects ??= new List<GradeSubject>() { gradesubject };
                    gradeSubjectRepository.Add(gradesubject);
                    gradeSubjectRepository.Save();
                    // Redirect to the list of all subjects
                    return RedirectToAction(nameof(AllSubjects));
                

            }

            // Repopulate ViewBag.Grades for redisplay in case of validation errors
            ViewBag.Grades = new SelectList(gradeRepository.GetAll(), "Id", "Name");
            return View(subject);
        }

        [HttpGet]
        public IActionResult EditSubject(int Id)
        {
            var subject = subjectRepsitory.GetOne(expresion:e=>e.Id==Id);


            return View(subject);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSubject(Subject subject, int GradeId)
        {

            if (ModelState.IsValid)
            {
               
                subjectRepsitory.Edit(subject);
                subjectRepsitory.Save();

                // Redirect to the list of all subjects
                return RedirectToAction(nameof(AllSubjects));



            }
            return RedirectToAction("error", "Home");
        }
        public IActionResult DeleteSubject(int Id)
        {
            var subject = subjectRepsitory.GetOne(expresion:e=>e.Id==Id);
            subjectRepsitory.Delete(subject);
            subjectRepsitory.Save();
            return RedirectToAction(nameof(AllSubjects));
        }



        public IActionResult Add(Subject subject, int GradeId)
        {
            if (ModelState.IsValid)
            {
                // Ensure GradeId is valid and maps to a GradeSubject
                var gradesubject = gradeSubjectRepository.GetOne(expresion: e => e.GradeId == GradeId);
                if (gradesubject != null)
                {
                    // Initialize GradeSubjects if null and add the associated GradeSubject
                    subject.GradeSubjects ??= new List<GradeSubject>();
                    subject.GradeSubjects.Add(gradesubject);

                    // Add subject to the repository
                    subjectRepsitory.Add(subject);

                    // Redirect to the list of all subjects
                    return RedirectToAction(nameof(AllSubjects));
                }

                // Handle case where GradeId is invalid
                ModelState.AddModelError("GradeId", "The selected grade is invalid.");
            }

            // Repopulate ViewBag.Grades for redisplay in case of validation errors
            ViewBag.Grades = new SelectList(gradeRepository.GetAll(), "Id", "Name");
            return View(subject);
        }

    }
}
