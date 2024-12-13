using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace OnlineCenter.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class AssistantController : Controller
    {
        public UserManager<IdentityUser> userManager { get; set; }
        public IAssistantRepository AssistantRepository { get; set; }
        public IAssestInReposetory AssestInReposetory { get; set; }
        public ICourseRepository courseRepository { get; set; }
        public ITeacherCoursesRepository TeacherCoursesRepository { get; set; }

        public AssistantController(ITeacherCoursesRepository TeacherCoursesRepository ,IAssistantRepository AssistantRepository , UserManager<IdentityUser> userManager, IAssestInReposetory AssestInReposetory)
        {
            this.AssistantRepository = AssistantRepository;
            this.userManager = userManager;
            this.AssestInReposetory = AssestInReposetory;
            this.TeacherCoursesRepository = TeacherCoursesRepository;
        }
        public IActionResult AllAssistants(int courseId = 0)
        {
            IEnumerable<Assistant> assistants;

            if (courseId == 0)
            {
                assistants = AssistantRepository.GetAll([e => e.ApplicationUser]);
            }
            else
            {
                assistants = AssistantRepository.GetAll([e => e.ApplicationUser, e => e.AssistIns])
                                                .Where(th => th.AssistIns.Any(ai => ai.CourseId == courseId));
                                                
            }

            return View(assistants.ToList());
        }
        
        public IActionResult AddAssistantToCourse(int CourseId)
        {
            ViewBag.CourseId= CourseId;
            var assistants= AssistantRepository.GetAll([e=>e.ApplicationUser]);
            return View(assistants);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAssistantToCourse(int CourseId, string ApplicationUserId)
        {
            if (ModelState.IsValid) 
            {
                var existingAssignment = AssestInReposetory.GetOne(expresion:e => e.CourseId == CourseId && e.AssistantId == ApplicationUserId);

                if (existingAssignment == null)
                {
                    var teachercourse = TeacherCoursesRepository.GetOne(expresion: e => e.CourseId == CourseId);
                    var teacherId = teachercourse.TeacherId;

                    var asstantin = new AssistIn()
                    {
                        CourseId = CourseId,
                        AssistantId = ApplicationUserId,
                        TeacherId = teacherId

                    };
                    AssestInReposetory.Add(asstantin);
                    AssestInReposetory.Save();

                }
               
                return RedirectToAction("Index", "Course");
            }
            ViewBag.CourseId = CourseId;
            var assistants = AssistantRepository.GetAll([e => e.ApplicationUser]);
            return RedirectToAction("AddAssistantToCourse", new {CourseId= CourseId });

        }



    }
    
}
