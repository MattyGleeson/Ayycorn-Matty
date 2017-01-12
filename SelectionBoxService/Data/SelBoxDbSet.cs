using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SelectionBoxService.Data
{
    public class SelBoxDbSet<T> : DbSet<T> where T : class
    {
        public virtual void SetSBAvailable(SelectionBox sb, bool val)
        {
            sb.Available = val;
        }

        public virtual void SetSBVisible(SelectionBox sb, bool val)
        {
            sb.Visible = val;
        }

        public virtual void SetSBRemoved(SelectionBox sb)
        {
            sb.Removed = true;
        }
    }
}