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
        private DbInterface db;
        private bool fakeData;

        public SelectionBoxController()
        {
            db = new AyycornDb();
        }

        public SelectionBoxController(DbInterface db)
        {
            this.db = db;
        }

        [Route("service/GetBoxes")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllSelectionBoxes()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            IEnumerable<Data.SelectionBox> res = await db.SelectionBoxes.ToListAsync();

            IEnumerable<LibAyycorn.Dtos.SelectionBox> boxes = res.Select(b => new LibAyycorn.Dtos.SelectionBox
            {
                Id = b.Id,
                Total = b.Total,
                Wrapping = b.Wrapping,
                Removed = b.Removed == false ? false : true,
                Visible = b.Visible == false ? false : true,
                Available = b.Available == false ? false : true,
                Products = GetProductsForBox(b)
            });

            return boxes.Any() ?
                Request.CreateResponse(HttpStatusCode.OK, boxes) :
                Request.CreateErrorResponse(HttpStatusCode.NoContent, "No Selection Boxes");
        }

        public async Task<HttpResponseMessage> Test()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Test");
        }

        private IEnumerable<LibAyycorn.Dtos.Product> GetProductsForBox(Data.SelectionBox box)
        {
            IEnumerable<Data.Product> boxProducts = box.SelectionBoxProducts.Select(b => b.Product);
            return boxProducts.Select(b => new LibAyycorn.Dtos.Product
            {
                Id = b.ProductId,
                Name = b.Name,
                Store = b.Store
            });
        }

        [Route("service/PostBox")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostSelectionBox()
        {
            string json = await Request.Content.ReadAsStringAsync();
            LibAyycorn.Dtos.SelectionBox gb = JsonConvert.DeserializeObject<LibAyycorn.Dtos.SelectionBox>(json);

            try
            {
                Data.SelectionBox newSelectionBox = db.SelectionBoxes.Add(new Data.SelectionBox
                {
                    Total = gb.Total,
                    Wrapping = gb.Wrapping,
                    Removed = gb.Removed,
                    Visible = gb.Visible,
                    Available = gb.Available
                });
                await db.SaveChangesAsync();
                
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
                
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Failed");
            }
        }

        [Route("service/DeleteBox/{id:int?}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteSelectionBox(int id)
        {
            try
            {
                Data.SelectionBox selectionBox = await db.SelectionBoxes.Where(sb => sb.Id == id).FirstOrDefaultAsync();
                selectionBox.Removed = true;

                db.Entry(selectionBox).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Failed");
            }
        }

        [Route("service/UpdateBox/{id:int?}")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateSelectionBox(int id)
        {
            string json = await Request.Content.ReadAsStringAsync();
            LibAyycorn.Dtos.SelectionBox postObject = JsonConvert.DeserializeObject<LibAyycorn.Dtos.SelectionBox>(json);

            try
            {
                Data.SelectionBox selectionBox = await db.SelectionBoxes.Where(sb => sb.Id == postObject.Id).FirstOrDefaultAsync();
                
                selectionBox.Available = (selectionBox.Available != postObject.Available) ? 
                    postObject.Available : 
                    selectionBox.Available;

                selectionBox.Visible = (selectionBox.Visible != postObject.Visible) ?
                    postObject.Visible :
                    selectionBox.Visible;

                db.Entry(selectionBox).State = EntityState.Modified;
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
