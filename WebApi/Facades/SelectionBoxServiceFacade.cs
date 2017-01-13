using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Facades
{
    public class SelectionBoxServiceFacade : ApiController
    {
        private readonly HttpClient client;

        public SelectionBoxServiceFacade()
        {
            client = new HttpClient();
        }

        public SelectionBoxServiceFacade(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<LibAyycorn.Dtos.SelectionBox>> GetSelectionBoxes()
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage response = await client.GetAsync("http://ayycornselectionboxservice.azurewebsites.net/getboxes");

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<LibAyycorn.Dtos.SelectionBox>>(content);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<LibAyycorn.Dtos.SelectionBox>();
            }
        }

        public async Task<bool> PostSelectionBox(LibAyycorn.Dtos.SelectionBox selectionBox)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                string json = JsonConvert.SerializeObject(selectionBox);
                HttpResponseMessage response = await client.PostAsJsonAsync("http://ayycornselectionboxservice.azurewebsites.net/postbox", json);

                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateSelectionBox(LibAyycorn.Dtos.SelectionBox selectionBox)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                string json = JsonConvert.SerializeObject(selectionBox);
                HttpResponseMessage response = await client.PutAsJsonAsync("http://ayycornselectionboxservice.azurewebsites.net/updatebox/" + selectionBox.Id, json);

                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoveSelectionBox(LibAyycorn.Dtos.SelectionBox selectionBox)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri("http://ayycornselectionboxservice.azurewebsites.net/deletebox/" + selectionBox.Id)
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
    }
}
