using Microsoft.AspNetCore.Mvc;

namespace TestChatSignalR.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
