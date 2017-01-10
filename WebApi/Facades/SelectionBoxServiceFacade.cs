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
        public async Task<IEnumerable<LibAyycorn.Dtos.SelectionBox>> GetSelectionBoxes()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync("http://localhost:16635/service/GetBoxes");

                    response.EnsureSuccessStatusCode();

                    string content = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<IEnumerable<LibAyycorn.Dtos.SelectionBox>>(content);
                }
                catch(Exception ex)
                {
                    return Enumerable.Empty<LibAyycorn.Dtos.SelectionBox>();
                }
            }
        }

        public async Task<bool> PostSelectionBox(LibAyycorn.Dtos.SelectionBox selectionBox)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    string json = JsonConvert.SerializeObject(selectionBox);
                    HttpResponseMessage response = await client.PostAsJsonAsync("http://localhost:16635/service/PostBox", json);

                    response.EnsureSuccessStatusCode();

                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<bool> UpdateSelectionBox(LibAyycorn.Dtos.SelectionBox selectionBox)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    string json = JsonConvert.SerializeObject(selectionBox);
                    HttpResponseMessage response = await client.PutAsJsonAsync("http://localhost:16635/service/UpdateBox/" + selectionBox.Id, json);

                    response.EnsureSuccessStatusCode();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<bool> RemoveSelectionBox(LibAyycorn.Dtos.SelectionBox selectionBox)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri("http://localhost:16635/service/UpdateBox/" + selectionBox.Id)
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
}
