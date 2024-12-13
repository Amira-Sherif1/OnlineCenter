using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public List<Lecture> Lectures { get; set; }
        public List<TeacherCourse> TeacherCourses { get; set; }
        public List<TeacherBook> Teacherbooks { get; set; }
        public List<AssistIn> AssistIns { get; set; }

    }
}
