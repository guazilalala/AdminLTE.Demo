using AdminLTE.Demo.Application.DataDictonaryApp.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdminLTE.Demo.Application.DataDictonaryApp
{
    public interface IDataDictonaryAppService
    {
        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <returns></returns>
        List<DataDictionaryDto> GetAllList();


        /// <summary>
        /// 新增或修改功能
        /// </summary>
        /// <param name="dto">实体</param>
        /// <returns></returns>
        bool InsertOrUpdate(DataDictionaryDto dto);

        /// <summary>
        /// 根据Id集合批量删除
        /// </summary>
        /// <param name="ids">功能Id集合</param>
        void DeleteBatch(List<Guid> ids);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">功能Id</param>
        void Delete(Guid id);

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="id">功能Id</param>
        /// <returns></returns>
        DataDictionaryDto Get(Guid id);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="startPage">起始页</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="rowCount">数据总数</param>
        /// <returns></returns>
        List<DataDictionaryDto> GetAllPageList(int startPage, int pageSize, out int rowCount);

    }
}
