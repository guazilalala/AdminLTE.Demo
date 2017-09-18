using System;
using System.Collections.Generic;
using System.Text;
using AdminLTE.Demo.Application.DataDictonaryApp.Dtos;
using AdminLTE.Demo.Domain.IRepositories;
using AutoMapper;
using System.Linq;
using AdminLTE.Demo.Domain.Entities;

namespace AdminLTE.Demo.Application.DataDictonaryApp
{
    public class DataDictonaryAppService : IDataDictonaryAppService
    {
        private readonly IDataDictionaryRepository _dataDictonaryRepository;
        public DataDictonaryAppService(IDataDictionaryRepository dataDictonaryRepository)
        {
            _dataDictonaryRepository = dataDictonaryRepository;
        }
        public void Delete(Guid id)
        {
            _dataDictonaryRepository.Delete(id);
        }

        public void DeleteBatch(List<Guid> ids)
        {
            _dataDictonaryRepository.Delete(it => ids.Contains(it.Id));
        }

        public DataDictionaryDto Get(Guid id)
        {
            return Mapper.Map<DataDictionaryDto>(_dataDictonaryRepository.Get(id));
        }

        public List<DataDictionaryDto> GetAllList()
        {
            var dataDictonary = _dataDictonaryRepository.GetAllList().OrderBy(it => it.SerialNumber);

            return Mapper.Map<List<DataDictionaryDto>>(dataDictonary);
        }

        public List<DataDictionaryDto> GetAllPageList(int startPage, int pageSize, out int rowCount)
        {
            return Mapper.Map<List<DataDictionaryDto>>(_dataDictonaryRepository.LoadPageList(startPage, pageSize, out rowCount, null, it => it.Code));
        }

        public bool InsertOrUpdate(DataDictionaryDto dto)
        {
            var dataDictonary = _dataDictonaryRepository.InsertOrUpdate(Mapper.Map<DataDictionary>(dto));

            return dataDictonary == null ? false : true;
        }
    }
}
