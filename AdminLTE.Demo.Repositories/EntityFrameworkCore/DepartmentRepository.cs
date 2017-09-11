using AdminLTE.Demo.Domain.Entities;
using AdminLTE.Demo.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminLTE.Demo.Repositories.EntityFrameworkCore
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(DefaultDbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
