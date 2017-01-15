using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using LibAyycorn.Dtos;
using RichardSzalay.MockHttp;
using WebApi.Facades;
using System.Net.Http;
using System.Linq;

namespace WebApi.Tests.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
        private List<Giftbox> Boxes;
        private string BaseUrl = "http://ayycornselectionboxservice.azurewebsites.net/";
        private SelectionBoxServiceFacade Facade;

        [TestInitialize]
        public void Init()
        {
            var json = System.IO.File.ReadAllText("../../Data/selectionboxes.json");
            Boxes = JsonConvert.DeserializeObject<List<Giftbox>>(json, SerializerSettings);

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(BaseUrl + "getboxes").Respond("application/json", json);
            Facade = new SelectionBoxServiceFacade(new HttpClient(mockHttp));
        }

        [TestMethod]
        public void TestGet()
        {
            var response = Facade.GetSelectionBoxes().Result.ToList();

            Assert.AreEqual(response, Boxes);
        }

        [TestMethod]
        public void TestPost()
        {

        }

        [TestMethod]
        public void TestPut()
        {

        }

        [TestMethod]
        public void TestDelete()
        {

        }
    }
}
