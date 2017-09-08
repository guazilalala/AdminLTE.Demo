using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.Demo.Data.IRepositories
{
    public interface IBaseRepository:IDisposable
    {
        int Save();
        Task<int> SaveAsync();
    }
}
