using AdminLTE.Demo.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTE.Demo.MVC.Controllers
{
    [ServiceFilter(typeof(LoginActionFilter))]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}