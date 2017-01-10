using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibAyycorn.Dtos
{
    public class SelectionBox
    {
        public virtual int Id { get; set; }
        public virtual double Total { get; set; }
        public virtual string Wrapping { get; set; }
        public virtual bool Removed { get; set; }
        public virtual bool Visible { get; set; }
        public virtual bool Available { get; set; }
        public virtual IEnumerable<Product> Products { get; set; }
    }
}
