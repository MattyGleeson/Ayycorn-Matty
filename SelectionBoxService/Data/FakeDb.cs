using SelectionBoxService.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace SelectionBoxService.Data
{
    public partial class FakeDb : DbContext, DbInterface
    {
        
        public FakeDb()
        {
            this.Products = Set<Product>();
            this.SelectionBoxes = Set<SelectionBox>();
            this.SelectionBoxProducts = Set<SelectionBoxProduct>();

            Products.RemoveRange(Products);
            SelectionBoxes.RemoveRange(SelectionBoxes);
            SelectionBoxProducts.RemoveRange(SelectionBoxProducts);

            SaveChanges();

            for (int i = 1; i <= 20; i++)
            {
                Products.Add(new Product
                {
                    Name = "Product" + i,
                    Store = "FakeStore",
                    ProductId = i,
                    Id = i
                });
            }

            SaveChanges();

            for (int i = 1; i < 10; i++)
            {
                SelectionBoxes.Add(new SelectionBox
                {
                    Id = i,
                    Total = 10.0,
                    Wrapping = "Paper",
                    Removed = false,
                    Visible = true,
                    Available = true
                });
            }

            SaveChanges();

            for (int i = 0; i <= 20; i++)
            {
                
            }
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<SelectionBox> SelectionBoxes { get; set; }

        public virtual DbSet<SelectionBoxProduct> SelectionBoxProducts { get; set; }
        
    }
}