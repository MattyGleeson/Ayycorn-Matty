using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SelectionBoxService.Controllers;
using SelectionBoxService.Data;
using SelectionBoxService.Tests.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SelectionBoxService.Tests.Tests
{
    public class GenericTest
    {
        public SelectionBoxController controller;
        public Mock<AyycornDb> mockDb;
        public Mock<DbSet<SelectionBox>> mockBoxesSet;
        public Mock<DbSet<Product>> mockProductsSet;
        public Mock<DbSet<SelectionBoxProduct>> mockBoxProductsSet;
        public SampleData data;

        [TestInitialize]
        public void Init()
        {
            data = new SampleData();
            mockBoxesSet = data.boxes;
            mockProductsSet = data.products;
            mockBoxProductsSet = data.boxProducts;
            mockDb = data.Context();

            controller = new SelectionBoxController(mockDb.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }
    }
}
