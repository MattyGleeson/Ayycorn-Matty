namespace SelectionBoxService.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SelectionBoxProduct")]
    public partial class SelectionBoxProduct
    {
        public int Id { get; set; }

        public int SelectionBoxId { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual SelectionBox SelectionBox { get; set; }
    }
}
