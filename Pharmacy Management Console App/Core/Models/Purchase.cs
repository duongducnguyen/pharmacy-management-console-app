using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagement.Core.Models
{
    public class Purchase
    {
        public Purchase()
        {
            PurchaseDetails = new HashSet<PurchaseDetail>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        public decimal TotalAmount { get; set; }

        public int SupplierId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }


}