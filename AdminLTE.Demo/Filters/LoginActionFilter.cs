using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.Demo.Filters
{
    /// <summary>
    /// 登录过滤器
    /// </summary>
    public class LoginActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Todo
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.Controller.GetType().Name;

            byte[] result;
            filterContext.HttpContext.Session.TryGetValue("CurrentUser", out result);
            if (result == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
                return;
            }
        }
    }
}
