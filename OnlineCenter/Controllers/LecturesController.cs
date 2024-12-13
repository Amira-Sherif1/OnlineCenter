using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using DataAccsess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
using System.Collections.Specialized;
using System.Security.Claims;

namespace OnlineCenter.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class LecturesController : Controller
    {
        public ILectureRepository LectureRepository { get; set; }
        public ICourseRepository courseRepository { get; set; }
        public ITeacherCoursesRepository teacherCourses { get; set; }
        public ITeacherRepository TeacherRepository { get; set; }
        public LecturesController(ITeacherRepository TeacherRepository ,ITeacherCoursesRepository teacherCourses, ILectureRepository LectureRepository, ICourseRepository courseRepository)
        {
            this.LectureRepository = LectureRepository;
            this.courseRepository = courseRepository;
            this.teacherCourses = teacherCourses;
            this.TeacherRepository = TeacherRepository;
        }
        public IActionResult AllLectures(int CourseId)
        {
            var lectures = LectureRepository.GetAll(expression:e=>e.CourseId == CourseId);
            if (lectures.Any())
            {
                var studentCounts = lectures.ToDictionary(
                    lecture => lecture.Id,
                    lecture => lecture.StudentLectures?.Count() ?? 0  
                );

                ViewBag.StudentCounts = studentCounts;
            }
            ViewBag.CourseId = CourseId;

            return View(model: lectures.ToList());
        }

        [HttpGet]
        public IActionResult AddLecture(int CourseId)
        {
            ViewBag.CourseId = CourseId;
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> AddLectureAsync(Lecture lecture, IFormFile Document, IFormFile Assignment, IFormFile Video, int CourseId)


        {
            if (ModelState.IsValid)
            {
                if (Document != null && Document.Length > 0 && Assignment != null && Assignment.Length > 0 && Video != null && Video.Length > 0)

                {
                    var filename1 = Guid.NewGuid() + Path.GetExtension(Document.FileName);
                    var filename2 = Guid.NewGuid() + Path.GetExtension(Assignment.FileName);
                    var filename3 = Guid.NewGuid() + Path.GetExtension(Video.FileName);

                    var filepath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Doc", filename1);
                    var filepath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assignments", filename2);
                    var filepath3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", filename3);

                    using (var stream = System.IO.File.Create(filepath1))
                    {
                        await Document.CopyToAsync(stream);
                    }
                    lecture.Document = filename1;

                    using (var stream = System.IO.File.Create(filepath2))
                    {
                        await Assignment.CopyToAsync(stream);
                    }
                    lecture.Assignment = filename2;

                    using (var stream = System.IO.File.Create(filepath3))
                    {
                        await Video.CopyToAsync(stream);
                    }
                    lecture.Video = filename3;
                }
                lecture.CourseId = CourseId;
                LectureRepository.Add(lecture);
                LectureRepository.Save();

                return RedirectToAction("AllLectures", new { CourseId = lecture.CourseId });

            }
            return RedirectToAction("error", "Home");



        }




        [HttpGet]

        public IActionResult EditLicture(int Id, int CourseId)
        {
            //var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var teacherId = "e0c9e740-5a82-456f-ba7f-bf66d8c74ed5";
            var teachercourses = teacherCourses.GetAll([e => e.Teacher, e => e.Course], e => e.TeacherId == teacherId).Select(t => t.Course).ToList();
            var lecture= LectureRepository.GetOne(expresion:e=>e.Id == Id);
            ViewBag.CourseId = new SelectList(teachercourses, "Id", "Name");
            ViewBag.CourseId = CourseId;
            return View(lecture);

        }
        [HttpPost]
        public async Task<IActionResult> EditLicture(Lecture lecture, IFormFile Document, IFormFile Assignment, IFormFile Video, int CourseId)
        {
           var oldlecture = LectureRepository.GetOne(expresion:e=>e.Id==lecture.Id,tracking:false);
            if (Document != null && Document.Length > 0 && Assignment != null && Video.Length > 0 && Video != null)
            {
                var filename1 = Guid.NewGuid() + Path.GetExtension(Document.FileName);
                var filename2 = Guid.NewGuid() + Path.GetExtension(Assignment.FileName);
                var filename3 = Guid.NewGuid() + Path.GetExtension(Video.FileName);


                var filepath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Doc", filename1);
                var filepath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assignments", filename2);
                var filepath3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", filename3);

                var oldfilepath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Doc", oldlecture.Document);
                var oldfilepath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assignments", oldlecture.Assignment);
                var oldfilepath3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", oldlecture.Video);

                using (var stream = System.IO.File.Create(filepath1))
                {
                    await Document.CopyToAsync(stream);
                }
                lecture.Document = filename1;

                using (var stream = System.IO.File.Create(filepath2))
                {
                    await Assignment.CopyToAsync(stream);
                }
                lecture.Assignment = filename2;

                using (var stream = System.IO.File.Create(filepath3))
                {
                    await Video.CopyToAsync(stream);
                }
                lecture.Video = filename3;
                if (System.IO.File.Exists(oldfilepath1))
                {
                    System.IO.File.Delete(oldfilepath1);

                }
                if (System.IO.File.Exists(oldfilepath2))
                {
                    System.IO.File.Delete(oldfilepath2);

                }
                if (System.IO.File.Exists(oldfilepath3))
                {
                    System.IO.File.Delete(oldfilepath3);

                }
            }
            else
            {
                lecture.Document = oldlecture.Document;
                lecture.Assignment=oldlecture.Assignment;
                lecture.Video=oldlecture.Video;

            }
            lecture.CourseId = CourseId;
            LectureRepository.Edit(lecture);
            LectureRepository.Save();

            return RedirectToAction("AllLectures", new { CourseId = lecture.CourseId });


        }
        public IActionResult Deletelecture(int id)
        {
            var oldlecture = LectureRepository.GetOne(expresion: e => e.Id == id);
            var oldfilepath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Doc", oldlecture.Document);
            var oldfilepath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Assignments", oldlecture.Assignment);
            var oldfilepath3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", oldlecture.Video);
            int CourseId = (int)oldlecture.CourseId;

            if (System.IO.File.Exists(oldfilepath1))
            {
                System.IO.File.Delete(oldfilepath1);
            }
            if (System.IO.File.Exists(oldfilepath2))
            {
                System.IO.File.Delete(oldfilepath2);
            }
            if (System.IO.File.Exists(oldfilepath3))
            {
                System.IO.File.Delete(oldfilepath3);
            }
            LectureRepository.Delete(oldlecture);
            LectureRepository.Save();
            return RedirectToAction("AllLectures", new { CourseId = CourseId});

        }

    }
}
