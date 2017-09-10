using AdminLTE.Demo.Domain.Entities;
using System;

namespace AdminLTE.Demo.Domain.IRepositories
{
	/// <summary>
	/// 用户管理仓储接口
	/// </summary>
	public interface IUserRepository :IBaseRepository
    {
        /// <summary>
        /// 检查用户是存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>存在返回用户实体，否则返回NULL</returns>
        User CheckUser(string userName, string password);
        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        User GetWithRoles(Guid id);
    }
}
