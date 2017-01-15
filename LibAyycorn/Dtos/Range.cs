using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAyycorn.Dtos
{
    /// <summary>
    /// Class Range.
    /// </summary>
    /// <seealso cref="LibAyycorn.Dto" />
    class Range : Dto
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
