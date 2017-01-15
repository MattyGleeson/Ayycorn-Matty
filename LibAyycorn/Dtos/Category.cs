using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibAyycorn.Dtos
{
    /// <summary>
    /// Class Category.
    /// </summary>
    /// <seealso cref="LibAyycorn.Dto" />
    public class Category : Dto
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int AvailableProductCount { get; set; }
    }
}
