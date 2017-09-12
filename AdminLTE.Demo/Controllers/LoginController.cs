using AdminLTE.Demo.Domain.IRepositories;
using AdminLTE.Demo.Infrastructure.Utility;
using AdminLTE.Demo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace AdminLTE.Demo.MVC.Controllers
{

    public class LoginController : BaseController, IActionFilter
    {
        private readonly IUserRepository _userRepository;
        public LoginController(IUserRepository userRepository)
        {
			_userRepository = userRepository;
		}
        public IActionResult Index()
        {
            var model = new LoginModel
            {
                UserName = "Admin",
                Password = "123456"
            };
            return View(model);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.Controller.GetType().Name;

			filterContext.HttpContext.Session.TryGetValue("CurrentUser", out byte[] result);

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
                    HttpContext.Session.SetString("CurrentUserId", user.Id.ToString());
                    HttpContext.Session.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));
                    //跳转到系统首页
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