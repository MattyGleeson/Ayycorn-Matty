using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibAyycorn.Dtos
{
    /// <summary>
    /// Class User.
    /// </summary>
    /// <seealso cref="LibAyycorn.Dto" />
    public class User : Dto
    {
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
    }
}
