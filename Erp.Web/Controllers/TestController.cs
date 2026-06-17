using Microsoft.AspNetCore.Mvc;

namespace Erp.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return Content("Test Controller çalışıyor!");
        }
    }
}