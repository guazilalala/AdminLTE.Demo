using AdminLTE.Demo.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTE.Demo.Controllers
{
    [ServiceFilter(typeof(LoginActionFilter))]
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}