using SelectionBoxService.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SelectionBoxService.Interfaces
{
    public interface DbInterface
    {
        DbSet<Product> Products { get; set; }
        DbSet<SelectionBox> SelectionBoxes { get; set; }
        DbSet<SelectionBoxProduct> SelectionBoxProducts { get; set; }
        Task<int> SaveChangesAsync();
        DbEntityEntry Entry(object entity);
    }
}