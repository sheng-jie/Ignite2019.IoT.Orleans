using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using Ignite2019.IoT.Orleans.Controllers;
using Ignite2019.IoT.Orleans.ViewModel.DeviceVMs;
using Ignite2019.IoT.Orleans.Model;
using Ignite2019.IoT.Orleans.DataAccess;

namespace Ignite2019.IoT.Orleans.Test
{
    [TestClass]
    public class DeviceControllerTest
    {
        private DeviceController _controller;
        private string _seed;

        public DeviceControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<DeviceController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as DeviceListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(DeviceVM));

            DeviceVM vm = rv.Model as DeviceVM;
            Device v = new Device();
			
            v.ID = "44tvMxOP9";
            v.ProductId = AddProduct();
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Device>().FirstOrDefault();
				
                Assert.AreEqual(data.ID, "44tvMxOP9");
            }

        }

        [TestMethod]
        public void EditTest()
        {
            Device v = new Device();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.ID = "44tvMxOP9";
                v.ProductId = AddProduct();
                context.Set<Device>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(DeviceVM));

            DeviceVM vm = rv.Model as DeviceVM;
            v = new Device();
            v.ID = vm.Entity.ID;
       		
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.ID", "");
            vm.FC.Add("Entity.ProductId", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Device>().FirstOrDefault();
 				
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            Device v = new Device();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.ID = "44tvMxOP9";
                v.ProductId = AddProduct();
                context.Set<Device>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(DeviceVM));

            DeviceVM vm = rv.Model as DeviceVM;
            v = new Device();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<Device>().Count(), 0);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            Device v = new Device();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.ID = "44tvMxOP9";
                v.ProductId = AddProduct();
                context.Set<Device>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            Device v1 = new Device();
            Device v2 = new Device();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.ID = "44tvMxOP9";
                v1.ProductId = AddProduct();
                v2.ProductId = v1.ProductId; 
                context.Set<Device>().Add(v1);
                context.Set<Device>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(DeviceBatchVM));

            DeviceBatchVM vm = rv.Model as DeviceBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<Device>().Count(), 0);
            }
        }

        private Int32 AddProduct()
        {
            Product v = new Product();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {

                v.ID = 83;
                v.Name = "UPZo";
                v.CompanyId = $fk$;
                context.Set<Product>().Add(v);
                context.SaveChanges();
            }
            return v.ID;
        }


    }
}
