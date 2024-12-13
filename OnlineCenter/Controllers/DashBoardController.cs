using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCenter.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class DashBoardController : Controller
    {
        public IActionResult Statistics()
        {
            return View();
        }
    }
}
