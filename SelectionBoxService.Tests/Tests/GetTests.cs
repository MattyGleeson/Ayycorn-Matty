using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SelectionBoxService.Controllers;
using Moq;
using System.Data.Entity;
using SelectionBoxService.Data;
using SelectionBoxService.Tests.Data;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SelectionBoxService.Tests.Tests
{
    [TestClass]
    public class GetTests : GenericTest
    {
        [TestMethod]
        public async Task ServiceTestGet()
        {
            HttpResponseMessage response = await Controller.GetAllSelectionBoxes();

            IEnumerable<LibAyycorn.Dtos.Giftbox> selectionBoxes;
            IEnumerable<SelectionBox> dbSelectionBoxes = MockDb.Object.SelectionBoxes.ToList();

            Assert.IsTrue(response.TryGetContentValue(out selectionBoxes));
            Assert.AreEqual(selectionBoxes.Count(), dbSelectionBoxes.Count());
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task ServiceTestGetNoData()
        {
            SampleData Data = new SampleData(false);

            Mock<AyycornDb> MockDb = Data.Context();
            Mock<DbSet<SelectionBox>> MockBoxesSet = Data.Boxes;
            Mock<DbSet<Product>> MockProductsSet = Data.Products;
            Mock<DbSet<SelectionBoxProduct>> MockBoxProductsSet = Data.BoxProducts;
            
            SelectionBoxController Controller = new SelectionBoxController(MockDb.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            HttpResponseMessage response = await Controller.GetAllSelectionBoxes();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }
    }
}
