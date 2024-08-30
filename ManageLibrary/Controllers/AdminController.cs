using Microsoft.AspNetCore.Mvc;

namespace ManageLibrary.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
