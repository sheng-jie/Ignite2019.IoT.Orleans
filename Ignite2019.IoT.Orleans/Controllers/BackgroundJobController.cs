﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Model;
using Ignite2019.IoT.Orleans.Reminders;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.ViewModel.BackgroundJobVMs;
using Orleans;

namespace Ignite2019.IoT.Orleans.Controllers
{
    
    [ActionDescription("后台任务")]
    public partial class BackgroundJobController : BaseController
    {  private readonly IClusterClient _client;

        public BackgroundJobController(IClusterClient client)
        {
            _client = client;
        }
        #region 搜索
        [ActionDescription("搜索")]
        public ActionResult Index()
        {
            var vm = CreateVM<BackgroundJobListVM>();
            return PartialView(vm);
        }

        [ActionDescription("搜索")]
        [HttpPost]
        public string Search(BackgroundJobListVM vm)
        {
            return vm.GetJson(false);
        }

        #endregion

        #region 新建
        [ActionDescription("新建")]
        public ActionResult Create()
        {
            var vm = CreateVM<BackgroundJobVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("新建")]
        public async Task<ActionResult> Create(BackgroundJobVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                var backgroupJobGrain = _client.GetGrain<IBackgroundJobGrain>(Guid.NewGuid(), vm.Entity.DeviceId);

                if (vm.Entity.JobType==BackgroundJobType.Timer)
                {
                    await backgroupJobGrain.CreateTimer(vm.Entity.Command,
                        new JobPeriod(vm.Entity.StartTime, vm.Entity.Period, vm.Entity.EndTime.Value));
                }
                else
                {
                    await backgroupJobGrain.CreateReminder(vm.Entity.Command,
                        new JobPeriod(vm.Entity.StartTime, vm.Entity.Period, vm.Entity.EndTime.Value));
                }
                //vm.DoAdd();
                
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGrid();
                }
            }
        }
        #endregion

        #region 修改
        [ActionDescription("修改")]
        public ActionResult Edit(string id)
        {
            var vm = CreateVM<BackgroundJobVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("修改")]
        [HttpPost]
        [ValidateFormItemOnly]
        public ActionResult Edit(BackgroundJobVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoEdit();
                if (!ModelState.IsValid)
                {
                    vm.DoReInit();
                    return PartialView(vm);
                }
                else
                {
                    return FFResult().CloseDialog().RefreshGridRow(vm.Entity.ID);
                }
            }
        }
        #endregion

        #region 删除
        [ActionDescription("删除")]
        public ActionResult Delete(string id)
        {
            var vm = CreateVM<BackgroundJobVM>(id);
            return PartialView(vm);
        }

        [ActionDescription("删除")]
        [HttpPost]
        public ActionResult Delete(string id, IFormCollection nouse)
        {
            var vm = CreateVM<BackgroundJobVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid();
            }
        }
        #endregion

        #region 详细
        [ActionDescription("详细")]
        public ActionResult Details(string id)
        {
            var vm = CreateVM<BackgroundJobVM>(id);
            return PartialView(vm);
        }
        #endregion

        #region 批量修改
        [HttpPost]
        [ActionDescription("批量修改")]
        public ActionResult BatchEdit(string[] IDs)
        {
            var vm = CreateVM<BackgroundJobBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("批量修改")]
        public ActionResult DoBatchEdit(BackgroundJobBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchEdit())
            {
                return PartialView("BatchEdit",vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert("操作成功，共有"+vm.Ids.Length+"条数据被修改");
            }
        }
        #endregion

        #region 批量删除
        [HttpPost]
        [ActionDescription("批量删除")]
        public ActionResult BatchDelete(string[] IDs)
        {
            var vm = CreateVM<BackgroundJobBatchVM>(Ids: IDs);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("批量删除")]
        public ActionResult DoBatchDelete(BackgroundJobBatchVM vm, IFormCollection nouse)
        {
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return PartialView("BatchDelete",vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert("操作成功，共有"+vm.Ids.Length+"条数据被删除");
            }
        }
        #endregion

        #region 导入
		[ActionDescription("导入")]
        public ActionResult Import()
        {
            var vm = CreateVM<BackgroundJobImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("导入")]
        public ActionResult Import(BackgroundJobImportVM vm, IFormCollection nouse)
        {
            if (vm.ErrorListVM.EntityList.Count > 0 || !vm.BatchSaveData())
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert("成功导入 " + vm.EntityList.Count.ToString() + " 行数据");
            }
        }
        #endregion

        [ActionDescription("导出")]
        [HttpPost]
        public IActionResult ExportExcel(BackgroundJobListVM vm)
        {
            vm.SearcherMode = vm.Ids != null && vm.Ids.Count > 0 ? ListVMSearchModeEnum.CheckExport : ListVMSearchModeEnum.Export;
            var data = vm.GenerateExcel();
            return File(data, "application/vnd.ms-excel", $"Export_BackgroundJob_{DateTime.Now.ToString("yyyy-MM-dd")}.xls");
        }

    }
}
