using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibAyycorn.Dtos
{
    public class Wrapping
    {
        public virtual int Id { get; set; }
        public virtual double Size { get; set; }
        public virtual double Price { get; set; }
        public virtual string StoreName { get; set; }
        public virtual int TypeId { get; set; }
        public virtual int RangeId { get; set; }
        public virtual string TypeName { get; set; }
        public virtual string RangeName { get; set; }
    }
}
