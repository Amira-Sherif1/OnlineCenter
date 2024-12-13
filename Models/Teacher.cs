using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Teacher
    {
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string Degree { get; set; }
        public List<TeacherCourse> TeacherCourses { get; set; }
        public List<TeacherBook> Teacherbooks { get; set; }
        public List<AssistIn> AssistIns { get; set; }
    }
}
