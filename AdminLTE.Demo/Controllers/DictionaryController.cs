using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTE.Demo.MVC.Controllers
{
    public class DictionaryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}