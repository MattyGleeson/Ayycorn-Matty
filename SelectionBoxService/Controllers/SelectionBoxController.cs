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
    /// <summary>
    /// Selectionbox service controller containing the endpoints used to GET, POST, PUT and DELETE.
    /// </summary>
    public class SelectionBoxController : ApiController
    {
        private AyycornDb _db;

        /// <summary>
        /// Default constructor that sets the database to be an instance of AyycornDb
        /// </summary>
        public SelectionBoxController()
        {
            _db = new AyycornDb();
        }

        /// <summary>
        /// Constructor used when unit testing to pass a mock of the AyycornDb
        /// </summary>
        /// <param name="db"></param>
        public SelectionBoxController(AyycornDb db)
        {
            this._db = db;
        }

        /// <summary>
        /// Gets all selection boxes from the database.
        /// </summary>
        /// <returns></returns>
        [Route("getboxes")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllSelectionBoxes()
        {
            IEnumerable<SelectionBox> res = await _db.SelectionBoxes.Where(b => b.Removed != true).ToListAsync();

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
            SelectionBox res = await _db.SelectionBoxes.Where(b => b.Removed != true && b.Id == id).FirstOrDefaultAsync();

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
                SelectionBox newBox = _db.SelectionBoxes.Add(new SelectionBox
                {
                    Total = gb.Total,
                    WrappingId = gb.WrappingId,
                    WrappingRangeId = gb.WrappingRangeId,
                    WrappingRangeName = gb.WrappingRangeName,
                    WrappingTypeId = gb.WrappingTypeId,
                    WrappingTypeName = gb.WrappingTypeName,
                    Removed = false,
                    Visible = true,
                    Available = true,
                    Id = gb.Id != 0 ? gb.Id : 0
                });
                await _db.SaveChangesAsync();

                if (gb.Products != null)
                {
                    foreach (LibAyycorn.Dtos.Product product in gb.Products)
                    {
                        Data.Product dbProd = await GetProduct(product.Name, product.StoreName);

                        if (dbProd == null)
                        {
                            dbProd = _db.Products.Add(new Data.Product
                            {
                                Name = product.Name,
                                Store = product.StoreName,
                                ProductId = product.Id
                            });
                            await _db.SaveChangesAsync();
                        }


                        if (await CheckIfSelectionBoxProductIsValid(dbProd.Id, newBox.Id))
                        {
                            _db.SelectionBoxProducts.Add(new SelectionBoxProduct
                            {
                                ProductId = dbProd.Id,
                                Product = dbProd,
                                SelectionBoxId = newBox.Id,
                                SelectionBox = newBox
                            });
                            await _db.SaveChangesAsync();
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
                SelectionBox selectionBox = await _db.SelectionBoxes.Where(sb => sb.Id == id).FirstOrDefaultAsync();
                selectionBox.Removed = true;

                _db.SetModified(selectionBox);
                await _db.SaveChangesAsync();

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
                Data.SelectionBox selectionBox = await _db.SelectionBoxes.Where(sb => sb.Id == Id).FirstOrDefaultAsync();

                selectionBox.Available = postObject.Available;
                selectionBox.Removed = postObject.Removed;
                selectionBox.Total = postObject.Total;
                selectionBox.Visible = postObject.Visible;
                selectionBox.WrappingId = postObject.WrappingId;
                selectionBox.WrappingRangeId = postObject.WrappingRangeId;
                selectionBox.WrappingRangeName = postObject.WrappingRangeName;
                selectionBox.WrappingTypeId = postObject.WrappingTypeId;
                selectionBox.WrappingTypeName = postObject.WrappingTypeName;

                _db.SetModified(selectionBox);
                await _db.SaveChangesAsync();

                Giftbox res = CreateBoxFromDbBox(selectionBox);

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed");
            }
        }

        /// <summary>
        /// Returns a database product model using the name and the store.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="store"></param>
        /// <returns></returns>
        private async Task<Data.Product> GetProduct(string name, string store)
        {
            return await _db.Products.Where(p => p.Name == name && p.Store == store).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Checks to see if a row with the same properties already exists in the database.
        /// </summary>
        /// <param name="prod"></param>
        /// <param name="selectionBox"></param>
        /// <returns></returns>
        private async Task<bool> CheckIfSelectionBoxProductIsValid(int prod, int selectionBox)
        {
            return !(await _db.SelectionBoxProducts.AnyAsync(sb => (sb.ProductId == prod) && (sb.SelectionBoxId == selectionBox)));
        }

        /// <summary>
        /// Returns a list of products from the selection box parameter.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a giftbox model using the database selection box parameter.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
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
