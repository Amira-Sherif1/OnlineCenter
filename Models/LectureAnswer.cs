using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LectureAnswer
    {
        public string StudentId { get; set; }
        public Student? Student { get; set; }
        public int AnswerId { get; set; }
        public Answer? Answer { get; set; }
        public int LectureId { get; set; }
        public Lecture? Lecture { get; set; }
        [AllowNull]
        public double? Grade { get; set; }   

    }
}
