namespace SelectionBoxService.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SelectionBox")]
    public partial class SelectionBox
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SelectionBox()
        {
            SelectionBoxProducts = new HashSet<SelectionBoxProduct>();
        }

        public int Id { get; set; }

        public double Total { get; set; }

        public int WrappingTypeId { get; set; }

        public bool? Available { get; set; }

        public bool? Visible { get; set; }

        public bool? Removed { get; set; }

        [Required]
        [StringLength(20)]
        public string WrappingTypeName { get; set; }

        [Required]
        [StringLength(20)]
        public string WrappingRangeName { get; set; }

        public int WrappingRangeId { get; set; }

        public int WrappingId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SelectionBoxProduct> SelectionBoxProducts { get; set; }
    }
}
