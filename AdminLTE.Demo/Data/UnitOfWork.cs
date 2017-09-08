using AdminLTE.Demo.Data.Repositories;
using AdminLTE.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.Demo.Data
{
    /// <summary>
    ///仓储工作单元
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        private DefaultContext _dbContext;

        private GenericRepository<User> userRepository;
        private GenericRepository<Role> roleRepository;
        private GenericRepository<Menu> menuRepository;

        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _dbContext = new DefaultContext(serviceProvider.GetRequiredService<DbContextOptions<DefaultContext>>());
        }

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<User>(_dbContext);
                }
                return userRepository;
            }
        }
        public GenericRepository<Role> RoleRepository
        {
            get
            {
                if (this.roleRepository == null)
                {
                    this.roleRepository = new GenericRepository<Role>(_dbContext);
                }
                return roleRepository;
            }
        }
        public GenericRepository<Menu> MenuRepository
        {
            get
            {
                if (this.menuRepository == null)
                {
                    this.menuRepository = new GenericRepository<Menu>(_dbContext);
                }
                return menuRepository;
            }
        }

        private bool disposed = false;

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
    }
}
