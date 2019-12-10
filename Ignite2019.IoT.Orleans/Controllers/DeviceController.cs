using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ignite2019.IoT.Orleans.Grains;
using Ignite2019.IoT.Orleans.Model;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;
using WalkingTec.Mvvm.Core.Extensions;
using Ignite2019.IoT.Orleans.ViewModel.DeviceVMs;
using Orleans;

namespace Ignite2019.IoT.Orleans.Controllers
{

  [ActionDescription("设备管理")]
  public partial class DeviceController : BaseController
  {
    private readonly IClusterClient _client;

    public DeviceController(IClusterClient client)
    {
      _client = client;
    }

    [ActionDescription("激活影子设备")]
    public ActionResult ActiveShadowDevices()
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();

      var deviceIds = this.DC.Set<Device>().Select(d => d.ID).Take(10000).ToList();
      deviceIds.AsParallel().ForAll(deviceId =>
      {
            //active the grain
           var deviceGrain= _client.GetGrain<IDeviceGrain>(deviceId);
           deviceGrain.GetShadowDeviceAsync();
           //do nothing.
      });
      watch.Stop();
      return FFResult().Alert($"成功激活{ deviceIds.Count}个设备，耗时{watch.Elapsed.Seconds}s");

    }

    [ActionDescription("模拟并发创建")]
    public async Task<ActionResult> MockBatchCreate()
    {
      Stopwatch watch = new Stopwatch();
      watch.Start();
      var productIds = this.DC.Set<Product>().Select(p => p.ID).ToList();
      var random = new Random();


      var devices = Enumerable.Range(0, 10000)
          .Select(i =>
          {
            var index = random.Next(productIds.Count());
            var productId = productIds[index];
            var deviceVm = new Device();
            deviceVm.ProductId = productId;
            return deviceVm;
          }).ToList();

      var tasks = devices.Select(async d =>
      {
        var uniqueIdGenerator = _client.GetGrain<IUniqueIdGenerator>(d.ProductId);

        var newId = await uniqueIdGenerator.NewId();
        d.ID = newId;
        return d;
      });


      var newDevices = await Task.WhenAll(tasks);

      var duplicatedIds = newDevices.Select(d => d.ID).Distinct().Count();

      await this.DC.Set<Device>().AddRangeAsync(newDevices);

      var result = await this.DC.SaveChangesAsync();

      watch.Stop();
      return FFResult().Alert($"成功创建{ result}个设备，耗时{watch.Elapsed.Seconds}s");
    }

    #region 搜索
    [ActionDescription("搜索")]
    public ActionResult Index()
    {
      var vm = CreateVM<DeviceListVM>();
      return PartialView(vm);
    }

    [ActionDescription("搜索")]
    [HttpPost]
    public string Search(DeviceListVM vm)
    {
      return vm.GetJson(false);
    }

    #endregion

    #region 新建
    [ActionDescription("新建")]
    public ActionResult Create()
    {
      var vm = CreateVM<DeviceVM>();
      return PartialView(vm);
    }

    [HttpPost]
    [ActionDescription("新建")]
    public async Task<ActionResult> Create(DeviceVM vm)
    {
      var uniqueIdGenerator = _client.GetGrain<IUniqueIdGenerator>(vm.Entity.ProductId);

      var newId = await uniqueIdGenerator.NewId();
      vm.Entity.ID = newId;

      if (!ModelState.IsValid)
      {
        return PartialView(vm);
      }
      else
      {

        vm.DoAdd();
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
      var vm = CreateVM<DeviceVM>(id);
      return PartialView(vm);
    }

    [ActionDescription("修改")]
    [HttpPost]
    [ValidateFormItemOnly]
    public ActionResult Edit(DeviceVM vm)
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
      var vm = CreateVM<DeviceVM>(id);
      return PartialView(vm);
    }

    [ActionDescription("删除")]
    [HttpPost]
    public ActionResult Delete(string id, IFormCollection nouse)
    {
      var vm = CreateVM<DeviceVM>(id);
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
      var vm = CreateVM<DeviceVM>(id);
      return PartialView(vm);
    }
    #endregion

    #region 批量修改
    [HttpPost]
    [ActionDescription("批量修改")]
    public ActionResult BatchEdit(string[] IDs)
    {
      var vm = CreateVM<DeviceBatchVM>(Ids: IDs);
      return PartialView(vm);
    }

    [HttpPost]
    [ActionDescription("批量修改")]
    public ActionResult DoBatchEdit(DeviceBatchVM vm, IFormCollection nouse)
    {
      if (!ModelState.IsValid || !vm.DoBatchEdit())
      {
        return PartialView("BatchEdit", vm);
      }
      else
      {
        return FFResult().CloseDialog().RefreshGrid().Alert("操作成功，共有" + vm.Ids.Length + "条数据被修改");
      }
    }
    #endregion

    #region 批量删除
    [HttpPost]
    [ActionDescription("批量删除")]
    public ActionResult BatchDelete(string[] IDs)
    {
      var vm = CreateVM<DeviceBatchVM>(Ids: IDs);
      return PartialView(vm);
    }

    [HttpPost]
    [ActionDescription("批量删除")]
    public ActionResult DoBatchDelete(DeviceBatchVM vm, IFormCollection nouse)
    {
      if (!ModelState.IsValid || !vm.DoBatchDelete())
      {
        return PartialView("BatchDelete", vm);
      }
      else
      {
        return FFResult().CloseDialog().RefreshGrid().Alert("操作成功，共有" + vm.Ids.Length + "条数据被删除");
      }
    }
    #endregion

    #region 导入
    [ActionDescription("导入")]
    public ActionResult Import()
    {
      var vm = CreateVM<DeviceImportVM>();
      return PartialView(vm);
    }

    [HttpPost]
    [ActionDescription("导入")]
    public ActionResult Import(DeviceImportVM vm, IFormCollection nouse)
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
    public IActionResult ExportExcel(DeviceListVM vm)
    {
      vm.SearcherMode = vm.Ids != null && vm.Ids.Count > 0 ? ListVMSearchModeEnum.CheckExport : ListVMSearchModeEnum.Export;
      var data = vm.GenerateExcel();
      return File(data, "application/vnd.ms-excel", $"Export_Device_{DateTime.Now.ToString("yyyy-MM-dd")}.xls");
    }

  }
}
