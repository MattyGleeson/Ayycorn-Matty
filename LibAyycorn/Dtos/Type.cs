using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAyycorn.Dtos
{
    /// <summary>
    /// Class Type.
    /// </summary>
    /// <seealso cref="LibAyycorn.Dto" />
    class Type : Dto
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
