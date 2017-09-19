using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminLTE.Demo.Application.DataDictonaryApp;
using AdminLTE.Demo.MVC.Models;
using AdminLTE.Demo.Application.DataDictonaryApp.Dtos;
using AdminLTE.Demo.Domain.IRepositories;

namespace AdminLTE.Demo.MVC.Controllers
{
    public class DataDictionaryController : BaseController
    {
        private readonly IDataDictonaryAppService _dataDictionaryAppService;
        private readonly IDataDictionaryRepository _dataDictionaryRepository;

        public DataDictionaryController(IDataDictonaryAppService dataDictionaryAppService, IDataDictionaryRepository dataDictionaryRepository)
        {
            _dataDictionaryAppService = dataDictionaryAppService;
            _dataDictionaryRepository = dataDictionaryRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllPageList(int startPage, int pageSize)
        {
            int rowCount = 0;
            var result = _dataDictionaryAppService.GetAllPageList(startPage, pageSize, out rowCount);
            return Json(new
            {
                rowCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize),
                rows = result,
            });
        }

        /// <summary>
        /// 通过id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPageListByGuid(Guid id, int startPage, int pageSize)
        {
            int rowCount = 0;

            var result = _dataDictionaryAppService.GetListById(id, startPage, pageSize, out rowCount);

            var isRootList = result.Where(p => p.ParentId == Guid.Empty).Any();

            if (result.Count < 1)
            {
                isRootList = false;
            }
            return Json(new
            {
                rowCount = rowCount,
                pageCount = Math.Ceiling(Convert.ToDecimal(rowCount) / pageSize),
                rows = result,
                isRootList = isRootList,
            });
        }

        /// <summary>
        /// 获取字典树JSON数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDictionaryTreeData()
        {
            var dics = _dataDictionaryAppService.GetAllList();
            List<TreeModel> treeModels = new List<TreeModel>();
            foreach (var dic in dics.Where(p => p.ParentId == Guid.Empty))
            {
                treeModels.Add(new TreeModel
                {
                    Id = dic.Id.ToString(),
                    Text = dic.Name,
                    Parent = dic.ParentId == Guid.Empty ? "#" : dic.ParentId.ToString()
                });
            }

            ViewBag.TreeModels = treeModels;
            return Json(treeModels);

        }

        /// <summary>
        /// 新增或编辑功能
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IActionResult Edit(DataDictionaryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    Result = "Faild",
                    Message = GetModelStateError()
                });
            }

            if (_dataDictionaryRepository.IsExist(p => p.Name == dto.Name && p.ParentId == Guid.Empty) && dto.Id == Guid.Empty && dto.ParentId==Guid.Empty)
            {
                return Json(new { Result = "Faild",
                    Message = "根字典名称已存在"
                });
            }
            if (_dataDictionaryAppService.InsertOrUpdate(dto))
            {
                return Json(new { Result = "Success" });
            }

            return Json(new
            {
                Result = "Faild",
                Message = "保存失败"
            });
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            try
            {
                if (_dataDictionaryAppService.HasChild(id))
                {
                    return Json(new
                    {
                        Result = "Faild",
                        Message = "该节点含有子节点，请先删除子节点"
                    });
                }

                _dataDictionaryAppService.Delete(id);
                return Json(new
                {
                    Result = "Success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Faild",
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult DeleteMuti(string ids)
        {
            try
            {
                string[] idArray = ids.Split(',');
                List<Guid> delIds = new List<Guid>();
                foreach (string id in idArray)
                {
                    delIds.Add(Guid.Parse(id));
                }
                _dataDictionaryAppService.DeleteBatch(delIds);
                return Json(new
                {
                    Result = "Success"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "Faild",
                    Message = ex.Message
                });
            }
        }

        public IActionResult Get(Guid id)
        {
            var dto = _dataDictionaryAppService.Get(id);
            return Json(dto);
        }
    }
}