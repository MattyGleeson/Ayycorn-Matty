using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelectionBoxService.Interfaces
{
    public interface IDbContext
    {
        void SetModified(object entity);
    }
}