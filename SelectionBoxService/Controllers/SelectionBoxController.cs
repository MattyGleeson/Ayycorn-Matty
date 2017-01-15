using LibAyycorn.Dtos;
using Newtonsoft.Json;
using SelectionBoxService.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SelectionBoxService.Controllers
{
    public class SelectionBoxController : ApiController
    {
        private AyycornDb db;

        public SelectionBoxController()
        {
            db = new AyycornDb();
        }

        public SelectionBoxController(AyycornDb db)
        {
            this.db = db;
        }

        /// <summary>
        /// Gets all selection boxes from the database.
        /// </summary>
        /// <returns></returns>
        [Route("getboxes")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllSelectionBoxes()
        {
            IEnumerable<SelectionBox> res = await db.SelectionBoxes.Where(b => b.Removed != true).ToListAsync();

            IEnumerable<Giftbox> boxes = res.Select(b => CreateBoxFromDbBox(b));

            return boxes.Any() ?
                Request.CreateResponse(HttpStatusCode.OK, boxes) :
                Request.CreateErrorResponse(HttpStatusCode.NoContent, "No Selection Boxes");
        }

        /// <summary>
        /// Gets a selection box by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("getbox/{id:int?}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetSelectionBox(int id)
        {
            SelectionBox res = await db.SelectionBoxes.Where(b => b.Removed != true && b.Id == id).FirstOrDefaultAsync();

            if (res != null)
                return Request.CreateResponse(HttpStatusCode.OK, CreateBoxFromDbBox(res));
            else
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "No Selection Box with Id");
        }

        /// <summary>
        /// Posts a selection box Dto to the database.
        /// </summary>
        /// <param name="gb"></param>
        /// <returns></returns>
        [Route("postbox")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostSelectionBox(Giftbox gb)
        {
            try
            {
                SelectionBox newBox = db.SelectionBoxes.Add(new SelectionBox
                {
                    Total = gb.Total,
                    WrappingId = gb.WrappingId,
                    WrappingRangeId = gb.WrappingRangeId,
                    WrappingRangeName = gb.WrappingRangeName,
                    WrappingTypeId = gb.WrappingTypeId,
                    WrappingTypeName = gb.WrappingTypeName,
                    Removed = gb.Removed,
                    Visible = gb.Visible,
                    Available = gb.Available,
                    Id = gb.Id != 0 ? gb.Id : 0
                });
                await db.SaveChangesAsync();

                if (gb.Products != null)
                {
                    foreach (LibAyycorn.Dtos.Product product in gb.Products)
                    {
                        Data.Product dbProd = await GetProduct(product.Name, product.StoreName);

                        if (dbProd == null)
                        {
                            dbProd = db.Products.Add(new Data.Product
                            {
                                Name = product.Name,
                                Store = product.StoreName,
                                ProductId = product.Id
                            });
                            await db.SaveChangesAsync();
                        }


                        if (await CheckIfSelectionBoxProductIsValid(dbProd.Id, newBox.Id))
                        {
                            db.SelectionBoxProducts.Add(new SelectionBoxProduct
                            {
                                ProductId = dbProd.Id,
                                Product = dbProd,
                                SelectionBoxId = newBox.Id,
                                SelectionBox = newBox
                            });
                            await db.SaveChangesAsync();
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, CreateBoxFromDbBox(newBox));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed");
            }
        }

        /// <summary>
        /// Removes a selection box.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("deletebox/{id:int?}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteSelectionBox(int id)
        {
            try
            {
                SelectionBox selectionBox = await db.SelectionBoxes.Where(sb => sb.Id == id).FirstOrDefaultAsync();
                selectionBox.Removed = true;

                db.SetModified(selectionBox);
                await db.SaveChangesAsync();

                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed");
            }
        }

        /// <summary>
        /// Puts a selection box to the database. Allows updating of the available and visible properties.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="postObject"></param>
        /// <returns></returns>
        [Route("updatebox/{id:int?}")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateSelectionBox(int Id, Giftbox postObject)
        {
            try
            {
                Data.SelectionBox selectionBox = await db.SelectionBoxes.Where(sb => sb.Id == Id).FirstOrDefaultAsync();

                selectionBox.Available = postObject.Available;
                selectionBox.Removed = postObject.Removed;
                selectionBox.Total = postObject.Total;
                selectionBox.Visible = postObject.Visible;
                selectionBox.WrappingId = postObject.WrappingId;
                selectionBox.WrappingRangeId = postObject.WrappingRangeId;
                selectionBox.WrappingRangeName = postObject.WrappingRangeName;
                selectionBox.WrappingTypeId = postObject.WrappingTypeId;
                selectionBox.WrappingTypeName = postObject.WrappingTypeName;

                db.SetModified(selectionBox);
                await db.SaveChangesAsync();

                Giftbox res = CreateBoxFromDbBox(selectionBox);

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed");
            }
        }

        private async Task<Data.Product> GetProduct(string name, string store)
        {
            return await db.Products.Where(p => p.Name == name && p.Store == store).FirstOrDefaultAsync();
        }

        private async Task<bool> CheckIfSelectionBoxProductIsValid(int prod, int selectionBox)
        {
            return !(await db.SelectionBoxProducts.AnyAsync(sb => (sb.ProductId == prod) && (sb.SelectionBoxId == selectionBox)));
        }

        private IEnumerable<LibAyycorn.Dtos.Product> GetProductsForBox(SelectionBox box)
        {
            IEnumerable<Data.Product> boxProducts = box.SelectionBoxProducts.Select(b => b.Product);
            if (boxProducts.Any())
                return boxProducts.Select(b => new LibAyycorn.Dtos.Product
                {
                    Id = b.ProductId,
                    Name = b.Name,
                    StoreName = b.Store
                });
            return Enumerable.Empty<LibAyycorn.Dtos.Product>();
        }

        private Giftbox CreateBoxFromDbBox(SelectionBox box)
        {
            return new Giftbox
            {
                Id = box.Id,
                Total = box.Total,
                WrappingId = box.WrappingId,
                WrappingRangeId = box.WrappingRangeId ?? default(int),
                WrappingRangeName = box.WrappingRangeName,
                WrappingTypeId = box.WrappingTypeId ?? default(int),
                WrappingTypeName = box.WrappingTypeName,
                Removed = box.Removed == false ? false : true,
                Visible = box.Visible == false ? false : true,
                Available = box.Available == false ? false : true,
                Products = GetProductsForBox(box)
            };
        }
    }
}
