using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibAyycorn.Dtos
{
    /// <summary>
    /// Class Wrapping.
    /// </summary>
    /// <seealso cref="LibAyycorn.Dto" />
    public class Wrapping : Dto
    {
        public virtual double Size { get; set; }
        public virtual double Price { get; set; }
        public virtual string StoreName { get; set; }
        public virtual int TypeId { get; set; }
        public virtual int RangeId { get; set; }
        public virtual string TypeName { get; set; }
        public virtual string RangeName { get; set; }
    }
}
