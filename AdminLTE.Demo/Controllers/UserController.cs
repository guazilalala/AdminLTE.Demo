using AdminLTE.Demo.Application.DepartmentApp;
using AdminLTE.Demo.Application.DepartmentApp.Dtos;
using AdminLTE.Demo.Application.MenuApp;
using AdminLTE.Demo.Application.RoleApp;
using AdminLTE.Demo.Application.UserApp;
using AdminLTE.Demo.Application.UserApp.Dtos;
using AdminLTE.Demo.Domain.Entities;
using AdminLTE.Demo.Domain.IRepositories;
using AdminLTE.Demo.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AdminLTE.Demo.MVC.Controllers
{
    [ServiceFilter(typeof(LoginActionFilter))]
    public class UserController : BaseController
    {
        private readonly IUserAppService _userAppService;
        private readonly IRoleAppService _roleAppService;
        private readonly IMenuAppService _menuAppService;
        private readonly IDepartmentAppService _departmentAppService;

        public UserController(IUserAppService userAppService, 
            IRoleAppService roleAppService, 
            IMenuAppService menuAppService,
            IDepartmentAppService departmentAppService)
        {
            _departmentAppService = departmentAppService;
            _userAppService = userAppService;
            _roleAppService = roleAppService;
            _menuAppService = menuAppService;
        }

        public IActionResult Index()
        {
            ViewBag.PageHeader = "用户管理";
            ViewBag.OptionalDescription = "列表";

            List<DepartmentDto> departments = _departmentAppService.GetAllList();
            return View(departments);
        }

        public IActionResult GetUserByDepartment(Guid departmentId, int startPage, int pageSize)
        {
            int rowCount = 0;
            var result = _userAppService.GetUserByDepartment(departmentId, startPage, pageSize, out rowCount);
            var roles = _roleAppService.GetAllList();
            return Json(new
            {
                rowCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize),
                rows = result,
                roles = roles
            });
        }

        public IActionResult Edit(UserDto dto, string roles)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState.Values)
                {
                    if (item.Errors.Count > 0)
                    {
                        ViewBag.ErrorInfo = item.Errors[0].ErrorMessage;
                        break;
                    }
                }
                return View(dto);
            }

            try
            {
                if (dto.Id == Guid.Empty)
                {
                    dto.Id = Guid.NewGuid();
                }
                var userRoles = new List<UserRoleDto>();
                foreach (var role in roles.Split(','))
                {
                    userRoles.Add(new UserRoleDto() { UserId = dto.Id, RoleId = Guid.Parse(role) });
                }
                dto.UserRoles = userRoles;
                var user = _userAppService.InsertOrUpdate(dto);
                return Json(new { Result = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Faild", Message = ex.Message });

            }
        }

        public IActionResult DeleteMuti(string ids)
        {
            try
            {
                string[] idArray = ids.Split(',');
                List<Guid> delIds = new List<Guid>();
                foreach (string id in idArray)
                {
                    delIds.Add(Guid.Parse(id));
                }
                _userAppService.DeleteBatch(delIds);
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
        public IActionResult Delete(Guid id)
        {
            try
            {
                _userAppService.Delete(id);
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
            var dto = _userAppService.Get(id);
            return Json(dto);
        }
    }
}