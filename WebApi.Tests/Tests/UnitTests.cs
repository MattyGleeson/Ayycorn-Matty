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
    public class UnitTests
    {
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
        private List<Giftbox> _boxes;
        private string _baseUrl = "http://ayycornselectionboxservice.azurewebsites.net/";
        private string _json;

        [TestInitialize]
        public void Init()
        {
            _json = System.IO.File.ReadAllText("../../Data/selectionboxes.json");
            _boxes = JsonConvert.DeserializeObject<List<Giftbox>>(_json, _serializerSettings);
        }

        [TestMethod]
        public async Task FacadeTestGet()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(_baseUrl + "getboxes").Respond("application/json", _json);
            SelectionBoxServiceFacade facade = new SelectionBoxServiceFacade(new HttpClient(mockHttp));

            var response = await facade.GetSelectionBoxes();

            CollectionAssert.AreEqual(response.ToList(), _boxes, new GiftboxComparer());
        }

        [TestMethod]
        public async Task FacadeTestGetFailureHandling()
        {
            var mockFailHttp = new MockHttpMessageHandler();
            mockFailHttp.When(_baseUrl + "getboxes").Respond(HttpStatusCode.InternalServerError);
            SelectionBoxServiceFacade failFacade = new SelectionBoxServiceFacade(new HttpClient(mockFailHttp));

            var response = await failFacade.GetSelectionBoxes();

            Assert.AreEqual(response.Count(), 0);
        }

        [TestMethod]
        public async Task FacadeTestPost()
        {
            var mockHttp = new MockHttpMessageHandler();
            string json = JsonConvert.SerializeObject(_boxes.ElementAt(1));

            mockHttp.When(_baseUrl + "postbox").Respond("application/json", json);
            SelectionBoxServiceFacade facade = new SelectionBoxServiceFacade(new HttpClient(mockHttp));

            var response = await facade.PostSelectionBox(_boxes.ElementAt(1));

            Assert.AreEqual(response.Id, _boxes.ElementAt(1).Id);
        }

        [TestMethod]
        public async Task FacadeTestPostFailureHandling()
        {
            var mockFailHttp = new MockHttpMessageHandler();
            mockFailHttp.When(_baseUrl + "postbox").Respond(HttpStatusCode.InternalServerError);
            SelectionBoxServiceFacade failFacade = new SelectionBoxServiceFacade(new HttpClient(mockFailHttp));

            var response = await failFacade.PostSelectionBox(_boxes.ElementAt(1));

            Assert.IsNull(response);
        }

        [TestMethod]
        public async Task FacadeTestPut()
        {
            _boxes.ElementAt(1).Available = false;
            string json = JsonConvert.SerializeObject(_boxes.ElementAt(1));

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(_baseUrl + "updatebox/" + _boxes.ElementAt(1).Id).Respond("application/json", json);
            SelectionBoxServiceFacade facade = new SelectionBoxServiceFacade(new HttpClient(mockHttp));

            var response = await facade.UpdateSelectionBox(_boxes.ElementAt(1));

            Assert.IsFalse(response.Available);
        }

        [TestMethod]
        public async Task FacadeTestPutFailureHandling()
        {
            _boxes.ElementAt(2).Available = false;
            string json = JsonConvert.SerializeObject(_boxes.ElementAt(2));

            var mockFailHttp = new MockHttpMessageHandler();
            mockFailHttp.When(_baseUrl + "updatebox/" + _boxes.ElementAt(2).Id).Respond(HttpStatusCode.InternalServerError);
            SelectionBoxServiceFacade failFacade = new SelectionBoxServiceFacade(new HttpClient(mockFailHttp));

            var response = await failFacade.UpdateSelectionBox(_boxes.ElementAt(2));

            Assert.IsNull(response);
        }

        [TestMethod]
        public async Task FacadeTestDelete()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(_baseUrl + "deletebox/*").Respond(HttpStatusCode.OK);
            SelectionBoxServiceFacade facade = new SelectionBoxServiceFacade(new HttpClient(mockHttp));

            var response = await facade.RemoveSelectionBox(_boxes.ElementAt(1).Id);

            Assert.IsTrue(response);
        }

        [TestMethod]
        public async Task FacadeTestDeleteFailurehandling()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(_baseUrl + "deletebox/" + _boxes.ElementAt(1).Id).Respond(HttpStatusCode.InternalServerError);
            SelectionBoxServiceFacade facade = new SelectionBoxServiceFacade(new HttpClient(mockHttp));

            var response = await facade.RemoveSelectionBox(_boxes.ElementAt(1).Id);

            Assert.IsFalse(response);
        }
    }

    /// <summary>
    /// Compares LibAyycorn Giftboxes from results with each other by multiple fields.
    /// </summary>
    /// <seealso cref="Giftbox" />
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
