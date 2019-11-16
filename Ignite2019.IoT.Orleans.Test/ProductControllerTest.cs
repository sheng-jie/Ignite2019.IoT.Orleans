using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WalkingTec.Mvvm.Core;
using Ignite2019.IoT.Orleans.Controllers;
using Ignite2019.IoT.Orleans.ViewModel.ProductVMs;
using Ignite2019.IoT.Orleans.Model;
using Ignite2019.IoT.Orleans.DataAccess;

namespace Ignite2019.IoT.Orleans.Test
{
    [TestClass]
    public class ProductControllerTest
    {
        private ProductController _controller;
        private string _seed;

        public ProductControllerTest()
        {
            _seed = Guid.NewGuid().ToString();
            _controller = MockController.CreateController<ProductController>(_seed, "user");
        }

        [TestMethod]
        public void SearchTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            string rv2 = _controller.Search(rv.Model as ProductListVM);
            Assert.IsTrue(rv2.Contains("\"Code\":200"));
        }

        [TestMethod]
        public void CreateTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Create();
            Assert.IsInstanceOfType(rv.Model, typeof(ProductVM));

            ProductVM vm = rv.Model as ProductVM;
            Product v = new Product();
			
            v.ID = 75;
            v.Name = "nXNX";
            v.CompanyId = AddCompany();
            vm.Entity = v;
            _controller.Create(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Product>().FirstOrDefault();
				
                Assert.AreEqual(data.ID, 75);
                Assert.AreEqual(data.Name, "nXNX");
                Assert.AreEqual(data.CreateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.CreateTime.Value).Seconds < 10);
            }

        }

        [TestMethod]
        public void EditTest()
        {
            Product v = new Product();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
       			
                v.ID = 75;
                v.Name = "nXNX";
                v.CompanyId = AddCompany();
                context.Set<Product>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Edit(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(ProductVM));

            ProductVM vm = rv.Model as ProductVM;
            v = new Product();
            v.ID = vm.Entity.ID;
       		
            v.Name = "WOAO2";
            vm.Entity = v;
            vm.FC = new Dictionary<string, object>();
			
            vm.FC.Add("Entity.ID", "");
            vm.FC.Add("Entity.Name", "");
            vm.FC.Add("Entity.CompanyId", "");
            _controller.Edit(vm);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                var data = context.Set<Product>().FirstOrDefault();
 				
                Assert.AreEqual(data.Name, "WOAO2");
                Assert.AreEqual(data.UpdateBy, "user");
                Assert.IsTrue(DateTime.Now.Subtract(data.UpdateTime.Value).Seconds < 10);
            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            Product v = new Product();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
        		
                v.ID = 75;
                v.Name = "nXNX";
                v.CompanyId = AddCompany();
                context.Set<Product>().Add(v);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.Delete(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(ProductVM));

            ProductVM vm = rv.Model as ProductVM;
            v = new Product();
            v.ID = vm.Entity.ID;
            vm.Entity = v;
            _controller.Delete(v.ID.ToString(),null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<Product>().Count(), 1);
            }

        }


        [TestMethod]
        public void DetailsTest()
        {
            Product v = new Product();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v.ID = 75;
                v.Name = "nXNX";
                v.CompanyId = AddCompany();
                context.Set<Product>().Add(v);
                context.SaveChanges();
            }
            PartialViewResult rv = (PartialViewResult)_controller.Details(v.ID.ToString());
            Assert.IsInstanceOfType(rv.Model, typeof(IBaseCRUDVM<TopBasePoco>));
            Assert.AreEqual(v.ID, (rv.Model as IBaseCRUDVM<TopBasePoco>).Entity.GetID());
        }

        [TestMethod]
        public void BatchDeleteTest()
        {
            Product v1 = new Product();
            Product v2 = new Product();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
				
                v1.ID = 75;
                v1.Name = "nXNX";
                v1.CompanyId = AddCompany();
                v2.Name = "WOAO2";
                v2.CompanyId = v1.CompanyId; 
                context.Set<Product>().Add(v1);
                context.Set<Product>().Add(v2);
                context.SaveChanges();
            }

            PartialViewResult rv = (PartialViewResult)_controller.BatchDelete(new string[] { v1.ID.ToString(), v2.ID.ToString() });
            Assert.IsInstanceOfType(rv.Model, typeof(ProductBatchVM));

            ProductBatchVM vm = rv.Model as ProductBatchVM;
            vm.Ids = new string[] { v1.ID.ToString(), v2.ID.ToString() };
            _controller.DoBatchDelete(vm, null);

            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {
                Assert.AreEqual(context.Set<Product>().Count(), 2);
            }
        }

        [TestMethod]
        public void ExportTest()
        {
            PartialViewResult rv = (PartialViewResult)_controller.Index();
            Assert.IsInstanceOfType(rv.Model, typeof(IBasePagedListVM<TopBasePoco, BaseSearcher>));
            IActionResult rv2 = _controller.ExportExcel(rv.Model as ProductListVM);
            Assert.IsTrue((rv2 as FileContentResult).FileContents.Length > 0);
        }

        private Int32 AddCompany()
        {
            Company v = new Company();
            using (var context = new DataContext(_seed, DBTypeEnum.Memory))
            {

                v.ID = 83;
                context.Set<Company>().Add(v);
                context.SaveChanges();
            }
            return v.ID;
        }


    }
}
