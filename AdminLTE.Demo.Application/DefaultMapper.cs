using AdminLTE.Demo.Application.DataDictonaryApp.Dtos;
using AdminLTE.Demo.Application.DepartmentApp.Dtos;
using AdminLTE.Demo.Application.MenuApp.Dtos;
using AdminLTE.Demo.Application.RoleApp.Dtos;
using AdminLTE.Demo.Application.UserApp.Dtos;
using AdminLTE.Demo.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminLTE.Demo.Application
{
    /// <summary>
    /// Enity与Dto映射
    /// </summary>
   public class DefaultMapper
    {
        /// <summary>
        /// 初始化映射关系
        /// </summary>
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Menu, MenuDto>();
                cfg.CreateMap<MenuDto, Menu>();
                cfg.CreateMap<Department, DepartmentDto>();
                cfg.CreateMap<DepartmentDto, Department>();
                cfg.CreateMap<RoleDto, Role>();
                cfg.CreateMap<Role, RoleDto>();
                cfg.CreateMap<RoleMenuDto, RoleMenu>();
                cfg.CreateMap<RoleMenu, RoleMenuDto>();
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserRoleDto, UserRole>();
                cfg.CreateMap<UserRole, UserRoleDto>();
                cfg.CreateMap<DataDictionary, DataDictionaryDto>();
                cfg.CreateMap<DataDictionaryDto, DataDictionary>();
            });
        }

    }
}
