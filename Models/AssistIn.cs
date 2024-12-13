using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AssistIn
    {
        public string AssistantId { get; set; }
        public Assistant? Assistant { get; set; }
        public string TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }

    }
}
