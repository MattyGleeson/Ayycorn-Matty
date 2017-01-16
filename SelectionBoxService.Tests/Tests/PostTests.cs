using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SelectionBoxService.Controllers;
using Moq;
using System.Data.Entity;
using SelectionBoxService.Data;
using SelectionBoxService.Tests.Data;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;

namespace SelectionBoxService.Tests.Tests
{
    [TestClass]
    public class PostTests : GenericTest
    {
        [TestMethod]
        public async Task ServiceTestPostNoProducts()
        {
            HttpResponseMessage response = await controller.PostSelectionBox(new LibAyycorn.Dtos.Giftbox
            {
                Id = 7,
                Total = 10.0,
                WrappingId = 1,
                WrappingRangeId = 1,
                WrappingRangeName = "Swirl-Tastic",
                WrappingTypeId = 1,
                WrappingTypeName = "Gift Bag",
                Removed = false,
                Visible = true,
                Available = true
            });

            Assert.IsTrue(response.IsSuccessStatusCode);

            mockBoxesSet.Verify(m => m.Add(It.IsAny<SelectionBox>()), Times.Once);
            mockDb.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task ServiceTestPostSingleProduct()
        {
            HttpResponseMessage response = await controller.PostSelectionBox(new LibAyycorn.Dtos.Giftbox
            {
                Id = 7,
                Total = 10.0,
                WrappingId = 1,
                WrappingRangeId = 1,
                WrappingRangeName = "Swirl-Tastic",
                WrappingTypeId = 1,
                WrappingTypeName = "Gift Bag",
                Removed = false,
                Visible = true,
                Available = true,
                Products = new List<LibAyycorn.Dtos.Product>()
                {
                    new LibAyycorn.Dtos.Product
                    {
                        Id = 4,
                        Name = "Product4",
                        StoreName = "TestStore"
                    }
                }
            });

            Assert.IsTrue(response.IsSuccessStatusCode);

            mockBoxesSet.Verify(m => m.Add(It.IsAny<SelectionBox>()), Times.Once);
            mockBoxProductsSet.Verify(m => m.Add(It.IsAny<SelectionBoxProduct>()), Times.Once);
            mockProductsSet.Verify(m => m.Add(It.IsAny<Product>()), Times.Once);
            mockDb.Verify(m => m.SaveChangesAsync(), Times.Exactly(3));
        }

        [TestMethod]
        public async Task ServiceTestPostMultipleProducts()
        {
            HttpResponseMessage response = await controller.PostSelectionBox(new LibAyycorn.Dtos.Giftbox
            {
                Id = 8,
                Total = 10.0,
                WrappingId = 1,
                WrappingRangeId = 1,
                WrappingRangeName = "Swirl-Tastic",
                WrappingTypeId = 1,
                WrappingTypeName = "Gift Bag",
                Removed = false,
                Visible = true,
                Available = true,
                Products = new List<LibAyycorn.Dtos.Product>()
                {
                    new LibAyycorn.Dtos.Product
                    {
                        Id = 5,
                        Name = "Product5",
                        StoreName = "TestStore"
                    },
                    new LibAyycorn.Dtos.Product
                    {
                        Id = 6,
                        Name = "Product6",
                        StoreName = "TestStore"
                    }
                }
            });

            Assert.IsTrue(response.IsSuccessStatusCode);

            mockBoxesSet.Verify(m => m.Add(It.IsAny<SelectionBox>()), Times.Once);
            mockBoxProductsSet.Verify(m => m.Add(It.IsAny<SelectionBoxProduct>()), Times.Exactly(2));
            mockProductsSet.Verify(m => m.Add(It.IsAny<Product>()), Times.Exactly(2));
            mockDb.Verify(m => m.SaveChangesAsync(), Times.Exactly(5));
        }

        [TestMethod]
        public async Task ServiceTestPostExistingProduct()
        {
            HttpResponseMessage response = await controller.PostSelectionBox(new LibAyycorn.Dtos.Giftbox
            {
                Id = 9,
                Total = 10.0,
                WrappingId = 1,
                WrappingRangeId = 1,
                WrappingRangeName = "Swirl-Tastic",
                WrappingTypeId = 1,
                WrappingTypeName = "Gift Bag",
                Removed = false,
                Visible = true,
                Available = true,
                Products = new List<LibAyycorn.Dtos.Product>()
                {
                    new LibAyycorn.Dtos.Product
                    {
                        Id = 2,
                        Name = "Product2",
                        StoreName = "TestStore"
                    }
                }
            });

            Assert.IsTrue(response.IsSuccessStatusCode);

            mockBoxesSet.Verify(m => m.Add(It.IsAny<SelectionBox>()), Times.Once);
            mockBoxProductsSet.Verify(m => m.Add(It.IsAny<SelectionBoxProduct>()), Times.Once);
            mockProductsSet.Verify(m => m.Add(It.IsAny<Product>()), Times.Never);
            mockDb.Verify(m => m.SaveChangesAsync(), Times.Exactly(2));
        }

        [TestMethod]
        public async Task ServiceTestPostNull()
        {
            HttpResponseMessage response = await controller.PostSelectionBox(null);

            Assert.IsTrue(!response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }
    }
}
