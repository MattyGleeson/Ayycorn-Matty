using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibAyycorn.Dtos
{
    /// <summary>
    /// Class Order.
    /// </summary>
    /// <seealso cref="LibAyycorn.Dto"/>
    public class Order : Dto
    {
        public virtual string AccountName { get; set; }
        public virtual string CardNumber { get; set; }
        public virtual int ProductId { get; set; }
        public virtual int Quantity { get; set; }
        public virtual DateTime When { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string ProductEan { get; set; }
        public virtual double TotalPrice { get; set; }
    }
}
