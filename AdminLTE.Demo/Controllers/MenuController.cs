using AdminLTE.Demo.Application.MenuApp;
using AdminLTE.Demo.Application.MenuApp.Dtos;
using AdminLTE.Demo.Filters;
using AdminLTE.Demo.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AdminLTE.Demo.MVC.Controllers
{
    [ServiceFilter(typeof(LoginActionFilter))]
    public class MenuController : BaseController
    {
        private readonly IMenuAppService _menuAppService;

        public MenuController(IMenuAppService menuAppService, Application.UserApp.IUserAppService userAppService)
        {
            _menuAppService = menuAppService;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取功能树JSON数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMenuTreeData()
        {
            var menus = _menuAppService.GetAllList();
            List<TreeModel> treeModels = new List<TreeModel>();
            foreach (var menu in menus)
            {
                treeModels.Add(new TreeModel
                {
                    Id = menu.Id.ToString(),
                    Text = menu.Name,
                    Parent = menu.ParentId == Guid.Empty ? "#" : menu.ParentId.ToString()
                });
            }
            ViewBag.TreeModels = treeModels;
            return Json(treeModels);

        }

        /// <summary>
        /// 获取子级功能列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="startPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IActionResult GetMenusByParent(Guid parentId, int startPage, int pageSize)
        {
            int rowCount = 0;
            var result = _menuAppService.GetMenusByParent(parentId, startPage, pageSize, out rowCount);

            return Json(new
            {
                rowCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount / pageSize)),
                rows = result
            });
        }

        /// <summary>
        /// 新增或编辑功能
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IActionResult Edit(MenuDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    Result = "Faild",
                    Message = GetModelStateError()
                });
            }

            if (_menuAppService.InsertOrUpdate(dto))
            {
                return Json(new { Result = "Success" });
            }

            return Json(new { Result = "Faild" });
        }

        public IActionResult Delete(Guid id)
        {
            try
            {
                _menuAppService.Delete(id);
                return Json(new
                {
                    Result = "Success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Faild",
                    Message = ex.Message
                });
            }
        }

        public IActionResult Get(Guid id)
        {
            var dto = _menuAppService.Get(id);
            return Json(dto);
        }

    }
}