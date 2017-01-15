using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibAyycorn.Dtos
{
    /// <summary>
    /// Class Selection Box.
    /// </summary>
    /// <seealso cref="LibAyycorn.Dto" />
    public class SelectionBox : Dto
    {
        public virtual double Total { get; set; }
        public virtual int WrappingId { get; set; }
        public virtual int WrappingTypeId { get; set; }
        public virtual string WrappingTypeName { get; set; }
        public virtual int WrappingRangeId { get; set; }
        public virtual string WrappingRangeName { get; set; }
        public virtual bool Removed { get; set; }
        public virtual bool Visible { get; set; }
        public virtual bool Available { get; set; }
        public virtual IEnumerable<Product> Products { get; set; }
    }
}
