using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibAyycorn.Dtos
{
    /// <summary>
    /// Class Brand.
    /// </summary>
    /// <seealso cref="LibAyycorn.Dto" />
    public class Brand : Dto
    {
        public virtual string Name { get; set; }
        public virtual int AvailableProductCount { get; set; }
    }
}
