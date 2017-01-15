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
        public Mock<DbSet<SelectionBox>> Boxes;
        public Mock<DbSet<Product>> Products;
        public Mock<DbSet<SelectionBoxProduct>> BoxProducts;

        public SampleData()
        {
            Boxes = GetSelectionBoxes();
            Products = GetProducts();
            BoxProducts = GetSelectionBoxProducts();
        }

        public SampleData(bool data)
        {
            Boxes = GetSelectionBoxes(data);
            Products = GetProducts(data);
            BoxProducts = GetSelectionBoxProducts(data);
        }

        private Mock<DbSet<SelectionBox>> GetSelectionBoxes(bool data = true)
        {
            IQueryable<SelectionBox> BoxesData;

            if (data)
            {
                BoxesData = new List<SelectionBox>()
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
                BoxesData = Enumerable.Empty<SelectionBox>().AsQueryable();
            }

            Mock<DbSet<SelectionBox>> MockBoxesSet = new Mock<DbSet<SelectionBox>>();
            MockBoxesSet.As<IDbAsyncEnumerable<SelectionBox>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<SelectionBox>(BoxesData.GetEnumerator()));
            MockBoxesSet.As<IQueryable<SelectionBox>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<SelectionBox>(BoxesData.Provider));
            MockBoxesSet.As<IQueryable>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<SelectionBox>(BoxesData.Provider));
            MockBoxesSet.As<IQueryable<SelectionBox>>().Setup(m => m.Expression).Returns(BoxesData.Expression);
            MockBoxesSet.As<IQueryable<SelectionBox>>().Setup(m => m.ElementType).Returns(BoxesData.ElementType);
            MockBoxesSet.As<IQueryable<SelectionBox>>().Setup(m => m.GetEnumerator()).Returns(BoxesData.GetEnumerator());
            MockBoxesSet.Setup(m => m.Add(It.IsAny<SelectionBox>())).Returns((SelectionBox r) => r);

            return MockBoxesSet;
        }

        private Mock<DbSet<Product>> GetProducts(bool data = true)
        {
            IQueryable<Product> ProductsData;
            if (data)
            {
                ProductsData = new List<Product>()
                {
                    new Product() { Id = 1, Name = "Product1", ProductId = 1, Store = "TestStore" },
                    new Product() { Id = 2, Name = "Product2", ProductId = 2, Store = "TestStore" },
                    new Product() { Id = 3, Name = "Product3", ProductId = 3, Store = "TestStore" }
                }.AsQueryable();
            }
            else
            {
                ProductsData = Enumerable.Empty<Product>().AsQueryable();
            }
            

            Mock<DbSet<Product>> MockProductsSet = new Mock<DbSet<Product>>();
            MockProductsSet.As<IDbAsyncEnumerable<Product>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<Product>(ProductsData.GetEnumerator()));
            MockProductsSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Product>(ProductsData.Provider));
            MockProductsSet.As<IQueryable>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Product>(ProductsData.Provider));
            MockProductsSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(ProductsData.Expression);
            MockProductsSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(ProductsData.ElementType);
            MockProductsSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(ProductsData.GetEnumerator());
            MockProductsSet.Setup(m => m.Add(It.IsAny<Product>())).Returns((Product r) => r);

            return MockProductsSet;
        }

        private Mock<DbSet<SelectionBoxProduct>> GetSelectionBoxProducts(bool data = true)
        {
            IEnumerable<SelectionBox> Boxes = GetSelectionBoxes(false).Object.ToList();
            IEnumerable<Product> Prods = GetProducts(false).Object.ToList();
            IQueryable<SelectionBoxProduct> BoxProductsData;
            if (data)
            {
                BoxProductsData = new List<SelectionBoxProduct>()
                {
                    CreateSelectionBoxProd(1, 1, 1, Boxes, Prods),
                    CreateSelectionBoxProd(2, 1, 2, Boxes, Prods),
                    CreateSelectionBoxProd(3, 2, 1, Boxes, Prods),
                    CreateSelectionBoxProd(4, 2, 3, Boxes, Prods),
                    CreateSelectionBoxProd(5, 3, 2, Boxes, Prods),
                    CreateSelectionBoxProd(6, 3, 3, Boxes, Prods),
                    CreateSelectionBoxProd(7, 4, 1, Boxes, Prods),
                    CreateSelectionBoxProd(8, 4, 2, Boxes, Prods),
                    CreateSelectionBoxProd(9, 5, 1, Boxes, Prods),
                    CreateSelectionBoxProd(10, 5, 3, Boxes, Prods),
                    CreateSelectionBoxProd(11, 6, 2, Boxes, Prods),
                    CreateSelectionBoxProd(12, 6, 3, Boxes, Prods)
                }.AsQueryable();
            }
            else
            {
                BoxProductsData = Enumerable.Empty<SelectionBoxProduct>().AsQueryable();
            }

            Mock<DbSet<SelectionBoxProduct>> MockBoxProductsSet = new Mock<DbSet<SelectionBoxProduct>>();
            MockBoxProductsSet.As<IDbAsyncEnumerable<SelectionBoxProduct>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<SelectionBoxProduct>(BoxProductsData.GetEnumerator()));
            MockBoxProductsSet.As<IQueryable<SelectionBoxProduct>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<SelectionBoxProduct>(BoxProductsData.Provider));
            MockBoxProductsSet.As<IQueryable>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<SelectionBoxProduct>(BoxProductsData.Provider));
            MockBoxProductsSet.As<IQueryable<SelectionBoxProduct>>().Setup(m => m.Expression).Returns(BoxProductsData.Expression);
            MockBoxProductsSet.As<IQueryable<SelectionBoxProduct>>().Setup(m => m.ElementType).Returns(BoxProductsData.ElementType);
            MockBoxProductsSet.As<IQueryable<SelectionBoxProduct>>().Setup(m => m.GetEnumerator()).Returns(BoxProductsData.GetEnumerator());
            MockBoxProductsSet.Setup(m => m.Add(It.IsAny<SelectionBoxProduct>())).Returns((SelectionBoxProduct r) => r);

            return MockBoxProductsSet;
        }

        public Mock<AyycornDb> Context()
        {
            Mock < AyycornDb > MockDb = new Mock<AyycornDb>();
            MockDb.Setup(m => m.SelectionBoxes).Returns(Boxes.Object);
            MockDb.Setup(m => m.Products).Returns(Products.Object);
            MockDb.Setup(m => m.SelectionBoxProducts).Returns(BoxProducts.Object);
            MockDb.Setup(m => m.SetModified(It.IsAny<object>())).Callback((object entity) => { });

            return MockDb;
        }

        private SelectionBoxProduct CreateSelectionBoxProd(int Id, int SBoxId, int ProdId,
            IEnumerable<SelectionBox> Boxes, IEnumerable<Product> Products)
        {
            return new SelectionBoxProduct()
            {
                Id = Id,
                SelectionBoxId = SBoxId,
                SelectionBox = Boxes.Where(b => b.Id == SBoxId).FirstOrDefault(),
                ProductId = ProdId,
                Product = Products.Where(p => p.Id == ProdId).FirstOrDefault()
            };
        }
    }
}
