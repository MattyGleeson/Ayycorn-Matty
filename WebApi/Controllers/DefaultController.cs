using LibAyycorn.Dtos;
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
        private readonly Facades.SelectionBoxServiceFacade _facade;

        public DefaultController()
        {
            _facade = new Facades.SelectionBoxServiceFacade();
        }

        [HttpGet]
        [Route("api/get")]
        public async Task<IEnumerable<LibAyycorn.Dtos.Giftbox>> Get()
        {
            return await _facade.GetSelectionBoxes();
        }

        [HttpGet]
        [Route("api/getfilters")]
        public async Task<IEnumerable<LibAyycorn.Dtos.Giftbox>> GetFilters()
        {
            return await _facade.GetByFilters(maxPrice: 30);
        }

        [HttpPost]
        [Route("api/post")]
        public async Task<LibAyycorn.Dtos.Giftbox> Post()
        {
            Giftbox gb = new Giftbox
            {
                Id = 1,
                Total = 50,
                WrappingRangeId = 1,
                WrappingRangeName = "From Api Range",
                WrappingTypeId = 1,
                WrappingTypeName = "From Api Type",
                WrappingId = 1,
                Products = new List<LibAyycorn.Dtos.Product>()
                {
                    new Product { Id = 1, BrandId = 1, BrandName = "From Api Brand", CategoryId = 1, CategoryName = "From Api Cat", Name = "From Api Name", StoreName = "From Api Store" }
                }
            };

            return await _facade.PostSelectionBox(gb);
        }

        [HttpPost]
        [Route("api/postEnum")]
        public async Task<LibAyycorn.Dtos.Giftbox> PostEnumerableProds()
        {
            IEnumerable<Product> prods = new List<LibAyycorn.Dtos.Product>()
            {
                new Product { Id = 2, BrandId = 1, BrandName = "From Api Brand 1", CategoryId = 1, CategoryName = "From Api Cat 1", Name = "From Api Name 1", StoreName = "From Api Store 1" }
            };

            Giftbox gb = new Giftbox
            {
                Id = 1,
                Total = 50,
                WrappingRangeId = 1,
                WrappingRangeName = "From Api Range 1",
                WrappingTypeId = 1,
                WrappingTypeName = "From Api Type 1",
                WrappingId = 1,
                Products = prods
            };

            return await _facade.PostSelectionBox(gb);
        }

        [HttpPut]
        [Route("api/put")]
        public async Task<LibAyycorn.Dtos.Giftbox> Put()
        {
            Giftbox gb = new Giftbox
            {
                Id = 1,
                Total = 50,
                WrappingRangeId = 1,
                WrappingRangeName = "From Api Range",
                WrappingTypeId = 1,
                WrappingTypeName = "From Api Type",
                WrappingId = 1,
                Available = false,
                Products = new List<LibAyycorn.Dtos.Product>()
                {
                    new Product { BrandId = 1, BrandName = "From Api Brand", CategoryId = 1, CategoryName = "From Api Cat", Name = "From Api Name", StoreName = "From Api Store" }
                }
            };

            return await _facade.UpdateSelectionBox(gb);
        }

        [HttpDelete]
        [Route("api/delete")]
        public async Task<bool> Delete()
        {
            return await _facade.RemoveSelectionBox(1);
        }
    }
}
