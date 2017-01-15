using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpGet]
        [Route("api/test")]
        public async Task<IEnumerable<LibAyycorn.Dtos.Giftbox>> Get()
        {
            Facades.SelectionBoxServiceFacade sbsf = new Facades.SelectionBoxServiceFacade();

            return await sbsf.GetSelectionBoxes();
        }

        [HttpPost]
        [Route("api/test1")]
        public async Task<IEnumerable<LibAyycorn.Dtos.Giftbox>> Post()
        {
            Facades.SelectionBoxServiceFacade sbsf = new Facades.SelectionBoxServiceFacade();

            return await sbsf.GetSelectionBoxes();
        }

        [HttpPut]
        [Route("api/test2")]
        public async Task<IEnumerable<LibAyycorn.Dtos.Giftbox>> Put()
        {
            Facades.SelectionBoxServiceFacade sbsf = new Facades.SelectionBoxServiceFacade();

            return await sbsf.GetSelectionBoxes();
        }

        [HttpDelete]
        [Route("api/test3")]
        public async Task<IEnumerable<LibAyycorn.Dtos.Giftbox>> Delete()
        {
            Facades.SelectionBoxServiceFacade sbsf = new Facades.SelectionBoxServiceFacade();

            return await sbsf.GetSelectionBoxes();
        }
    }
}
