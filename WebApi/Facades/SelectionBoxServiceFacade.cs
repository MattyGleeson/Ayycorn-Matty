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
    public class SelectionBoxServiceFacade : ApiController
    {
        private readonly HttpClient client;
        private readonly string BaseUrl = "http://ayycornselectionboxservice.azurewebsites.net/";
        protected JsonSerializerSettings SerializerSettings;

        public SelectionBoxServiceFacade()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
        }

        public SelectionBoxServiceFacade(HttpClient client)
        {
            this.client = client;
            SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
        }

        public async Task<IQueryable<LibAyycorn.Dtos.Giftbox>> GetSelectionBoxes()
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(BaseUrl + "getboxes")
                };

                IQueryable< LibAyycorn.Dtos.Giftbox > res = await ExecuteRequestAsync<IQueryable<LibAyycorn.Dtos.Giftbox>>(request);

                return res.Any() 
                    ? res
                    : Enumerable.Empty<LibAyycorn.Dtos.Giftbox>().AsQueryable();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<LibAyycorn.Dtos.Giftbox>().AsQueryable();
            }
        }

        public async Task<LibAyycorn.Dtos.Giftbox> PostSelectionBox(LibAyycorn.Dtos.Giftbox selectionBox)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(BaseUrl + "postbox"),
                    Content = new StringContent(JsonConvert.SerializeObject(selectionBox), Encoding.UTF8, "application/json")
                };

                return await ExecuteRequestAsync<LibAyycorn.Dtos.Giftbox>(request);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<LibAyycorn.Dtos.Giftbox> UpdateSelectionBox(LibAyycorn.Dtos.Giftbox selectionBox)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(BaseUrl + "updatebox/" + selectionBox.Id),
                    Content = new StringContent(JsonConvert.SerializeObject(selectionBox), Encoding.UTF8, "application/json")
                };

                return await ExecuteRequestAsync<LibAyycorn.Dtos.Giftbox>(request);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> RemoveSelectionBox(LibAyycorn.Dtos.Giftbox selectionBox)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(BaseUrl + "deletebox/" + selectionBox.Id)
                };

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<T> ExecuteRequestAsync<T>(HttpRequestMessage request) where T : class
        {
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content, SerializerSettings);
        }
    }
}
