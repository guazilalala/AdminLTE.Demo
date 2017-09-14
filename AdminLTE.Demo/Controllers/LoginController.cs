using AdminLTE.Demo.Application.UserApp;
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
        private readonly IUserAppService _userAppService;
        public LoginController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
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
                var user = _userAppService.CheckUser(model.UserName, model.Password);
                if (user != null)
                {
                    //记录Session
                    HttpContext.Session.SetString("CurrentUserId", user.Id.ToString());
                    HttpContext.Session.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));
                    //跳转到系统首页
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ErrorInfo = "用户名或密码错误。";
                return View();
            }

            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    ViewBag.ErrorInfo = item.Errors[0].ErrorMessage;
                    break;
                }
            }
            return View(model);
        }
    }
}