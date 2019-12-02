using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using Ignite2019.IoT.Orleans.Controllers;
using Ignite2019.IoT.Orleans.ViewModel.EventHistoryVMs;
using Ignite2019.IoT.Orleans.Model;
using Ignite2019.IoT.Orleans.DataAccess;

namespace Ignite2019.IoT.Orleans.Test
{
    [TestClass]
    public class EventHistoryControllerTest
    {
        private EventHistoryController _controller;
        private string _seed;

        public EventHistoryControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            //_controller = MockController.CreateController<EventHistoryController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as EventHistoryListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(EventHistoryVM));

            EventHistoryVM vm = rv.Model as EventHistoryVM;
            EventHistory v = new EventHistory();
			
            v.ID = 69;
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<EventHistory>().FirstOrDefault();
				
                Assert.AreEqual(data.ID, 69);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            EventHistory v = new EventHistory();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.ID = 69;
                context.Set<EventHistory>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(EventHistoryVM));

            EventHistoryVM vm = rv.Model as EventHistoryVM;
            v = new EventHistory();
            v.ID = vm.Entity.ID;
       		
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.ID", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<EventHistory>().FirstOrDefault();
 				
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            EventHistory v = new EventHistory();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.ID = 69;
                context.Set<EventHistory>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(EventHistoryVM));

            EventHistoryVM vm = rv.Model as EventHistoryVM;
            v = new EventHistory();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<EventHistory>().Count(), 0);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            EventHistory v = new EventHistory();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.ID = 69;
                context.Set<EventHistory>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            EventHistory v1 = new EventHistory();
            EventHistory v2 = new EventHistory();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.ID = 69;
                context.Set<EventHistory>().Add(v1);
                context.Set<EventHistory>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(EventHistoryBatchVM));

            EventHistoryBatchVM vm = rv.Model as EventHistoryBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<EventHistory>().Count(), 0);
            }
        }


    }
}
