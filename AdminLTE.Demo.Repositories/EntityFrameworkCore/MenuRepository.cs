using AdminLTE.Demo.Domain.Entities;
using AdminLTE.Demo.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminLTE.Demo.Repositories.EntityFrameworkCore
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(DefaultDbContext dbcontext) : base(dbcontext)
        {

        }
    }

}
