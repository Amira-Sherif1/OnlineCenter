using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StudentLecture
    {
        public string StudentId { get; set; }
        public Student? Student { get; set; }
        public int LectureId { get; set; }
        public Lecture? Lecture { get; set; }

        public bool status { get; set; } = false;

    }
}
