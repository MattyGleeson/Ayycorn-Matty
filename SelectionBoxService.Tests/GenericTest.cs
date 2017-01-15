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
        public SelectionBoxController Controller;
        public Mock<AyycornDb> MockDb;
        public Mock<DbSet<SelectionBox>> MockBoxesSet;
        public Mock<DbSet<Product>> MockProductsSet;
        public Mock<DbSet<SelectionBoxProduct>> MockBoxProductsSet;
        public SampleData Data;

        [TestInitialize]
        public void Init()
        {
            Data = new SampleData();
            MockBoxesSet = Data.Boxes;
            MockProductsSet = Data.Products;
            MockBoxProductsSet = Data.BoxProducts;
            MockDb = Data.Context();

            Controller = new SelectionBoxController(MockDb.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }
    }
}
