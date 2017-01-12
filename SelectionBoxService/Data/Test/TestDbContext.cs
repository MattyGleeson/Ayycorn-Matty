using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SelectionBoxService.Data.Test
{
    public class TestDbContext : DbContext
    {
        public override Task<int> SaveChangesAsync()
        {
            return Task.FromResult(SaveChanges());
        }
    }
}