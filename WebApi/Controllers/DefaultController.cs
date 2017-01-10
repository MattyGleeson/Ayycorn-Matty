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
        public async Task<IEnumerable<LibAyycorn.Dtos.SelectionBox>> Get()
        {
            Facades.SelectionBoxServiceFacade sbsf = new Facades.SelectionBoxServiceFacade();

            return await sbsf.GetSelectionBoxes();
        }
    }
}
