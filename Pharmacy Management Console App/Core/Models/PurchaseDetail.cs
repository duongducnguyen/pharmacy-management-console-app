using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagement.Core.Models
{
    public class PurchaseDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public decimal Subtotal { get; set; }

        public int PurchaseId { get; set; }
        public int MedicineId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("PurchaseId")]
        public virtual Purchase Purchase { get; set; }

        [ForeignKey("MedicineId")]
        public virtual Medicine Medicine { get; set; }
    }


}