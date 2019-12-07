using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using Ignite2019.IoT.Orleans.Controllers;
using Ignite2019.IoT.Orleans.ViewModel.BackgroundJobVMs;
using Ignite2019.IoT.Orleans.Model;
using Ignite2019.IoT.Orleans.DataAccess;

namespace Ignite2019.IoT.Orleans.Test
{
    [TestClass]
    public class BackgroundJobControllerTest
    {
        private BackgroundJobController _controller;
        private string _seed;

        public BackgroundJobControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
//            _controller = MockController.CreateController<BackgroundJobController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as BackgroundJobListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(BackgroundJobVM));

            BackgroundJobVM vm = rv.Model as BackgroundJobVM;
            BackgroundJob v = new BackgroundJob();
			
            v.DeviceId = "Erut2";
            v.Command = "YVMBjEd";
            v.Period = 63;
            v.ExecutedCount = 98;
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<BackgroundJob>().FirstOrDefault();
				
                Assert.AreEqual(data.DeviceId, "Erut2");
                Assert.AreEqual(data.Command, "YVMBjEd");
                Assert.AreEqual(data.Period, 63);
                Assert.AreEqual(data.ExecutedCount, 98);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            BackgroundJob v = new BackgroundJob();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.DeviceId = "Erut2";
                v.Command = "YVMBjEd";
                v.Period = 63;
                v.ExecutedCount = 98;
                context.Set<BackgroundJob>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(BackgroundJobVM));

            BackgroundJobVM vm = rv.Model as BackgroundJobVM;
            v = new BackgroundJob();
            v.ID = vm.Entity.ID;
       		
            v.DeviceId = "MtCMB5LAV";
            v.Command = "mof";
            v.Period = 15;
            v.ExecutedCount = 52;
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.DeviceId", "");
            vm.FC.Add("Entity.Command", "");
            vm.FC.Add("Entity.Period", "");
            vm.FC.Add("Entity.ExecutedCount", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<BackgroundJob>().FirstOrDefault();
 				
                Assert.AreEqual(data.DeviceId, "MtCMB5LAV");
                Assert.AreEqual(data.Command, "mof");
                Assert.AreEqual(data.Period, 15);
                Assert.AreEqual(data.ExecutedCount, 52);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            BackgroundJob v = new BackgroundJob();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.DeviceId = "Erut2";
                v.Command = "YVMBjEd";
                v.Period = 63;
                v.ExecutedCount = 98;
                context.Set<BackgroundJob>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(BackgroundJobVM));

            BackgroundJobVM vm = rv.Model as BackgroundJobVM;
            v = new BackgroundJob();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<BackgroundJob>().Count(), 0);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            BackgroundJob v = new BackgroundJob();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.DeviceId = "Erut2";
                v.Command = "YVMBjEd";
                v.Period = 63;
                v.ExecutedCount = 98;
                context.Set<BackgroundJob>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            BackgroundJob v1 = new BackgroundJob();
            BackgroundJob v2 = new BackgroundJob();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.DeviceId = "Erut2";
                v1.Command = "YVMBjEd";
                v1.Period = 63;
                v1.ExecutedCount = 98;
                v2.DeviceId = "MtCMB5LAV";
                v2.Command = "mof";
                v2.Period = 15;
                v2.ExecutedCount = 52;
                context.Set<BackgroundJob>().Add(v1);
                context.Set<BackgroundJob>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(BackgroundJobBatchVM));

            BackgroundJobBatchVM vm = rv.Model as BackgroundJobBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<BackgroundJob>().Count(), 0);
            }
        }


    }
}
