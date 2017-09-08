using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminLTE.Demo.Data.Repositories;
using AdminLTE.Demo.Entities;
using AdminLTE.Demo.Data;
using AdminLTE.Demo.Filters;

namespace AdminLTE.Demo.Controllers
{
    [ServiceFilter(typeof(LoginActionFilter))]
    public class UserController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        public UserController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var users = _unitOfWork.UserRepository.Get();
            return View(users);
        }

        public IActionResult GetUserByDepartment(Guid departmentId, int startPage, int pageSize)
        {
            int rowCount = 0;
            var result = _unitOfWork.UserRepository.Get();
            var roles = _unitOfWork.RoleRepository.Get();

            return Json(new
            {
                rowCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize),
                rows = result,
                roles = roles
            });
        }

        public IActionResult Edit(User user, string roles)
        {
            try
            {
                if (user.Id == Guid.Empty)
                {
                    user.Id = Guid.NewGuid();
                    var userRoles = new List<UserRole>();
                    foreach (var role in roles.Split(','))
                    {
                        userRoles.Add(new UserRole() { UserId = user.Id, RoleId = Guid.Parse(role) });
                    }
                    user.UserRoles = userRoles;
                    _unitOfWork.UserRepository.Update(user);
                }
                else
                {
                    var userRoles = new List<UserRole>();
                    foreach (var role in roles.Split(','))
                    {
                        userRoles.Add(new UserRole() { UserId = user.Id, RoleId = Guid.Parse(role) });
                    }
                    user.UserRoles = userRoles;
                    _unitOfWork.UserRepository.Add(user);
                }
                _unitOfWork.Save();
                _unitOfWork.Dispose();
                return Json(new { Result = "Success" });
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

        public IActionResult GetByID(Guid id)
        {
            var result = _unitOfWork.UserRepository.GetByID(id);
            _unitOfWork.Dispose();
            return Json(result);
        }

        public IActionResult Delete(Guid id)
        {
            try
            {
                _unitOfWork.UserRepository.Delete(id);
                _unitOfWork.Save();
                _unitOfWork.Dispose();
                return Json(new { Result = "Success" });
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

        public IActionResult DeleteMuti(string ids)
        {
            try
            {
                string[] idArray = ids.Split(',');
                List<object> delIds = new List<object>();
                foreach (string id in idArray)
                {
                    delIds.Add(Guid.Parse(id));
                }

                _unitOfWork.UserRepository.DeleteMuti(delIds);
                _unitOfWork.Save();
                _unitOfWork.Dispose();

                return Json(new
                {
                    Result="Success"
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

    }
}