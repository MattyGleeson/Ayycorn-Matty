using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibAyycorn.Dtos
{
    public class Brand
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int AvailableProductCount { get; set; }
    }
}
