using AdminLTE.Demo.Domain.Entities;
using AdminLTE.Demo.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminLTE.Demo.Repositories.EntityFrameworkCore
{
    public class DataDictionaryRepository:BaseRepository<DataDictionary>, IDataDictionaryRepository
    {
        /// <summary>
        /// 数据字典仓储实现
        /// </summary>
        /// <param name="dbContext"></param>
        public DataDictionaryRepository(DefaultDbContext dbContext) : base(dbContext)
        {

        }
    }
}
