using DataAccess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Mvc;
using OnlineCenter.Models;
using System.Diagnostics;

namespace OnlineCenter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IGradeRepository gradeRepository { get; set; }
        public HomeController(ILogger<HomeController> logger , IGradeRepository gradeRepository)
        {
            _logger = logger;
            this.gradeRepository = gradeRepository;
        }

        public IActionResult Index()
        {
            var grades = gradeRepository.GetAll([e=>e.GradeSubjects]);           
            return View(grades);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
