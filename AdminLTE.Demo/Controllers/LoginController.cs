using AdminLTE.Demo.Data;
using AdminLTE.Demo.Data.IRepositories;
using AdminLTE.Demo.Models;
using AdminLTE.Demo.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace AdminLTE.Demo.Controllers
{

    public class LoginController : Controller,IActionFilter
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public LoginController(UnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.Controller.GetType().Name;

            byte[] result;

            filterContext.HttpContext.Session.TryGetValue("CurrentUser", out result);

            if (result != null)
            {
                filterContext.Result = new RedirectResult("/Home/Index");
                return;
            }
        }

        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //检查用户信息
                var user = _userRepository.CheckUser(model.UserName, model.Password);

                if (user != null)
                {
                    //记录Session
                    HttpContext.Session.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));

                    return RedirectToAction("Index", "Home");
                }

                ViewBag.ErrorInfo = "用户名或密码错误";

                return View();
            }

            ViewBag.ErrorInfo = ModelState.Values.First().Errors[0].ErrorMessage;
            return View(model);
        }
    }
}