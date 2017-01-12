using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using SelectionBoxService.Controllers;
using System.Data.Entity;
using SelectionBoxService.Data;
using System.Linq;
using System.Net;
using Moq;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using SelectionBoxService.Data.Test;

namespace SelectionBoxService.Tests
{
    [TestClass]
    public class UnitTest
    {
        private SelectionBoxController Controller;
        private Mock<AyycornDb> MockDb;
        private AyycornDb Db;
        private Mock<SelBoxDbSet<SelectionBox>> MockBoxesSet;
        private Mock<DbSet<Product>> MockProductsSet;
        private Mock<DbSet<SelectionBoxProduct>> MockBoxProductsSet;

        [TestInitialize]
        public void Init()
        {
            IEnumerable<SelectionBox> Boxes = new List<SelectionBox>()
            {
                new SelectionBox() { Id = 1, Total = 10.0, Wrapping = "Paper", Removed = false, Visible = true, Available = true },
                new SelectionBox() { Id = 2, Total = 10.0, Wrapping = "Paper", Removed = false, Visible = true, Available = true },
                new SelectionBox() { Id = 3, Total = 10.0, Wrapping = "Paper", Removed = false, Visible = true, Available = true },
                new SelectionBox() { Id = 4, Total = 10.0, Wrapping = "Paper", Removed = false, Visible = true, Available = true },
                new SelectionBox() { Id = 5, Total = 10.0, Wrapping = "Paper", Removed = false, Visible = true, Available = true },
                new SelectionBox() { Id = 6, Total = 10.0, Wrapping = "Paper", Removed = false, Visible = true, Available = true }
            };

            IEnumerable<Product> Products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Product1", ProductId = 1, Store = "TestStore" },
                new Product() { Id = 2, Name = "Product2", ProductId = 2, Store = "TestStore" },
                new Product() { Id = 3, Name = "Product3", ProductId = 3, Store = "TestStore" }
            };

            IEnumerable<SelectionBoxProduct> BoxProducts = new List<SelectionBoxProduct>()
            {
                CreateSelectionBoxProd(1, 1, 1, Boxes, Products),
                CreateSelectionBoxProd(2, 1, 2, Boxes, Products),
                CreateSelectionBoxProd(3, 2, 1, Boxes, Products),
                CreateSelectionBoxProd(4, 2, 3, Boxes, Products),
                CreateSelectionBoxProd(5, 3, 2, Boxes, Products),
                CreateSelectionBoxProd(6, 3, 3, Boxes, Products),
                CreateSelectionBoxProd(7, 4, 1, Boxes, Products),
                CreateSelectionBoxProd(8, 4, 2, Boxes, Products),
                CreateSelectionBoxProd(9, 5, 1, Boxes, Products),
                CreateSelectionBoxProd(10, 5, 3, Boxes, Products),
                CreateSelectionBoxProd(11, 6, 2, Boxes, Products),
                CreateSelectionBoxProd(12, 6, 3, Boxes, Products)
            };

            IQueryable<SelectionBox> BoxesData = Boxes.AsQueryable();
            IQueryable<Product> ProductsData = Products.AsQueryable();
            IQueryable<SelectionBoxProduct> BoxProductsData = BoxProducts.AsQueryable();

            //MockBoxesSet = GenerateMockDbSet<SelectionBox>(BoxesData);
            //MockProductsSet = GenerateMockDbSet<Product>(ProductsData);
            //MockBoxProductsSet = GenerateMockDbSet<SelectionBoxProduct>(BoxProductsData);

            MockBoxesSet = new Mock<SelBoxDbSet<SelectionBox>>();
            MockBoxesSet.As<IDbAsyncEnumerable<SelectionBox>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<SelectionBox>(BoxesData.GetEnumerator()));
            MockBoxesSet.As<IQueryable<SelectionBox>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<SelectionBox>(BoxesData.Provider));
            MockBoxesSet.As<IQueryable>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<SelectionBox>(BoxesData.Provider));
            MockBoxesSet.As<IQueryable<SelectionBox>>().Setup(m => m.Expression).Returns(BoxesData.Expression);
            MockBoxesSet.As<IQueryable<SelectionBox>>().Setup(m => m.ElementType).Returns(BoxesData.ElementType);
            MockBoxesSet.As<IQueryable<SelectionBox>>().Setup(m => m.GetEnumerator()).Returns(BoxesData.GetEnumerator());
            MockBoxesSet.Setup(m => m.Add(It.IsAny<SelectionBox>())).Returns((SelectionBox r) => r);
            MockBoxesSet.Setup(m => m.SetSBAvailable(It.IsAny<SelectionBox>(), It.IsAny<bool>())).Verifiable();
            MockBoxesSet.Setup(m => m.SetSBVisible(It.IsAny<SelectionBox>(), It.IsAny<bool>())).Verifiable();
            MockBoxesSet.Setup(m => m.SetSBRemoved(It.IsAny<SelectionBox>())).Verifiable();

            MockProductsSet = new Mock<DbSet<Product>>();
            MockProductsSet.As<IDbAsyncEnumerable<Product>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<Product>(ProductsData.GetEnumerator()));
            MockProductsSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Product>(ProductsData.Provider));
            MockProductsSet.As<IQueryable>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Product>(ProductsData.Provider));
            MockProductsSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(ProductsData.Expression);
            MockProductsSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(ProductsData.ElementType);
            MockProductsSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(ProductsData.GetEnumerator());
            MockProductsSet.Setup(m => m.Add(It.IsAny<Product>())).Returns((Product r) => r);


            MockBoxProductsSet = new Mock<DbSet<SelectionBoxProduct>>();
            MockBoxProductsSet.As<IDbAsyncEnumerable<SelectionBoxProduct>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<SelectionBoxProduct>(BoxProductsData.GetEnumerator()));
            MockBoxProductsSet.As<IQueryable<SelectionBoxProduct>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<SelectionBoxProduct>(BoxProductsData.Provider));
            MockBoxProductsSet.As<IQueryable>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<SelectionBoxProduct>(BoxProductsData.Provider));
            MockBoxProductsSet.As<IQueryable<SelectionBoxProduct>>().Setup(m => m.Expression).Returns(BoxProductsData.Expression);
            MockBoxProductsSet.As<IQueryable<SelectionBoxProduct>>().Setup(m => m.ElementType).Returns(BoxProductsData.ElementType);
            MockBoxProductsSet.As<IQueryable<SelectionBoxProduct>>().Setup(m => m.GetEnumerator()).Returns(BoxProductsData.GetEnumerator());
            MockBoxProductsSet.Setup(m => m.Add(It.IsAny<SelectionBoxProduct>())).Returns((SelectionBoxProduct r) => r);

            MockDb = new Mock<AyycornDb>();
            MockDb.Setup(m => m.SelectionBoxes).Returns(MockBoxesSet.Object);
            MockDb.Setup(m => m.Products).Returns(MockProductsSet.Object);
            MockDb.Setup(m => m.SelectionBoxProducts).Returns(MockBoxProductsSet.Object);
            MockDb.Setup(m => m.SetModified(It.IsAny<object>())).Callback((object entity) => { });

            Db = MockDb.Object;

            Controller = new SelectionBoxController(MockDb.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        private Mock<DbSet<T>> GenerateMockDbSet<T>(IQueryable<T> Data) where T : class
        {
            Mock<DbSet<T>> MockSet = new Mock<DbSet<T>>();
            MockSet.As<IDbAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<T>(Data.GetEnumerator()));
            MockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<T>(Data.Provider));
            MockSet.As<IQueryable>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<T>(Data.Provider));
            MockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(Data.Provider);
            MockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(Data.Expression);
            MockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(Data.ElementType);
            MockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(Data.GetEnumerator());
            MockSet.Setup(m => m.Add(It.IsAny<T>())).Returns((T r) => r);

            return MockSet;
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

        [TestMethod]
        public async Task TestGet()
        {
            HttpResponseMessage response = await Controller.GetAllSelectionBoxes();

            IEnumerable<LibAyycorn.Dtos.SelectionBox> selectionBoxes;
            IEnumerable<SelectionBox> dbSelectionBoxes = Db.SelectionBoxes.ToList();

            Assert.IsTrue(response.TryGetContentValue(out selectionBoxes));
            Assert.AreEqual(selectionBoxes.Count(), dbSelectionBoxes.Count());
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task TestPostNoProducts()
        {
            HttpResponseMessage response = await Controller.PostSelectionBox(new LibAyycorn.Dtos.SelectionBox
            {
                Id = 7,
                Total = 10.0,
                Wrapping = "Paper",
                Removed = false,
                Visible = true,
                Available = true
            });

            Assert.IsTrue(response.IsSuccessStatusCode);

            MockBoxesSet.Verify(m => m.Add(It.IsAny<SelectionBox>()), Times.Once);
            MockDb.Verify(m => m.SaveChangesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task TestPostSingleProduct()
        {
            HttpResponseMessage response = await Controller.PostSelectionBox(new LibAyycorn.Dtos.SelectionBox
            {
                Id = 7,
                Total = 10.0,
                Wrapping = "Paper",
                Removed = false,
                Visible = true,
                Available = true,
                Products = new List<LibAyycorn.Dtos.Product>()
                {
                    new LibAyycorn.Dtos.Product
                    {
                        Id = 4,
                        Name = "Product4",
                        Store = "TestStore"
                    }
                }
            });

            Assert.IsTrue(response.IsSuccessStatusCode);

            MockBoxesSet.Verify(m => m.Add(It.IsAny<SelectionBox>()), Times.Once);
            MockBoxProductsSet.Verify(m => m.Add(It.IsAny<SelectionBoxProduct>()), Times.Once);
            MockProductsSet.Verify(m => m.Add(It.IsAny<Product>()), Times.Once);
            MockDb.Verify(m => m.SaveChangesAsync(), Times.Exactly(3));
        }

        [TestMethod]
        public async Task TestPostMultipleProducts()
        {
            HttpResponseMessage response = await Controller.PostSelectionBox(new LibAyycorn.Dtos.SelectionBox
            {
                Id = 8,
                Total = 10.0,
                Wrapping = "Paper",
                Removed = false,
                Visible = true,
                Available = true,
                Products = new List<LibAyycorn.Dtos.Product>()
                {
                    new LibAyycorn.Dtos.Product
                    {
                        Id = 5,
                        Name = "Product5",
                        Store = "TestStore"
                    },
                    new LibAyycorn.Dtos.Product
                    {
                        Id = 6,
                        Name = "Product6",
                        Store = "TestStore"
                    }
                }
            });

            Assert.IsTrue(response.IsSuccessStatusCode);

            MockBoxesSet.Verify(m => m.Add(It.IsAny<SelectionBox>()), Times.Once);
            MockBoxProductsSet.Verify(m => m.Add(It.IsAny<SelectionBoxProduct>()), Times.Exactly(2));
            MockProductsSet.Verify(m => m.Add(It.IsAny<Product>()), Times.Exactly(2));
            MockDb.Verify(m => m.SaveChangesAsync(), Times.Exactly(5));
        }

        [TestMethod]
        public async Task TestPostExistingProduct()
        {
            HttpResponseMessage response = await Controller.PostSelectionBox(new LibAyycorn.Dtos.SelectionBox
            {
                Id = 9,
                Total = 10.0,
                Wrapping = "Paper",
                Removed = false,
                Visible = true,
                Available = true,
                Products = new List<LibAyycorn.Dtos.Product>()
                {
                    new LibAyycorn.Dtos.Product
                    {
                        Id = 2,
                        Name = "Product2",
                        Store = "TestStore"
                    }
                }
            });

            Assert.IsTrue(response.IsSuccessStatusCode);

            MockBoxesSet.Verify(m => m.Add(It.IsAny<SelectionBox>()), Times.Once);
            MockBoxProductsSet.Verify(m => m.Add(It.IsAny<SelectionBoxProduct>()), Times.Once);
            MockProductsSet.Verify(m => m.Add(It.IsAny<Product>()), Times.Never);
            MockDb.Verify(m => m.SaveChangesAsync(), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestPutChangeAvailable()
        {
            HttpResponseMessage response = await Controller.UpdateSelectionBox(1, new LibAyycorn.Dtos.SelectionBox
            {
                Id = 1,
                Total = 10.0,
                Wrapping = "Paper",
                Removed = false,
                Visible = true,
                Available = false
            });

            Assert.IsTrue(response.IsSuccessStatusCode);

            MockBoxesSet.Verify(m => m.SetSBAvailable(It.IsAny<SelectionBox>(), It.IsAny<bool>()), Times.Once);
            MockBoxesSet.Verify(m => m.SetSBVisible(It.IsAny<SelectionBox>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public async Task TestPutChangeVisible()
        {
            HttpResponseMessage response = await Controller.UpdateSelectionBox(2, new LibAyycorn.Dtos.SelectionBox
            {
                Id = 2,
                Total = 10.0,
                Wrapping = "Paper",
                Removed = false,
                Visible = false,
                Available = true
            });

            Assert.IsTrue(response.IsSuccessStatusCode);

            MockBoxesSet.Verify(m => m.SetSBAvailable(It.IsAny<SelectionBox>(), It.IsAny<bool>()), Times.Never);
            MockBoxesSet.Verify(m => m.SetSBVisible(It.IsAny<SelectionBox>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public async Task TestPutChangeAvailableAndVisible()
        {
            HttpResponseMessage response = await Controller.UpdateSelectionBox(2, new LibAyycorn.Dtos.SelectionBox
            {
                Id = 3,
                Total = 10.0,
                Wrapping = "Paper",
                Removed = false,
                Visible = false,
                Available = false
            });

            Assert.IsTrue(response.IsSuccessStatusCode);

            MockBoxesSet.Verify(m => m.SetSBAvailable(It.IsAny<SelectionBox>(), It.IsAny<bool>()), Times.Once);
            MockBoxesSet.Verify(m => m.SetSBVisible(It.IsAny<SelectionBox>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public async Task TestPutNoChanges()
        {
            HttpResponseMessage response = await Controller.UpdateSelectionBox(2, new LibAyycorn.Dtos.SelectionBox
            {
                Id = 4,
                Total = 10.0,
                Wrapping = "Paper",
                Removed = false,
                Visible = true,
                Available = true
            });

            Assert.IsTrue(response.IsSuccessStatusCode);

            MockBoxesSet.Verify(m => m.SetSBAvailable(It.IsAny<SelectionBox>(), It.IsAny<bool>()), Times.Never);
            MockBoxesSet.Verify(m => m.SetSBVisible(It.IsAny<SelectionBox>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        public async Task TestRemove()
        {
            HttpResponseMessage response = await Controller.DeleteSelectionBox(5);

            Assert.IsTrue(response.IsSuccessStatusCode);

            MockBoxesSet.Verify(m => m.SetSBRemoved(It.IsAny<SelectionBox>()), Times.Once);
        }

    }

    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }
}
