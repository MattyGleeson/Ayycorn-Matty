using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using LibAyycorn.Dtos;
using RichardSzalay.MockHttp;
using WebApi.Facades;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Tests.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
        private List<Giftbox> Boxes;
        private string BaseUrl = "http://ayycornselectionboxservice.azurewebsites.net/";
        private string json;

        [TestInitialize]
        public void Init()
        {
            json = System.IO.File.ReadAllText("../../Data/selectionboxes.json");
            Boxes = JsonConvert.DeserializeObject<List<Giftbox>>(json, SerializerSettings);
        }

        [TestMethod]
        public async Task TestGet()
        {
            var MockHttp = new MockHttpMessageHandler();
            MockHttp.When(BaseUrl + "getboxes").Respond("application/json", json);
            SelectionBoxServiceFacade Facade = new SelectionBoxServiceFacade(new HttpClient(MockHttp));

            var response = await Facade.GetSelectionBoxes();

            CollectionAssert.AreEqual(response.ToList(), Boxes, new GiftboxComparer());
        }

        [TestMethod]
        public async Task TestGetFailureHandling()
        {
            var mockFailHttp = new MockHttpMessageHandler();
            mockFailHttp.When(BaseUrl + "getboxes").Respond(HttpStatusCode.InternalServerError);
            SelectionBoxServiceFacade FailFacade = new SelectionBoxServiceFacade(new HttpClient(mockFailHttp));

            var response = await FailFacade.GetSelectionBoxes();

            Assert.AreEqual(response.Count(), 0);
        }

        [TestMethod]
        public async Task TestPost()
        {
            var MockHttp = new MockHttpMessageHandler();
            string json = JsonConvert.SerializeObject(Boxes.ElementAt(1));

            MockHttp.When(BaseUrl + "postbox").Respond("application/json", json);
            SelectionBoxServiceFacade Facade = new SelectionBoxServiceFacade(new HttpClient(MockHttp));

            var response = await Facade.PostSelectionBox(Boxes.ElementAt(1));

            Assert.AreEqual(response.Id, Boxes.ElementAt(1).Id);
        }

        [TestMethod]
        public async Task TestPostFailureHandling()
        {
            var mockFailHttp = new MockHttpMessageHandler();
            mockFailHttp.When(BaseUrl + "postbox").Respond(HttpStatusCode.InternalServerError);
            SelectionBoxServiceFacade FailFacade = new SelectionBoxServiceFacade(new HttpClient(mockFailHttp));

            var response = await FailFacade.PostSelectionBox(Boxes.ElementAt(1));

            Assert.IsNull(response);
        }

        [TestMethod]
        public async Task TestPut()
        {
            Boxes.ElementAt(1).Available = false;
            string json = JsonConvert.SerializeObject(Boxes.ElementAt(1));

            var MockHttp = new MockHttpMessageHandler();
            MockHttp.When(BaseUrl + "updatebox/" + Boxes.ElementAt(1).Id).Respond("application/json", json);
            SelectionBoxServiceFacade Facade = new SelectionBoxServiceFacade(new HttpClient(MockHttp));

            var response = await Facade.UpdateSelectionBox(Boxes.ElementAt(1));

            Assert.IsFalse(response.Available);
        }

        [TestMethod]
        public async Task TestPutFailureHandling()
        {
            Boxes.ElementAt(2).Available = false;
            string json = JsonConvert.SerializeObject(Boxes.ElementAt(2));

            var MockFailHttp = new MockHttpMessageHandler();
            MockFailHttp.When(BaseUrl + "updatebox/" + Boxes.ElementAt(2).Id).Respond(HttpStatusCode.InternalServerError);
            SelectionBoxServiceFacade FailFacade = new SelectionBoxServiceFacade(new HttpClient(MockFailHttp));

            var response = await FailFacade.UpdateSelectionBox(Boxes.ElementAt(2));

            Assert.IsNull(response);
        }

        [TestMethod]
        public async Task TestDelete()
        {
            var MockHttp = new MockHttpMessageHandler();
            MockHttp.When(BaseUrl + "deletebox/*").Respond(HttpStatusCode.OK);
            SelectionBoxServiceFacade Facade = new SelectionBoxServiceFacade(new HttpClient(MockHttp));

            var response = await Facade.RemoveSelectionBox(Boxes.ElementAt(1).Id);

            Assert.IsTrue(response);
        }

        [TestMethod]
        public async Task TestDeleteFailurehandling()
        {
            var MockHttp = new MockHttpMessageHandler();
            MockHttp.When(BaseUrl + "deletebox/" + Boxes.ElementAt(1).Id).Respond(HttpStatusCode.InternalServerError);
            SelectionBoxServiceFacade Facade = new SelectionBoxServiceFacade(new HttpClient(MockHttp));

            var response = await Facade.RemoveSelectionBox(Boxes.ElementAt(1).Id);

            Assert.IsFalse(response);
        }
    }

    /// <summary>
    /// Compares LibAyycorn Giftboxes from results with each other by multiple fields.
    /// </summary>
    /// <seealso cref="Product" />
    internal class GiftboxComparer : Comparer<Giftbox>
    {
        public override int Compare(Giftbox x, Giftbox y)
        {
            return x.Id.CompareTo(y.Id) +
                   string.Compare(x.WrappingTypeName, y.WrappingTypeName, StringComparison.Ordinal) +
                   string.Compare(x.WrappingRangeName, y.WrappingRangeName, StringComparison.Ordinal);
        }
    }
}
