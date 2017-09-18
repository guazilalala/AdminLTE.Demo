using AdminLTE.Demo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminLTE.Demo.Domain.IRepositories
{
    /// <summary>
    /// 数据字典仓储接口
    /// </summary>
    public interface IDataDictionaryRepository: IRepository<DataDictionary,Guid>
    {
    }
}
