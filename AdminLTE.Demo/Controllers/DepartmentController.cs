using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminLTE.Demo.Filters;

namespace AdminLTE.Demo.Controllers
{
    [ServiceFilter(typeof(LoginActionFilter))]
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}