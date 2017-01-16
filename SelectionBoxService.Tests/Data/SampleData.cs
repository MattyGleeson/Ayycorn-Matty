using Moq;
using SelectionBoxService.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectionBoxService.Tests.Data
{
    public class SampleData : MoqTestHandlers
    {
        public Mock<DbSet<SelectionBox>> boxes;
        public Mock<DbSet<Product>> products;
        public Mock<DbSet<SelectionBoxProduct>> boxProducts;

        public SampleData()
        {
            boxes = GetSelectionBoxes();
            products = GetProducts();
            boxProducts = GetSelectionBoxProducts();
        }

        public SampleData(bool data)
        {
            boxes = GetSelectionBoxes(data);
            products = GetProducts(data);
            boxProducts = GetSelectionBoxProducts(data);
        }

        private Mock<DbSet<SelectionBox>> GetSelectionBoxes(bool data = true)
        {
            IQueryable<SelectionBox> boxesData;

            if (data)
            {
                boxesData = new List<SelectionBox>()
                {
                    new SelectionBox() { Id = 1, Total = 10.0, WrappingId = 1, WrappingRangeId = 1, WrappingRangeName = "Swirl-Tastic", WrappingTypeId = 1, WrappingTypeName = "Gift Bag", Removed = false, Visible = true, Available = true },
                    new SelectionBox() { Id = 2, Total = 10.0, WrappingId = 1, WrappingRangeId = 1, WrappingRangeName = "Swirl-Tastic", WrappingTypeId = 1, WrappingTypeName = "Gift Bag", Removed = false, Visible = true, Available = true },
                    new SelectionBox() { Id = 3, Total = 10.0, WrappingId = 2, WrappingRangeId = 1, WrappingRangeName = "Swirl-Tastic", WrappingTypeId = 1, WrappingTypeName = "Gift Bag", Removed = false, Visible = true, Available = true },
                    new SelectionBox() { Id = 4, Total = 10.0, WrappingId = 2, WrappingRangeId = 1, WrappingRangeName = "Swirl-Tastic", WrappingTypeId = 1, WrappingTypeName = "Gift Bag", Removed = false, Visible = true, Available = true },
                    new SelectionBox() { Id = 5, Total = 10.0, WrappingId = 3, WrappingRangeId = 1, WrappingRangeName = "Swirl-Tastic", WrappingTypeId = 1, WrappingTypeName = "Gift Bag", Removed = false, Visible = true, Available = true },
                    new SelectionBox() { Id = 6, Total = 10.0, WrappingId = 3, WrappingRangeId = 1, WrappingRangeName = "Swirl-Tastic", WrappingTypeId = 1, WrappingTypeName = "Gift Bag", Removed = false, Visible = true, Available = true }
                }.AsQueryable();
            }
            else
            {
                boxesData = Enumerable.Empty<SelectionBox>().AsQueryable();
            }

            Mock<DbSet<SelectionBox>> mockBoxesSet = new Mock<DbSet<SelectionBox>>();
            mockBoxesSet.As<IDbAsyncEnumerable<SelectionBox>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<SelectionBox>(boxesData.GetEnumerator()));
            mockBoxesSet.As<IQueryable<SelectionBox>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<SelectionBox>(boxesData.Provider));
            mockBoxesSet.As<IQueryable>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<SelectionBox>(boxesData.Provider));
            mockBoxesSet.As<IQueryable<SelectionBox>>().Setup(m => m.Expression).Returns(boxesData.Expression);
            mockBoxesSet.As<IQueryable<SelectionBox>>().Setup(m => m.ElementType).Returns(boxesData.ElementType);
            mockBoxesSet.As<IQueryable<SelectionBox>>().Setup(m => m.GetEnumerator()).Returns(boxesData.GetEnumerator());
            mockBoxesSet.Setup(m => m.Add(It.IsAny<SelectionBox>())).Returns((SelectionBox r) => r);

            return mockBoxesSet;
        }

        private Mock<DbSet<Product>> GetProducts(bool data = true)
        {
            IQueryable<Product> productsData;
            if (data)
            {
                productsData = new List<Product>()
                {
                    new Product() { Id = 1, Name = "Product1", ProductId = 1, Store = "TestStore" },
                    new Product() { Id = 2, Name = "Product2", ProductId = 2, Store = "TestStore" },
                    new Product() { Id = 3, Name = "Product3", ProductId = 3, Store = "TestStore" }
                }.AsQueryable();
            }
            else
            {
                productsData = Enumerable.Empty<Product>().AsQueryable();
            }
            

            Mock<DbSet<Product>> mockProductsSet = new Mock<DbSet<Product>>();
            mockProductsSet.As<IDbAsyncEnumerable<Product>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<Product>(productsData.GetEnumerator()));
            mockProductsSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Product>(productsData.Provider));
            mockProductsSet.As<IQueryable>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Product>(productsData.Provider));
            mockProductsSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(productsData.Expression);
            mockProductsSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(productsData.ElementType);
            mockProductsSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(productsData.GetEnumerator());
            mockProductsSet.Setup(m => m.Add(It.IsAny<Product>())).Returns((Product r) => r);

            return mockProductsSet;
        }

        private Mock<DbSet<SelectionBoxProduct>> GetSelectionBoxProducts(bool data = true)
        {
            IEnumerable<SelectionBox> boxes = GetSelectionBoxes(false).Object.ToList();
            IEnumerable<Product> prods = GetProducts(false).Object.ToList();
            IQueryable<SelectionBoxProduct> boxProductsData;
            if (data)
            {
                boxProductsData = new List<SelectionBoxProduct>()
                {
                    CreateSelectionBoxProd(1, 1, 1, boxes, prods),
                    CreateSelectionBoxProd(2, 1, 2, boxes, prods),
                    CreateSelectionBoxProd(3, 2, 1, boxes, prods),
                    CreateSelectionBoxProd(4, 2, 3, boxes, prods),
                    CreateSelectionBoxProd(5, 3, 2, boxes, prods),
                    CreateSelectionBoxProd(6, 3, 3, boxes, prods),
                    CreateSelectionBoxProd(7, 4, 1, boxes, prods),
                    CreateSelectionBoxProd(8, 4, 2, boxes, prods),
                    CreateSelectionBoxProd(9, 5, 1, boxes, prods),
                    CreateSelectionBoxProd(10, 5, 3, boxes, prods),
                    CreateSelectionBoxProd(11, 6, 2, boxes, prods),
                    CreateSelectionBoxProd(12, 6, 3, boxes, prods)
                }.AsQueryable();
            }
            else
            {
                boxProductsData = Enumerable.Empty<SelectionBoxProduct>().AsQueryable();
            }

            Mock<DbSet<SelectionBoxProduct>> mockBoxProductsSet = new Mock<DbSet<SelectionBoxProduct>>();
            mockBoxProductsSet.As<IDbAsyncEnumerable<SelectionBoxProduct>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<SelectionBoxProduct>(boxProductsData.GetEnumerator()));
            mockBoxProductsSet.As<IQueryable<SelectionBoxProduct>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<SelectionBoxProduct>(boxProductsData.Provider));
            mockBoxProductsSet.As<IQueryable>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<SelectionBoxProduct>(boxProductsData.Provider));
            mockBoxProductsSet.As<IQueryable<SelectionBoxProduct>>().Setup(m => m.Expression).Returns(boxProductsData.Expression);
            mockBoxProductsSet.As<IQueryable<SelectionBoxProduct>>().Setup(m => m.ElementType).Returns(boxProductsData.ElementType);
            mockBoxProductsSet.As<IQueryable<SelectionBoxProduct>>().Setup(m => m.GetEnumerator()).Returns(boxProductsData.GetEnumerator());
            mockBoxProductsSet.Setup(m => m.Add(It.IsAny<SelectionBoxProduct>())).Returns((SelectionBoxProduct r) => r);

            return mockBoxProductsSet;
        }

        public Mock<AyycornDb> Context()
        {
            Mock < AyycornDb > mockDb = new Mock<AyycornDb>();
            mockDb.Setup(m => m.SelectionBoxes).Returns(boxes.Object);
            mockDb.Setup(m => m.Products).Returns(products.Object);
            mockDb.Setup(m => m.SelectionBoxProducts).Returns(boxProducts.Object);
            mockDb.Setup(m => m.SetModified(It.IsAny<object>())).Callback((object entity) => { });

            return mockDb;
        }

        private SelectionBoxProduct CreateSelectionBoxProd(int id, int sBoxId, int prodId,
            IEnumerable<SelectionBox> boxes, IEnumerable<Product> products)
        {
            return new SelectionBoxProduct()
            {
                Id = id,
                SelectionBoxId = sBoxId,
                SelectionBox = boxes.Where(b => b.Id == sBoxId).FirstOrDefault(),
                ProductId = prodId,
                Product = products.Where(p => p.Id == prodId).FirstOrDefault()
            };
        }
    }
}
