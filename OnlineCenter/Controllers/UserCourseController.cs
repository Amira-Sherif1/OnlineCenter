using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq.Expressions;

namespace OnlineCenter.Controllers
{
    public class UserCourseController : Controller
    {
        public ICourseRepository courseRepository { get; set; }
        public ITeacherCoursesRepository teacherCoursesRepository { get; set; }
        public UserCourseController(ICourseRepository courseRepository , ITeacherCoursesRepository teacherCoursesRepository)
        {
            this.courseRepository = courseRepository;
            this.teacherCoursesRepository = teacherCoursesRepository;
        }
        public IActionResult AllCourses(int SubjectId =0)
        {
            if (SubjectId == 0) 
            {
                var courses = courseRepository.GetAll([e => e.Subject, e => e.Lectures, e => e.TeacherCourses]);
                return View(courses);

            }
            else
            {
                var courses = courseRepository.GetAll([e => e.Subject, e => e.Lectures], expression: e => e.SubjectId == SubjectId);

                return View(courses);

            }
           
        }
    }
}
