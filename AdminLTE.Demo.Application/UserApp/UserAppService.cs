using System;
using System.Collections.Generic;
using System.Text;
using AdminLTE.Demo.Application.UserApp.Dtos;
using AdminLTE.Demo.Domain.Entities;
using AdminLTE.Demo.Domain.IRepositories;
using AutoMapper;

namespace AdminLTE.Demo.Application.UserApp
{
    /// <summary>
    /// 用户管理服务
    /// </summary>
    public class UserAppService : IUserAppService
    {
        /// <summary>
        /// 用户管理仓储接口
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 构造函数实现依赖注入
        /// </summary>
        /// <param name="userRepository">仓储对象</param>
        public UserAppService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public User CheckUser(string userName, string password)
        {
            return _userRepository.CheckUser(userName, password);
        }

        public void Delete(Guid id)
        {
            _userRepository.Delete(id);
        }

        public void DeleteBatch(List<Guid> ids)
        {
            _userRepository.Delete(it=>ids.Contains(it.Id));
        }

        public UserDto Get(Guid id)
        {
            return Mapper.Map<UserDto>(_userRepository.GetWithRoles(id));
        }

        public List<UserDto> GetUserByDepartment(Guid departmentId, int startPage, int pageSize, out int rowCount)
        {
            return Mapper.Map<List<UserDto>>(_userRepository.LoadPageList(startPage, pageSize, out rowCount, it => it.DepartmentId == departmentId, it => it.CreateTime));
        }

        public UserDto InsertOrUpdate(UserDto dto)
        {
            if (Get(dto.Id) !=null)
            {
                _userRepository.Delete(dto.Id);
            }
            var user = _userRepository.InsertOrUpdate(Mapper.Map<User>(dto));
            return Mapper.Map<UserDto>(user);
        }
    }
}
