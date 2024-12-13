using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Lecture
    {
        public int Id { get; set; }
        public string? Video { get; set; }
        public string? Assignment { get; set; }
        public string? Document { get; set; }
        [Required]
        public string Title { get; set; } //
        [Required]
        public int Price { get; set; }
        public DateTime Date { get; set; }
        public int? CourseId { get; set; }
        public Course? Course { get; set; }
        public LectureAnswer? LectureAnswer { get; set; }
        public List<StudentLecture>? StudentLectures { get; set; }

    }
}
