using AdminLTE.Demo.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AdminLTE.Demo.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DefaultContext _dbContext;
        public UserRepository(IServiceProvider serviceProvider)
        {
            _dbContext = new DefaultContext(serviceProvider.GetRequiredService<DbContextOptions<DefaultContext>>());
        }

        /// <summary>
        /// 检查用户是存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>存在返回用户实体，否则返回NULL</returns>
        public User CheckUser(string userName, string password)
        {
            return _dbContext.Set<User>().FirstOrDefault(p => p.UserName == userName &&
                    p.Password == password);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public User GetWithRoles(Guid id)
        {
            var user = _dbContext.Set<User>().FirstOrDefault(p => p.Id == id);

            if (user != null)
            {
                user.UserRoles = _dbContext.Set<UserRole>().Where(p => p.UserId == id).ToList();
            }

            return user;
        }

        public int Save()
        {
          return _dbContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
