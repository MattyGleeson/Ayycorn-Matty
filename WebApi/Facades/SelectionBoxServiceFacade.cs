using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Facades
{
    /// <summary>
    /// Facade that handls the interactions between the web api and the selection box service.
    /// </summary>
    public class SelectionBoxServiceFacade : ApiController
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://ayycornselectionboxservice.azurewebsites.net/";
        private JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// Default constructor that sets up the HttpClient and JsonSerializerSettings.
        /// </summary>
        public SelectionBoxServiceFacade()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
        }

        /// <summary>
        /// Constructor used for testing that accepts a mock HttpCient.
        /// </summary>
        /// <param name="client"></param>
        public SelectionBoxServiceFacade(HttpClient client)
        {
            this._client = client;
            _serializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
        }

        /// <summary>
        /// Returns an IQueryable of giftboxes from the selection box service.
        /// </summary>
        /// <returns></returns>
        public async Task<IQueryable<LibAyycorn.Dtos.Giftbox>> GetSelectionBoxes()
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(_baseUrl + "getboxes")
                };

                IQueryable< LibAyycorn.Dtos.Giftbox > res = await ExecuteRequestAsyncList<LibAyycorn.Dtos.Giftbox>(request);

                return res.Any() 
                    ? res
                    : Enumerable.Empty<LibAyycorn.Dtos.Giftbox>().AsQueryable();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<LibAyycorn.Dtos.Giftbox>().AsQueryable();
            }
        }

        /// <summary>
        /// Returns a giftbox with the id parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LibAyycorn.Dtos.Giftbox> GetSelectionBoxById(int id)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(_baseUrl + "getbox/" + id)
                };

                return await ExecuteRequestAsync<LibAyycorn.Dtos.Giftbox>(request);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<IQueryable<LibAyycorn.Dtos.Giftbox>> GetByFilters(double minPrice = 0, double maxPrice = 0, string wrappingTypeName = null,
            string wrappingRangeName = null, bool? available = null, bool? visible = null)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(_baseUrl + "getboxes")
                };

                IQueryable<LibAyycorn.Dtos.Giftbox> res = await ExecuteRequestAsyncList<LibAyycorn.Dtos.Giftbox>(request);

                res = res.Where(g => g.Total >= minPrice);
                res = res.Where(g => g.Total <= (maxPrice == 0 ? Double.MaxValue : maxPrice));
                if (wrappingTypeName != null) res = res.Where(g => g.WrappingTypeName == wrappingTypeName);
                if (wrappingRangeName != null) res = res.Where(g => g.WrappingRangeName == wrappingRangeName);
                if (available != null) res = res.Where(g => g.Available == available);
                if (visible != null) res = res.Where(g => g.Visible == visible);

                return res.Any()
                    ? res
                    : Enumerable.Empty<LibAyycorn.Dtos.Giftbox>().AsQueryable();
            }
            catch(Exception ex)
            {
                return Enumerable.Empty<LibAyycorn.Dtos.Giftbox>().AsQueryable();
            }
        }

        /// <summary>
        /// Posts a selection box the service and returns the updated model.
        /// </summary>
        /// <param name="selectionBox"></param>
        /// <returns></returns>
        public async Task<LibAyycorn.Dtos.Giftbox> PostSelectionBox(LibAyycorn.Dtos.Giftbox selectionBox)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(_baseUrl + "postbox"),
                    Content = new StringContent(JsonConvert.SerializeObject(selectionBox), Encoding.UTF8, "application/json")
                };

                return await ExecuteRequestAsync<LibAyycorn.Dtos.Giftbox>(request);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Updates a selection box and returns the updated model.
        /// </summary>
        /// <param name="selectionBox"></param>
        /// <returns></returns>
        public async Task<LibAyycorn.Dtos.Giftbox> UpdateSelectionBox(LibAyycorn.Dtos.Giftbox selectionBox)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(_baseUrl + "updatebox/" + selectionBox.Id),
                    Content = new StringContent(JsonConvert.SerializeObject(selectionBox), Encoding.UTF8, "application/json")
                };

                return await ExecuteRequestAsync<LibAyycorn.Dtos.Giftbox>(request);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Removes the selection box with the id parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemoveSelectionBox(int id)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(_baseUrl + "deletebox/" + id)
                };

                HttpResponseMessage response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Async task to execute a HttpRequestMessage and return a single model. Uses T type parameter so the facade can be expanded if needed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<T> ExecuteRequestAsync<T>(HttpRequestMessage request) where T : class
        {
            HttpResponseMessage response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content, _serializerSettings);
        }

        /// <summary>
        /// Async task to execute a HttpRequestMessage and return a list of models. Uses T type parameter so the facade can be expanded if needed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<IQueryable<T>> ExecuteRequestAsyncList<T>(HttpRequestMessage request) where T : class
        {
            HttpResponseMessage response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<T>>(content, _serializerSettings).AsQueryable();
        }
    }
}
