using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewData
{
    public class DashboardViewModel
    {
        public decimal TotalBookSales { get; set; }
        public decimal TotalLectureRevenue { get; set; }
        public int TotalStudents { get; set; }
        public int ActiveLectures { get; set; }
        public string[] LectureNames { get; set; }
        public int[] LectureEnrollments { get; set; }
        public string[] GradeLabels { get; set; }
        public int[] GradeDistribution { get; set; }
        public string[] MonthLabels { get; set; }
        public decimal[] MonthlyBooksRevenue { get; set; }
        public decimal[] MonthlyLecturesRevenue { get; set; }
    }
}
