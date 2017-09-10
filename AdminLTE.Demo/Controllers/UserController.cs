using AdminLTE.Demo.Domain.Entities;
using AdminLTE.Demo.Domain.IRepositories;
using AdminLTE.Demo.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AdminLTE.Demo.Controllers
{
	[ServiceFilter(typeof(LoginActionFilter))]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
			_userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetUserByDepartment(Guid departmentId, int startPage, int pageSize)
        {
			return null;
        }

        public IActionResult Edit(User user, string roles)
        {
			return null;
		}

		public IActionResult GetByID(Guid id)
        {
			return null;
		}

		public IActionResult Delete(Guid id)
        {
			return null;
		}

		public IActionResult DeleteMuti(string ids)
        {
			return null;
		}

	}
}