using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminLTE.Demo.Application.DataDictonaryApp;
using AdminLTE.Demo.MVC.Models;
using AdminLTE.Demo.Application.DataDictonaryApp.Dtos;

namespace AdminLTE.Demo.MVC.Controllers
{
    public class DataDictionaryController : BaseController
    {
        private readonly IDataDictonaryAppService _dataDictionaryAppService;
        public DataDictionaryController(IDataDictonaryAppService dataDictionaryAppService)
        {
            _dataDictionaryAppService = dataDictionaryAppService;
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
        /// 获取字典树JSON数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDictionaryTreeData()
        {
            var dics = _dataDictionaryAppService.GetAllList();
            List<TreeModel> treeModels = new List<TreeModel>();
            foreach (var dic in dics)
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

            if (_dataDictionaryAppService.InsertOrUpdate(dto))
            {
                return Json(new { Result = "Success" });
            }

            return Json(new { Result = "Faild" });
        }

        public IActionResult Delete(Guid id)
        {
            try
            {
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

        public IActionResult Get(Guid id)
        {
            var dto = _dataDictionaryAppService.Get(id);
            return Json(dto);
        }
    }
}