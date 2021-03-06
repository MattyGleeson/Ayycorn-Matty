﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SelectionBoxService.Controllers;
using Moq;
using System.Data.Entity;
using SelectionBoxService.Data;
using SelectionBoxService.Tests.Data;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

namespace SelectionBoxService.Tests.Tests
{
    [TestClass]
    public class PutTests : GenericTest
    {
        [TestMethod]
        public async Task ServiceTestPutChangeAvailable()
        {
            HttpResponseMessage response = await controller.UpdateSelectionBox(1, new LibAyycorn.Dtos.Giftbox
            {
                Id = 1,
                Total = 10.0,
                WrappingId = 1,
                WrappingRangeId = 1,
                WrappingRangeName = "Swirl-Tastic",
                WrappingTypeId = 1,
                WrappingTypeName = "Gift Bag",
                Removed = false,
                Visible = true,
                Available = false
            });

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task ServiceTestPutChangeVisible()
        {
            HttpResponseMessage response = await controller.UpdateSelectionBox(2, new LibAyycorn.Dtos.Giftbox
            {
                Id = 2,
                Total = 10.0,
                WrappingId = 1,
                WrappingRangeId = 1,
                WrappingRangeName = "Swirl-Tastic",
                WrappingTypeId = 1,
                WrappingTypeName = "Gift Bag",
                Removed = false,
                Visible = false,
                Available = true
            });

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task ServiceTestPutChangeAvailableAndVisible()
        {
            HttpResponseMessage response = await controller.UpdateSelectionBox(2, new LibAyycorn.Dtos.Giftbox
            {
                Id = 3,
                Total = 10.0,
                WrappingId = 1,
                WrappingRangeId = 1,
                WrappingRangeName = "Swirl-Tastic",
                WrappingTypeId = 1,
                WrappingTypeName = "Gift Bag",
                Removed = false,
                Visible = false,
                Available = false
            });

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task ServiceTestPutNoChanges()
        {
            HttpResponseMessage response = await controller.UpdateSelectionBox(2, new LibAyycorn.Dtos.Giftbox
            {
                Id = 4,
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
        }
    }
}
