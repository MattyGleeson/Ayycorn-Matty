using LibAyycorn.Dtos;
using Newtonsoft.Json;
using SelectionBoxService.Data;
using SelectionBoxService.Interfaces;
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
        /// Gets all selection boxes from the database
        /// </summary>
        /// <returns></returns>
        [Route("getboxes")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllSelectionBoxes()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            IEnumerable<Data.SelectionBox> res = await db.SelectionBoxes.ToListAsync();

            IEnumerable<LibAyycorn.Dtos.SelectionBox> boxes = res.Select(b => new LibAyycorn.Dtos.SelectionBox
            {
                Id = b.Id,
                Total = b.Total,
                WrappingId = b.WrappingId,
                WrappingRangeId = b.WrappingRangeId,
                WrappingRangeName = b.WrappingRangeName,
                WrappingTypeId = b.WrappingTypeId,
                WrappingTypeName = b.WrappingTypeName,
                Removed = b.Removed == false ? false : true,
                Visible = b.Visible == false ? false : true,
                Available = b.Available == false ? false : true,
                Products = GetProductsForBox(b)
            });

            return boxes.Any() ?
                Request.CreateResponse(HttpStatusCode.OK, boxes) :
                Request.CreateErrorResponse(HttpStatusCode.NoContent, "No Selection Boxes");
        }

        private IEnumerable<LibAyycorn.Dtos.Product> GetProductsForBox(Data.SelectionBox box)
        {
            IEnumerable<Data.Product> boxProducts = box.SelectionBoxProducts.Select(b => b.Product);
            if (boxProducts.Any())
                return boxProducts.Select(b => new LibAyycorn.Dtos.Product
                {
                    Id = b.ProductId,
                    Name = b.Name,
                    Store = b.Store
                });
            return Enumerable.Empty<LibAyycorn.Dtos.Product>();
        }

        /// <summary>
        /// Posts a selection box Dto to the database
        /// </summary>
        /// <param name="gb"></param>
        /// <returns></returns>
        [Route("postbox")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostSelectionBox(LibAyycorn.Dtos.SelectionBox gb)
        {
            try
            {
                Data.SelectionBox newSelectionBox = db.SelectionBoxes.Add(new Data.SelectionBox
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
                        Data.Product dbProd = await GetProduct(product.Name, product.Store);

                        if (dbProd == null)
                        {
                            dbProd = db.Products.Add(new Data.Product
                            {
                                Name = product.Name,
                                Store = product.Store,
                                ProductId = product.Id
                            });
                            await db.SaveChangesAsync();
                        }

                        if (await CheckIfSelectionBoxProductIsValid(dbProd.Id, newSelectionBox.Id))
                        {
                            db.SelectionBoxProducts.Add(new SelectionBoxProduct
                            {
                                ProductId = dbProd.Id,
                                Product = dbProd,
                                SelectionBoxId = newSelectionBox.Id,
                                SelectionBox = newSelectionBox
                            });
                            await db.SaveChangesAsync();
                        }
                    }
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Failed");
            }
        }

        /// <summary>
        /// Removes a selection box
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("deletebox/{id:int?}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteSelectionBox(int id)
        {
            try
            {
                Data.SelectionBox selectionBox = await db.SelectionBoxes.Where(sb => sb.Id == id).FirstOrDefaultAsync();
                selectionBox.Removed = true;

                db.SetModified(selectionBox);
                await db.SaveChangesAsync();

                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Failed");
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
        public async Task<HttpResponseMessage> UpdateSelectionBox(int Id, LibAyycorn.Dtos.SelectionBox postObject)
        {
            try
            {
                Data.SelectionBox selectionBox = await db.SelectionBoxes.Where(sb => sb.Id == Id).FirstOrDefaultAsync();

                selectionBox.Available = selectionBox.Available != postObject.Available 
                    ? postObject.Available 
                    : selectionBox.Available;

                selectionBox.Visible = selectionBox.Visible != postObject.Visible 
                    ? postObject.Visible 
                    : selectionBox.Visible;

                db.SetModified(selectionBox);
                await db.SaveChangesAsync();

                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Failed");
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
    }
}
