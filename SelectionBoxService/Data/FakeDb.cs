using SelectionBoxService.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace SelectionBoxService.Data
{
    public partial class FakeDb : DbInterface
    {
        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<SelectionBox> SelectionBoxes { get; set; }

        public virtual DbSet<SelectionBoxProduct> SelectionBoxProducts { get; set; }

        public System.Data.Entity.Infrastructure.DbEntityEntry Entry(object entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}