using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;

namespace OnlineCenter.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class CourseController : Controller
    {
        //public ApplicationDbContext _db ;
        public ICourseRepository courseRepository { get; set; }
        public ISubjectRepsitory SubjectRepsitory { get; set; }
        public UserManager<IdentityUser> UserManager { get; set; }
        public ITeacherCoursesRepository teacherCoursesRepository { get; set; }
        public ITeacherRepository teacherRepository { get; set; }

        public CourseController(ITeacherRepository teacherRepository,ITeacherCoursesRepository teacherCoursesRepository,UserManager<IdentityUser> UserManager,ICourseRepository courseRepository, ISubjectRepsitory SubjectRepsitory)
        {

            this.courseRepository = courseRepository;
            this.SubjectRepsitory = SubjectRepsitory;
            this.UserManager = UserManager;
            this.teacherRepository = teacherRepository;
            this.teacherRepository = teacherRepository;
            this.teacherCoursesRepository = teacherCoursesRepository;
        }
        public IActionResult Index(int SubjectId = 0)
        {
            IEnumerable<Course> courses = Enumerable.Empty<Course>();

            if (SubjectId == 0)
            {
                if (User.IsInRole("Admin"))
                {
                    // Admin sees all courses
                    courses = courseRepository.GetAll([e => e.Subject]);
                }
                else if (User.IsInRole("Teacher"))
                {
                    // Teacher sees only their courses
                    var teacherId = UserManager.GetUserId(User); // Get the logged-in teacher's user ID
                    courses = courseRepository.GetAll(
                        [e => e.Subject],
                        expression: course => course.TeacherCourses.Any(tc => tc.TeacherId == teacherId)
                    );
                }

                // Populate subjects dropdown
                var subjects = courseRepository.GetAll();
                ViewBag.Subjects = new SelectList(subjects, "Id", "Name");
            }
            else
            {
                if (User.IsInRole("Admin"))
                {
                    // Admin sees courses filtered by SubjectId
                    courses = courseRepository.GetAll([e => e.Subject], expression: course => course.Subject.Id == SubjectId);



                }
                else if (User.IsInRole("Teacher"))
                {
                    // Teacher sees their courses filtered by SubjectId
                    var teacherId = UserManager.GetUserId(User); // Get the logged-in teacher's user ID
                    courses = courseRepository.GetAll([e => e.Subject],
                        
                        expression: course => course.TeacherCourses.Any(tc => tc.TeacherId == teacherId) && course.Subject.Id == SubjectId
                    );
                }

                // Populate subjects dropdown
                var subjects = courseRepository.GetAll();
                ViewBag.Subjects = new SelectList(subjects, "Id", "Name");
                ViewBag.SubjectId = SubjectId; // Keep the selected SubjectId for the UI
            }

            return View(courses);
        }


        public IActionResult Create()
        {
            var subjects = SubjectRepsitory.GetAll();
            ViewBag.Subjects = new SelectList(subjects, "Id", "Name");
            if (User.IsInRole("Admin"))
            {
                var teachers = teacherRepository.GetAll([e => e.ApplicationUser]);

                ViewBag.Teachers = teachers;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Course course, string ApplicationUserId)
        {
            courseRepository.Add(course);
            courseRepository.Save();
            var teacherId = "";
            if (User.IsInRole("Teacher"))
            {
                 teacherId = UserManager.GetUserId(User);
                
            }
            else if (User.IsInRole("Admin"))
            {
                teacherId = ApplicationUserId;
            }

            var teachercourse = new TeacherCourse()
            {
                TeacherId = teacherId,
                CourseId = course.Id,
                Course = course,
                Teacher = teacherRepository.GetOne(expresion: e => e.ApplicationUserId == teacherId)

            };
            teacherCoursesRepository.Add(teachercourse);
            teacherCoursesRepository.Save();
            course.TeacherCourses ??= new List<TeacherCourse>() { teachercourse };

            // i must add the course to subject courses and i should identify way to know in which grade 
            // this subject exists
            //SubjectRepsitory.Add();


            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int Id)
        {
            var course = courseRepository.GetOne(expresion: e => e.Id == Id);
            var subjects = SubjectRepsitory.GetAll();
            ViewBag.Subjects = new SelectList(subjects, "Id", "Name", course.SubjectId);
            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            courseRepository.Edit(course);
            courseRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int Id)
        {
            var course = courseRepository.GetOne(expresion: e => e.Id == Id);
            var teachercourse=teacherCoursesRepository.GetOne(expresion:e=>e.CourseId==course.Id);
            teacherCoursesRepository.Delete(teachercourse);
            courseRepository.Delete(course);
            courseRepository.Save();
            //Course course = new Course() {Id=courseId };
            //_db.Courses.Remove(course);
            //_db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }

}