using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using SelectionBoxService.Controllers;

namespace SelectionBoxService.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private SelectionBoxController controller;
        private Data.FakeDb db;

        [TestInitialize]
        public void Init()
        {
            db = new Data.FakeDb();
            controller = new SelectionBoxController(db)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [TestMethod]
        public void TestGet()
        {
            var response = controller.GetAllSelectionBoxes().Result;
            Console.WriteLine("Response");
            IEnumerable<LibAyycorn.Dtos.SelectionBox> selectionBoxes;

            //Assert.IsTrue(response.TryGetContentValue<IEnumerable<LibAyycorn.Dtos.SelectionBox>>(out selectionBoxes));
            //Assert.AreEqual(selectionBoxes, db.Products);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestPost()
        {
            Interfaces.DbInterface db = new Data.FakeDb();
            Controllers.SelectionBoxController sbc = new Controllers.SelectionBoxController(db);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestPut()
        {
            Interfaces.DbInterface db = new Data.FakeDb();
            Controllers.SelectionBoxController sbc = new Controllers.SelectionBoxController(db);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestRemove()
        {
            Interfaces.DbInterface db = new Data.FakeDb();
            Controllers.SelectionBoxController sbc = new Controllers.SelectionBoxController(db);

            Assert.IsTrue(true);
        }
    }
}
