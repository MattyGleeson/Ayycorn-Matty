using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Net.Http;
using SelectionBoxService.Controllers;
using SelectionBoxService.Data;
using System.Data.Entity;
using Moq;
using System.Web.Http;
using SelectionBoxService.Tests.Data;

namespace SelectionBoxService.Tests.Tests
{
    [TestClass]
    public class DeleteTests : GenericTest
    {
        [TestMethod]
        public async Task ServiceTestRemove()
        {
            HttpResponseMessage response = await Controller.DeleteSelectionBox(2);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
